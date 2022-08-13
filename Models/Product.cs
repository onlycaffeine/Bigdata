using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace testmongo.Models
{
    [BsonIgnoreExtraElements]
    public class Product
    {
        [BsonId]     //it uses because object id is primary for ourdatabase
        public ObjectId Id { get; set; }


        [BsonElement("ProductName")]
        public string ProductName { get; set; }



        [BsonElement("ProductDescription")]
        public string ProductDescription { get; set; }


        [BsonElement("Quantity")]
        public int Quantity { get; set; }

        [BsonElement("Price")]
        public int? Price { get; set; }

        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

        [BsonElement("Status")]
        public bool Status { get; set; }

        [BsonElement("Tophot")]
        public DateTime TopHot { get; set; }

        [BsonElement("ProductImage")]
        public string ProductImage { get; set; }

        [BsonElement("CategoryID")]
        public string CategoryID { get; set; }

        [BsonElement("MetaTitle")]
        public string MetaTitle { get; set; }
    }
}