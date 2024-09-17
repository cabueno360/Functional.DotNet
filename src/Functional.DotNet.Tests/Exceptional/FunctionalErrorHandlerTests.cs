using FluentAssertions;
using Functional.DotNet.Extensions;
using System;
using FluentAssertions;
using Xunit;
using static Functional.DotNet.Exceptional;
using System.Text.RegularExpressions;

namespace Functional.DotNet.Tests.Exceptional
{
    public class FunctionalErrorHandlerTests
    {
     
        private int ThrowArgumentException(int input) => throw new ArgumentException("Invalid argument");

      

        public int ArgumentHandle(ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }

        [Fact]
        public void MapWithTryCatch_ShouldReturnError_WhenNullReferenceExceptionIsThrown()
        {
            // Arrange
            int input = 5;

            Func<int, int> MutiplyByFive = (value) => value * 5;


            Func<Exception, int> ErrorHandler = (ex) => ex switch
            {
                ArgumentException argEx => ArgumentHandle(argEx),
                _ => 0,
            };



            var result = input
                            .Tap(x => Console.WriteLine("Starting Try Catch"))
                            .MapWithTryCatch(ThrowArgumentException)
                            .Tap(x => Console.WriteLine("Try Catch finished with success"))
                            .OnError(ErrorHandler)
                            .OnSuccess(MutiplyByFive);

            result.Should().Be(0);

        }

        [Fact]
        public void OnError_ShouldInvokeErrorHandler_WhenExceptionOccurs()
        {
            // Arrange
            int input = 5;
            Exception capturedException = null;

            // Act
            var result = input.MapWithTryCatch(ThrowArgumentException)
                              .OnError(ex => capturedException = ex);

            // Assert
            capturedException.Should().BeOfType<ArgumentException>();
            capturedException.Message.Should().Be("Invalid argument");
        }

        public static void HandleException(Exception ex)
        {
            switch (ex)
            {
                case ArgumentException argEx:
                    Console.WriteLine($"Argument Exception: {argEx.Message}");
                    break;
                case NullReferenceException nullEx:
                    Console.WriteLine($"Null Reference Exception: {nullEx.Message}");
                    break;
                case InvalidOperationException invalidOpEx:
                    Console.WriteLine($"Invalid Operation: {invalidOpEx.Message}");
                    break;
                default:
                    Console.WriteLine($"Unknown Exception: {ex.Message}");
                    break;
            }
        }

      
    }
}
