using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testmongo.Models
{
    public class Cart
    {
        [BsonId]     //it uses because object id is primary for ourdatabase
        public ObjectId Id { get; set; }


        [BsonElement("ProductID")]
        public string ProductID { get; set; }

        [BsonElement("UserID")]
        public string UserID { get; set; }

        [BsonElement("Quantity")]
        public string Quantity { get; set; }

        [BsonElement("Price")]
        public string Price { get; set; }
    }
}