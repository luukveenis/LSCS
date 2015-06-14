using System;
using MongoDB.Bson.Serialization.Attributes;

namespace LSCS.Models
{
    public class DtoBase
    {
        [BsonId]
        public Guid? Id { get; set; }
        public Guid? Nonce { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
    }
}
