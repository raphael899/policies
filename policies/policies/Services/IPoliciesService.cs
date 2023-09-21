using System;
using System.Collections.Generic;
using policies.Models;
namespace policies.Services
{
	public interface IPoliciesService
	{
        List<Policy> Get();
        void Create(Policy policy);
        List<Policy> GetPolicyByPlateOrNumber(string plateOrNumber);
    }
}

