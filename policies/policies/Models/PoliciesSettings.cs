using System;
namespace policies.Models
{
	public class PoliciesSettings: IPoliciesSettings
	{
        public string Server { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }

    public interface IPoliciesSettings
    {
        string Server { get; set; }
        string Database { get; set; }
        string Collection { get; set; }
    }
}

