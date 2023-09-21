using System;
using System.Collections.Generic;
using MongoDB.Driver;
using policies.Models;
using policies.Services;

namespace policies
{
    public class PoliciesService : IPoliciesService
    {
        private IMongoCollection<Policy> _policies;

        public PoliciesService(IPoliciesSettings settings)
        {
            var client = new MongoClient(settings.Server);
            var database = client.GetDatabase(settings.Database);
            _policies = database.GetCollection<Policy>(settings.Collection);
        }

        public List<Policy> Get() => _policies.Find(p=> true).ToList();
        public void Create(Policy policy)
        {
            if (policy.PolicyStartDate >= policy.PolicyEndDate)
            {
                throw new InvalidOperationException("La fecha de inicio debe ser anterior a la fecha de fin de vigencia.");
            }

            policy.Id = null; 
            _policies.InsertOne(policy);
        }
        public List<Policy> GetPolicyByPlateOrNumber(string plateOrNumber)
        {
            var filter = Builders<Policy>.Filter.Or(
                Builders<Policy>.Filter.Eq(policy => policy.VehiclePlate, plateOrNumber),
                Builders<Policy>.Filter.Eq(policy => policy.PolicyNumber, plateOrNumber)
            );

            return _policies.Find(filter).ToList();
        }

    }
}