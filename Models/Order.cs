using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testmongo.Models
{
    public class Order
    {
        [BsonId]     //it uses because object id is primary for ourdatabase
        public ObjectId Id { get; set; }

        [BsonElement("CustomerID")]
        public ObjectId CustomerID { get; set; }

        [BsonElement("ShipName")]
        public string ShipName { get; set; }

        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

        [BsonElement("ShipMobile")]
        public string ShipMobile { get; set; }

        [BsonElement("ShipAddress")]
        public string ShipAddress { get; set; }

        [BsonElement("ShipEmail")]
        public string ShipEmail { get; set; }

        [BsonElement("Address")]
        public string Address { get; set; }

        [BsonElement("Status")]
        public bool Status { get; set; }
    }
}