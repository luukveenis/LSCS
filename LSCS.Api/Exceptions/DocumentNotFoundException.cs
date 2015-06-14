using System;

namespace LSCS.Api.Exceptions
{
    public class DocumentNotFoundException : LscsApiException
    {
        public DocumentNotFoundException() { }

        public DocumentNotFoundException(string message) : base(message) { }

        public DocumentNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}