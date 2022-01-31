using System;
using ThaudalAPI.Controllers;
using Xunit;

namespace DayOfWeekTest;

public class DayOfWeekTest
{
    [Fact]
    public void Should_Return_True_If_It_Is_Friday()
    {
        DayOfWeekController controller = new DayOfWeekController();
        var inputDate = new DateTime(2022, 2, 4);
        var result = controller.IsItFridayInternal(inputDate);
        Assert.True(result);
    }
    [Fact]
    public void Should_Return_False_If_It_Is_Not_Friday()
    {
        DayOfWeekController controller = new DayOfWeekController();
        var inputDate = new DateTime(2022, 2, 3);
        var result = controller.IsItFridayInternal(inputDate);
        Assert.False(result);
    }
}