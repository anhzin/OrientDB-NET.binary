﻿using System.Linq;
using Orient.Client.Protocol.Serializers;

namespace Orient.Client.Protocol.Operations
{
    internal class DbExist : IOperation
    {
        internal string DatabaseName { get; set; }

        public Request Request(int sessionID)
        {
            Request request = new Request();

            // standard request fields
            request.DataItems.Add(new RequestDataItem() { Type = "byte", Data = BinarySerializer.ToArray((byte)OperationType.DB_EXIST) });
            request.DataItems.Add(new RequestDataItem() { Type = "int", Data = BinarySerializer.ToArray(sessionID) });
            // operation specific fields
            request.DataItems.Add(new RequestDataItem() { Type = "string", Data = BinarySerializer.ToArray(DatabaseName) });

            return request;
        }

        public ODataObject Response(Response response)
        {
            // start from this position since standard fields (status, session ID) has been already parsed
            int offset = 5;
            ODataObject dataObject = new ODataObject();

            if (response == null)
            {
                return dataObject;
            }

            // operation specific fields
            byte existByte = BinarySerializer.ToByte(response.Data.Skip(offset).Take(1).ToArray());
            offset += 1;

            if (existByte == 0)
            {
                dataObject.Set("Exists", false);
            }
            else
            {
                dataObject.Set("Exists", true);
            }

            return dataObject;
        }
    }
}
