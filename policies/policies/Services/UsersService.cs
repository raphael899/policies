using System;
using MongoDB.Driver;
using policies.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using Amazon.Auth.AccessControlPolicy;

namespace policies.Services
{
	public class UsersService
	{
        private IMongoCollection<User> _users;

        public UsersService(IUsersSettings settings)
		{
            var client = new MongoClient(settings.Server);
            var database = client.GetDatabase(settings.Database);
            _users = database.GetCollection<User>(settings.Collection);
        }


        public User GetByUsername(string username)
        {
            return _users.Find(user => user.UserName == username).FirstOrDefault();
        }

        public void CreateUser(User user)
        {
            user.Id = null;
            _users.InsertOne(user);
        }

    }
}

