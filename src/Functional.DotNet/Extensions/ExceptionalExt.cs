using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using static Functional.DotNet.Exceptional;
using static Functional.DotNet.F;

namespace Functional.DotNet.Extensions
{
    public static class ExceptionalExt
    {
        public static Exceptional<TOut> MapWithTryCatch<TIn, TOut>(
            this TIn @this,
            Func<TIn, TOut> f)
        {
            try
            {
                var result = f(@this);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error<TOut>(ex);
            }
        }

        public static TIn  Tap<TIn>(
            this TIn @this,
            Action<TIn> action)
        {
            action(@this);
            return @this;
        }

        public static Either<Exceptional<TIn>, TOut> OnError<TIn, TOut>(
           this Exceptional<TIn> @this,
           Func<Exception, TOut> OnError)
        {

            if (@this.IsException)
                return OnError(@this.Ex);

            return @this;
        }

        public static TOut OnSuccess<TIn, TOut>(
            this Either<Exceptional<TIn>, TOut> @this,
            Func<TIn, TOut> onSuccess) =>
            @this.Match(
                Left: (exceptional) => onSuccess(exceptional.Value),
                Right: (errorRetur) => errorRetur);
            

        public static Option<TOut> OnSuccess<TIn, TOut>(
            this Exceptional<TIn> @this,
            Func<TIn, TOut> onSuccess)
        {

            if (@this.IsSuccess)
                return Some(onSuccess(@this.Value));

            return None;
        }
    }
}
