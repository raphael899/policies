using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace policies.Models
{
    public class Policy
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("PolicyNumber")]
        public string PolicyNumber { get; set; }

        [BsonElement("CustomerName")]
        public string CustomerName { get; set; }

        [BsonElement("CustomerId")]
        public string CustomerId { get; set; }

        [BsonElement("CustomerDateOfBirth")]
        public DateTime CustomerDateOfBirth { get; set; }

        [BsonElement("PolicyStartDate")]
        public DateTime PolicyStartDate { get; set; }

        [BsonElement("PolicyEndDate")]
        public DateTime PolicyEndDate { get; set; }

        [BsonElement("Coverages")]
        public List<string> Coverages { get; set; }

        [BsonElement("MaxCoverageAmount")]
        public decimal MaxCoverageAmount { get; set; }

        [BsonElement("PlanName")]
        public string PlanName { get; set; }

        [BsonElement("CustomerCity")]
        public string CustomerCity { get; set; }

        [BsonElement("CustomerAddress")]
        public string CustomerAddress { get; set; }

        [BsonElement("VehiclePlate")]
        public string VehiclePlate { get; set; }

        [BsonElement("VehicleModel")]
        public string VehicleModel { get; set; }

        [BsonElement("HasVehicleInspection")]
        public bool HasVehicleInspection { get; set; }
    }
}
