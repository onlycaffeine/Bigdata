using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testmongo.Models
{
    public class Menu
    {
        [BsonId]     //it uses because object id is primary for ourdatabase
        public ObjectId Id { get; set; }


        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Text")]
        public string Text { get; set; }

        [BsonElement("Target")]
        public string Target { get; set; }

        [BsonElement("Link")]
        public string Link { get; set; }

        [BsonElement("DisplayOrder")]
        public string DisplayOrder { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; }

        [BsonElement("MenuTypeID")]
        public string MenuTypeID { get; set; }

        [BsonElement("MenuParentID")]
        public string MenuParentID { get; set; }
    }
}