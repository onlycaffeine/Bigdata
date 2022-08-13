using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testmongo.Models
{
    [BsonIgnoreExtraElements]
    public class Employee
    {
        //[BsonRepresentation(BsonType.ObjectId)]

        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Department")]
        public string Department { get; set; }
        [BsonElement("Address")]
        public string Address { get; set; }
        [BsonElement("City")]
        public string City { get; set; }
        [BsonElement("Country")]
        public string Country { get; set; }
    }
}