namespace ApiTest;
using policies.Models;
using policies.Controllers;
using policies.Services;
using Microsoft.AspNetCore.Mvc;
using policies;
using Moq;
public class UnitTest1
{

    [Fact]
    public void Get_ReturnsListOfPolicies()
    {
        // Arrange
        var mockPoliciesService = new Mock<IPoliciesService>();
        mockPoliciesService.Setup(service => service.Get())
            .Returns(new List<Policy>
            {
                    new Policy {
                        PolicyNumber = "123ABCD",
                        CustomerName = "Raphael",
                        CustomerId = "123Raphael",
                        CustomerDateOfBirth = DateTime.Parse("2023-09-20T19:17:53.471Z"),
                        PolicyStartDate = DateTime.Parse("2023-09-10T19:17:53.471Z"),
                        PolicyEndDate = DateTime.Parse("2023-09-20T19:17:53.471Z"),
                        Coverages = new List<string> { "string" },
                        MaxCoverageAmount = 20,
                        PlanName = "Gold",
                        CustomerCity = "Quito",
                        CustomerAddress = "Rpe",
                        VehiclePlate = "ABC124",
                        VehicleModel = "Sedan",
                        HasVehicleInspection = true

                    },
                    new Policy {
                        PolicyNumber = "211ABCD",
                        CustomerName = "Raphael22",
                        CustomerId = "1234Raphael",
                        CustomerDateOfBirth = DateTime.Parse("2023-09-20T19:17:53.471Z"),
                        PolicyStartDate = DateTime.Parse("2023-09-10T19:17:53.471Z"),
                        PolicyEndDate = DateTime.Parse("2023-09-20T19:17:53.471Z"),
                        Coverages = new List<string> { "string" },
                        MaxCoverageAmount = 20,
                        PlanName = "Gold",
                        CustomerCity = "Quito",
                        CustomerAddress = "Rpe",
                        VehiclePlate = "BCD234",
                        VehicleModel = "Sedan",
                        HasVehicleInspection = true
                    }
            });

        var controller = new PoliciesController(mockPoliciesService.Object);

        // Act
        var result = controller.Get();

        // Assert
        var okResult = Assert.IsType<ActionResult<List<Policy>>>(result);
        Assert.True(okResult.Result is OkObjectResult);
        var policies = Assert.IsAssignableFrom<List<Policy>>(((OkObjectResult)okResult.Result).Value);
        Assert.Equal(2, policies.Count);

    }





    [Fact]
    public void CreatePolicy_ReturnsCreatedAtRoute()
    {
        // Arrange
        var mockPoliciesService = new Mock<IPoliciesService>();
        var policyToCreate = new Policy
        {
            PolicyNumber = "123ABCD",
            CustomerName = "Raphael",
            CustomerId = "123Raphael",
            CustomerDateOfBirth = DateTime.Parse("2023-09-20T19:17:53.471Z"),
            PolicyStartDate = DateTime.Parse("2023-09-10T19:17:53.471Z"),
            PolicyEndDate = DateTime.Parse("2023-09-20T19:17:53.471Z"),
            Coverages = new List<string> { "string" },
            MaxCoverageAmount = 20,
            PlanName = "Gold",
            CustomerCity = "Quito",
            CustomerAddress = "Rpe",
            VehiclePlate = "ABC124",
            VehicleModel = "Sedan",
            HasVehicleInspection = true
        };

        mockPoliciesService.Setup(service => service.Create(policyToCreate)).Verifiable();
        var controller = new PoliciesController(mockPoliciesService.Object);

        // Act
        var result = controller.CreatePolicy(policyToCreate);

        // Assert
        var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
        Assert.Equal(201, createdAtRouteResult.StatusCode);
        Assert.Equal("GetPolicy", createdAtRouteResult.RouteName);
        Assert.Equal(policyToCreate.Id, createdAtRouteResult.RouteValues["id"]);
    }


    [Fact]
    public void GetPolicyByPlateOrNumber_ReturnsBadRequestWhenNoPoliciesExist()
    {
        // Arrange
        var mockPoliciesService = new Mock<IPoliciesService>();
        var plateOrNumber = "NonExistent";
        mockPoliciesService.Setup(service => service.GetPolicyByPlateOrNumber(plateOrNumber))
            .Returns(new List<Policy>());

        var controller = new PoliciesController(mockPoliciesService.Object);

        // Act
        var result = controller.GetPolicyByPlateOrNumber(plateOrNumber);

        // Assert
        var badRequestResult = Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
        var message = (string)badRequestResult.Value.GetType().GetProperty("message")?.GetValue(badRequestResult.Value, null);
        Assert.Equal("No policies found for the provided plate or policy number.", message);
    }

    [Fact]
    public void GetPolicyByPlateOrNumber_ReturnsOkWhenPoliciesExist()
    {
        // Arrange
        var mockPoliciesService = new Mock<IPoliciesService>();
        var plateOrNumber = "ExistingPlateOrNumber"; // Plate o número existente
        var policies = new List<Policy>
    {
        new Policy
        {
            PolicyNumber = "123ABCD",
            CustomerName = "Raphael",
            CustomerId = "123Raphael",
            CustomerDateOfBirth = DateTime.Parse("2023-09-20T19:17:53.471Z"),
            PolicyStartDate = DateTime.Parse("2023-09-10T19:17:53.471Z"),
            PolicyEndDate = DateTime.Parse("2023-09-20T19:17:53.471Z"),
            Coverages = new List<string> { "string" },
            MaxCoverageAmount = 20,
            PlanName = "Gold",
            CustomerCity = "Quito",
            CustomerAddress = "Rpe",
            VehiclePlate = "ABC124",
            VehicleModel = "Sedan",
            HasVehicleInspection = true
        }
    };

        mockPoliciesService.Setup(service => service.GetPolicyByPlateOrNumber(plateOrNumber))
            .Returns(policies);

        var controller = new PoliciesController(mockPoliciesService.Object);

        // Act
        var result = controller.GetPolicyByPlateOrNumber(plateOrNumber);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }


}
