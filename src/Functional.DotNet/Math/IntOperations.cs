using static Functional.DotNet.F;
using static Functional.DotNet.MathFunctions;
namespace Functional.DotNet
{
    public static class IntOperations
    {

        public static int Add(this int valueOriginal, Option<int> valueToAdd)
        {
            var valueToBeAdded = valueToAdd.GetOrElse(0);
            Option<int> intOpt = valueOriginal;

            return
                intOpt.Match(
                    None: () => 0,
                    Some: (value) => value + valueToBeAdded);
        }

        public static int Subtract(this int valueOriginal, Option<int> valueToSubtract)
        {
            var valueToBeSubtracted = valueToSubtract.GetOrElse(0);
            Option<int> intOpt = valueOriginal;

            return
                intOpt.Match(
                    None: () => 0,
                    Some: (value) => value - valueToBeSubtracted); // Fixed the subtraction logic
        }


        public static int Multiply(this int valueOriginal, Option<int> valueToMultiply)
        {
            var valueToBeMultiplied = valueToMultiply.GetOrElse(1);
            Option<int> intOpt = valueOriginal;

            return
                intOpt.Match(
                    None: () => 0,
                    Some: (value) => value * valueToBeMultiplied);
        }


        public static double Pow(this int valueOriginal, Option<int> exponent)
        {
            var exp = exponent.GetOrElse(1);
            Option<int> intOpt = valueOriginal;
            
            return
                intOpt.Match(
                    None: () => 0,
                    Some: (value) => Math.Pow(value, exp));
        }

        public static Option<int> Divide(this int valueOriginal, Option<int> divisor)
        {
            if (!divisor.IsSome()) return None;


            var div = divisor.GetOrElse(1);
            Option<int> intOpt = valueOriginal;

            return
                intOpt.Match(
                    None: () => None,
                    Some: (value) =>
                    {
                        if (div == 0)
                            return None; // Handle division by zero
                        return Some(value / div);
                    });
        }

        public static Option<int> Modulo(this int valueOriginal, Option<int> divisor)
        {
            if (!divisor.IsSome()) return None;

            var div = divisor.GetOrElse(1);
            Option<int> intOpt = valueOriginal;

            return
                intOpt.Match(
                    None: () => None,
                    Some: (value) =>
                    {
                        if (div == 0)
                            return None; // Handle modulo by zero
                        return Some(value % div);
                    });
        }

        public static Option<double> SquareRoot(this int valueOriginal)
        {
            Option<int> intOpt = valueOriginal;

            return
                intOpt.Match(
                    None: () => None,
                    Some: (value) =>
                    {
                        if (value < 0)
                            return None; // Handle negative values
                        return Some(Math.Sqrt(value));
                    });
        }

        public static int Absolute(this int valueOriginal)
        {
            Option<int> intOpt = valueOriginal;

            return
                intOpt.Match(
                    None: () => 0,
                    Some: (value) => Math.Abs(value));
        }

        public static Option<double> Logarithm(this int valueOriginal, Option<double> baseValue)
        {
            var baseVal = baseValue.GetOrElse(Math.E); // Default to natural log
            Option<int> intOpt = valueOriginal;

            return
                intOpt.Match(
                    None: () => None,
                    Some: (value) =>
                    {
                        if (value <= 0 || baseVal <= 1)
                            return None; // Handle invalid values
                        return Some(Math.Log(value) / Math.Log(baseVal));
                    });
        }
    }
}
