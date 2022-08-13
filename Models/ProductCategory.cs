using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testmongo.Models
{
    public class ProductCategory
    {
        [BsonId]     //it uses because object id is primary for ourdatabase
        public ObjectId Id { get; set; }

        [BsonElement("CategoryName")]
        public string CategoryName { get; set; }

        [BsonElement("Status")]
        public bool Status { get; set; }

        [BsonElement("MetaTitle")]
        public string MetaTitle { get; set; }

        [BsonElement("CategoryId")]
        public string CategoryId { get; set; }
    }
}