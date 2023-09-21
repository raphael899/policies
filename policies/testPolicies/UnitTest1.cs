namespace testPolicies;

public class UnitTest1
{
    [Fact]
    public void CreatePolicy_ValidPolicy_ReturnsCreated()
    {
        var mockPoliciesService = new Mock<PoliciesService>();
        var controller = new PoliciesController(mockPoliciesService.Object);
        // change data please
        var policy = new Policy
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

        var result = controller.CreatePolicy(policy);

        Assert.IsType<CreatedAtRouteResult>(result);
        var createdAtRouteResult = (CreatedAtRouteResult)result;
        Assert.Equal("GetPolicy", createdAtRouteResult.RouteName);
        Assert.Equal(policy.Id, createdAtRouteResult.RouteValues["id"]);
        Assert.Equal(policy, createdAtRouteResult.Value);
    }

    [Fact]
    public void GetPolicyByPlateOrNumber_ValidPlateOrNumber_ReturnsOk()
    {
        var mockPoliciesService = new Mock<PoliciesService>();
        var controller = new PoliciesController(mockPoliciesService.Object);
        // change data please

        var plateOrNumber = "ABC123";
        mockPoliciesService.Setup(service => service.GetPolicyByPlateOrNumber(plateOrNumber))
            .Returns(new List<Policy>());

        var result = controller.GetPolicyByPlateOrNumber(plateOrNumber);

        Assert.NotNull(result);
        Assert.IsType<ActionResult<List<Policy>>>(result);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var policies = Assert.IsAssignableFrom<List<Policy>>(okResult.Value);
    }

}
