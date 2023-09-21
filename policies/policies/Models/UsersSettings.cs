using System;
namespace policies.Models
{
	public class UsersSettings : IUsersSettings
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
	}

    public interface IUsersSettings
    {
        string Server { get; set; }
        string Database { get; set; }
        string Collection { get; set; }
    }

}

