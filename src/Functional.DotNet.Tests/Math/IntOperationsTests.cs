using Functional.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Functional.DotNet.Extensions;
using static Functional.DotNet.F;

namespace Functional.DotNet.Tests.Math
{
    public class IntOperationsTests
    {
        [Fact]
        public void Add_ShouldReturnSum_WhenValueToAddIsSome()
        {
            int originalValue = 5;
            var valueToAdd = Some(3);
            int result = originalValue.Add(valueToAdd);
            Assert.Equal(8, result);
        }

        [Fact]
        public void Add_ShouldReturnOriginalValue_WhenValueToAddIsNone()
        {
            int originalValue = 5;
            var valueToAdd = None;
            int result = originalValue.Add(valueToAdd);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Subtract_ShouldReturnDifference_WhenValueToSubtractIsSome()
        {
            int originalValue = 5;
            var valueToSubtract = Some(3);
            int result = originalValue.Subtract(valueToSubtract);
            Assert.Equal(2, result);
        }

        [Fact]
        public void Subtract_ShouldReturnOriginalValue_WhenValueToSubtractIsNone()
        {
            int originalValue = 5;
            var valueToSubtract = None;
            int result = originalValue.Subtract(valueToSubtract);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Multiply_ShouldReturnProduct_WhenValueToMultiplyIsSome()
        {
            int originalValue = 5;
            var valueToMultiply = Some(3);
            int result = originalValue.Multiply(valueToMultiply);
            Assert.Equal(15, result);
        }

        [Fact]
        public void Multiply_ShouldReturnOriginalValue_WhenValueToMultiplyIsNone()
        {
            int originalValue = 5;
            var valueToMultiply = None;
            int result = originalValue.Multiply(valueToMultiply);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Pow_ShouldReturnPower_WhenExponentIsSome()
        {
            int originalValue = 2;
            var exponent = Some(3);
            double result = originalValue.Pow(exponent);
            Assert.Equal(8, result);
        }

        [Fact]
        public void Pow_ShouldReturnOriginalValue_WhenExponentIsNone()
        {
            int originalValue = 2;
            var exponent = None;
            double result = originalValue.Pow(exponent);
            Assert.Equal(2, result);
        }

        [Fact]
        public void Divide_ShouldReturnQuotient_WhenDivisorIsSome()
        {
            int originalValue = 6;
            var divisor = Some(3);
            var result = originalValue.Divide(divisor);
            Assert.Equal(Some(2), result);
        }

        [Fact]
        public void Divide_ShouldReturnNone_WhenDivisorIsZero()
        {
            int originalValue = 6;
            var divisor = Some(0);
            var result = originalValue.Divide(divisor);
            Assert.Equal(None, result);
        }

        [Fact]
        public void Divide_ShouldReturnNone_WhenDivisorIsNone()
        {
            int originalValue = 6;
            var divisor = None;
            var result = originalValue.Divide(divisor);
            Assert.Equal(None, result);
        }

        [Fact]
        public void Modulo_ShouldReturnRemainder_WhenDivisorIsSome()
        {
            int originalValue = 7;
            var divisor = Some(3);
            var result = originalValue.Modulo(divisor);
            Assert.Equal(Some(1), result);
        }

        [Fact]
        public void Modulo_ShouldReturnNone_WhenDivisorIsZero()
        {
            int originalValue = 7;
            var divisor = Some(0);
            var result = originalValue.Modulo(divisor);
            Assert.Equal(None, result);
        }

        [Fact]
        public void Modulo_ShouldReturnNone_WhenDivisorIsNone()
        {
            int originalValue = 7;
            var divisor = None;
            var result = originalValue.Modulo(divisor);
            Assert.Equal(None, result);
        }

        [Fact]
        public void SquareRoot_ShouldReturnSquareRoot_WhenValueIsNonNegative()
        {
            int originalValue = 4;
            var result = originalValue.SquareRoot();
            Assert.Equal(Some(2.0), result);
        }

        [Fact]
        public void SquareRoot_ShouldReturnNone_WhenValueIsNegative()
        {
            int originalValue = -4;
            var result = originalValue.SquareRoot();
            Assert.Equal(None, result);
        }

        [Fact]
        public void Absolute_ShouldReturnAbsoluteValue()
        {
            int originalValue = -5;
            int result = originalValue.Absolute();
            Assert.Equal(5, result);
        }

        [Fact]
        public void Logarithm_ShouldReturnLogarithm_WhenBaseAndValueAreValid()
        {
            int originalValue = 8;
            var baseValue = Some(2.0);
            var result = originalValue.Logarithm(baseValue);
            Assert.Equal(Some(3.0), result); 
        }

        [Fact]
        public void Logarithm_ShouldReturnNone_WhenValueIsNonPositive()
        {
            int originalValue = 0;
            var baseValue = Some(2.0);
            var result = originalValue.Logarithm(baseValue);
            Assert.Equal(None, result);
        }

        [Fact]
        public void Logarithm_ShouldReturnNone_WhenBaseIsInvalid()
        {
            int originalValue = 8;
            var baseValue = Some(1.0);
            var result = originalValue.Logarithm(baseValue);
            Assert.Equal(None, result);
        }
    }
}
