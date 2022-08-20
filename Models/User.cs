using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testmongo.Models
{
    public class User
    {
        [BsonId]     //it uses because object id is primary for ourdatabase
        public ObjectId Id { get; set; }


        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("Password")]
        public string Password { get; set; }

        [BsonElement("Status")]
        public bool Status { get; set; }
    }
}