using FluentAssertions;
using Xunit;

namespace RadarrSonarrMcp.Tests;

/// <summary>
/// Example test class demonstrating xUnit test structure.
/// This file can be deleted once you add real tests.
/// </summary>
public class ExampleTests
{
    [Fact]
    public void Example_Test_Should_Pass()
    {
        // Arrange
        var expected = "Hello, World!";
        
        // Act
        var actual = "Hello, World!";
        
        // Assert
        actual.Should().Be(expected);
    }
    
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(5, 5, 10)]
    [InlineData(-1, 1, 0)]
    public void Example_Theory_Should_Add_Numbers(int a, int b, int expected)
    {
        // Arrange & Act
        var result = a + b;
        
        // Assert
        result.Should().Be(expected);
    }
}