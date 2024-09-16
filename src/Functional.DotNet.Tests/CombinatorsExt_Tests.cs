using Functional.DotNet.Extensions;

namespace Functional.DotNet.Tests;
using System;
using Functional.DotNet.Extensions.NoElevate;

public class CombinatorsExt_Tests
{
    [Fact]
    public void Map_WithValidFunction_ReturnsExpectedResult()
    {
        // Arrange
        int source = 5;
        Func<int, string> func = x => $"Number is {x}";

        // Act
        string result = source.Map(func);

        // Assert
        Assert.Equal("Number is 5", result);
    }



    [Fact]
    public void Fork_WithValidFunctions_ReturnsJoinedResult()
    {
        // Arrange
        string source = "Hello";
        Func<IEnumerable<int>, string> joinFunc = nums => string.Join(", ", nums);
        Func<string, int> partFunc1 = s => s.Length;
        Func<string, int> partFunc2 = s => s.Count(char.IsUpper);

        // Act
        string result = source.Fork(joinFunc, partFunc1, partFunc2);

        // Assert
        // Length of "Hello" is 5, number of uppercase letters is 1
        Assert.Equal("5, 1", result);
    }


    [Fact]
    public void Alt_WithMultipleFunctions_ReturnsFirstNonNullResult()
    {
        // Arrange
        string source = "test@example.com";
        Func<string, string> func1 = s => null;
        Func<string, string> func2 = s => s.Contains("@") ? s : null;
        Func<string, string> func3 = s => "Fallback";

        // Act
        string result = source.Alt(func1, func2, func3);

        // Assert
        Assert.Equal("test@example.com", result);
    }



    [Fact]
    public void Compose_WithValidFunctions_ReturnsExpectedResult()
    {
        // Arrange
        Func<int, int> multiplyByTwo = x => x * 2;
        Func<int, string> toString = x => $"Result: {x}";

        // Act
        Func<int, string> composedFunc = multiplyByTwo.Compose(toString);
        string result = composedFunc(5);

        // Assert
        Assert.Equal("Result: 10", result);
    }

    [Fact]
    public void Compose_ChainedCompositions_ReturnsExpectedResult()
    {
        // Arrange
        Func<int, int> addThree = x => x + 3;
        Func<int, int> multiplyByTwo = x => x * 2;
        Func<int, string> toString = x => $"Final Result: {x}";

        // Act
        Func<int, string> composedFunc = addThree.Compose(multiplyByTwo).Compose(toString);
        string result = composedFunc(4);

        // Assert
        // ((4 + 3) * 2) = 14
        Assert.Equal("Final Result: 14", result);
    }

 
    [Fact]
    public void Compose_WithDifferentTypes_ReturnsExpectedResult()
    {
        // Arrange
        Func<string, int> parseInt = s => int.Parse(s);
        Func<int, double> sqrt = x => System.Math.Sqrt(x);

        // Act
        Func<string, double> composedFunc = parseInt.Compose(sqrt);
        double result = composedFunc("16");

        // Assert
        Assert.Equal(4.0, result, 4); // 4 decimal places precision
    }
}