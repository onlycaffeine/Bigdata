using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testmongo.Models
{
    public class OrderDetail
    {
        [BsonId]     //it uses because object id is primary for ourdatabase
        public ObjectId Id { get; set; }

        [BsonElement("ProductID")]
        public ObjectId ProductID { get; set; }

        [BsonElement("OrderID")]
        public ObjectId OrderID { get; set; }

        [BsonElement("Quantity")]
        public int Quantity { get; set; }

        [BsonElement("Price")]
        public int Price { get; set; }

        [BsonElement("StatusID")]
        public string StatusID { get; set; }

        [BsonElement("StarRated")]
        public string StarRated { get; set; }

        [BsonElement("Comment")]
        public string Comment { get; set; }

    }
}