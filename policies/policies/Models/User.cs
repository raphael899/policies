using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace policies.Models
{
	public class User
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("UserName")]
        public string UserName { get; set; }

        [BsonElement("HashPassword")]
        public string HashPassword { get; set; }
    }
}

