using BudgetCalculator;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BudgetCalculator.Tests;

public class BudgetControllerTests
{
    [Fact]
    public void Calculate_WithoutInsurance_Returns500()
    {
        var controller = new BudgetController();
        var request = new BudgetRequest
        {
            DailyCost = 100m,
            Days = 5,
            IncludeInsurance = false
        };

        var result = controller.Calculate(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
        Assert.Equivalent(new { TotalAmount = 500m }, ok.Value);
    }

    [Fact]
    public void Calculate_WithInsurance_Returns525()
    {
        var controller = new BudgetController();
        var request = new BudgetRequest
        {
            DailyCost = 100m,
            Days = 5,
            IncludeInsurance = true
        };

        var result = controller.Calculate(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
        Assert.Equivalent(new { TotalAmount = 525m }, ok.Value);
    }

    [Fact]
    public void Calculate_NegativeDays_ReturnsBadRequest()
    {
        var controller = new BudgetController();
        var request = new BudgetRequest
        {
            DailyCost = 100m,
            Days = -1,
            IncludeInsurance = false
        };

        var result = controller.Calculate(request);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
        Assert.Equal("Количество дней должно быть больше нуля.", badRequest.Value);
    }

    [Fact]
    public void Calculate_ZeroDays_ReturnsBadRequest()
    {
        var controller = new BudgetController();
        var request = new BudgetRequest
        {
            DailyCost = 100m,
            Days = 0,
            IncludeInsurance = false
        };

        var result = controller.Calculate(request);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
        Assert.Equal("Количество дней должно быть больше нуля.", badRequest.Value);
    }

    [Fact]
    public void Calculate_ZeroDailyCost_Returns0()
    {
        var controller = new BudgetController();
        var request = new BudgetRequest
        {
            DailyCost = 0m,
            Days = 5,
            IncludeInsurance = false
        };

        var result = controller.Calculate(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
        Assert.Equivalent(new { TotalAmount = 0m }, ok.Value);
    }

    [Fact]
    public void Calculate_DecimalDailyCost_Returns301_50()
    {
        var controller = new BudgetController();
        var request = new BudgetRequest
        {
            DailyCost = 100.50m,
            Days = 3,
            IncludeInsurance = false
        };

        var result = controller.Calculate(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
        Assert.Equivalent(new { TotalAmount = 301.50m }, ok.Value);
    }
}
