using System;
using Functional.DotNet;
using System.Reactive;

namespace Functional.DotNet
{
    public static partial class F
    {
        public static Exceptional<T> Exceptional<T>(T t) => new(t);
    }

    public struct Exceptional<T>
    {
        public Exception? Ex { get; }
        public T? Value { get; }

        public bool IsSuccess { get; }
        public bool IsException => !IsSuccess;

        internal Exceptional(Exception ex)
        {
            IsSuccess = false;
            Ex = ex ?? throw new ArgumentNullException(nameof(ex));
            Value = default;
        }

        internal Exceptional(T value)
        {
            IsSuccess = true;
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Ex = default;
        }

        public static implicit operator Exceptional<T>(Exception ex) => new(ex);
        public static implicit operator Exceptional<T>(T t) => new(t);

        public TR Match<TR>(Func<Exception, TR> OnError, Func<T, TR> OnSuccess)
           => this.IsException ? OnError(Ex!) : OnSuccess(Value!);

        public Unit Match(Action<Exception> OnError, Action<T> OnSuccess)
           => Match(OnError.ToFunc(), OnSuccess.ToFunc());

        public override string ToString()
           => Match(
              ex => $"Exception({ex.Message})",
              t => $"Success({t})");
    }

    public static class Exceptional
    {
        // creating a new Exceptional
        public static Exceptional<T> Success<T>(T? value) => 
            new(value);

        public static Exceptional<T> Error<T>(Exception ex) =>
            new(ex);

        public static Func<T, Exceptional<T>> Return<T>()
           => t => t;

        public static Exceptional<R> Of<R>(Exception left)
           => new Exceptional<R>(left);

        public static Exceptional<R> Of<R>(R right)
           => new Exceptional<R>(right);

        // applicative

        public static Exceptional<R> Apply<T, R>
           (this Exceptional<Func<T, R>> @this, Exceptional<T> arg)
           => @this.Match(
              OnError: ex => ex,
              OnSuccess: func => arg.Match(
                 OnError: ex => ex,
                 OnSuccess: t => F.Exceptional(func(t))));

        public static Exceptional<Func<T2, R>> Apply<T1, T2, R>
           (this Exceptional<Func<T1, T2, R>> @this, Exceptional<T1> arg)
           => Apply(@this.Map(F.Curry), arg);

        public static Exceptional<Func<T2, T3, R>> Apply<T1, T2, T3, R>
           (this Exceptional<Func<T1, T2, T3, R>> @this, Exceptional<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Exceptional<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>
           (this Exceptional<Func<T1, T2, T3, T4, R>> @this, Exceptional<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Exceptional<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>
           (this Exceptional<Func<T1, T2, T3, T4, T5, R>> @this, Exceptional<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Exceptional<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>
           (this Exceptional<Func<T1, T2, T3, T4, T5, T6, R>> @this, Exceptional<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Exceptional<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>
           (this Exceptional<Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Exceptional<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Exceptional<Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>
           (this Exceptional<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Exceptional<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        public static Exceptional<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
           (this Exceptional<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Exceptional<T1> arg)
           => Apply(@this.Map(F.CurryFirst), arg);

        // functor

        public static Exceptional<RR> Map<R, RR>
        (
           this Exceptional<R> @this,
           Func<R, RR> f
        )
        => @this.Match
        (
           OnError: ex => new Exceptional<RR>(ex),
           OnSuccess: r => f(r)
        );

        public static Exceptional<Unit> ForEach<R>(this Exceptional<R> @this, Action<R> act)
           => Map(@this, act.ToFunc());

        public static Exceptional<RR> Bind<R, RR>
        (
           this Exceptional<R> @this,
           Func<R, Exceptional<RR>> f
        )
        => @this.Match
        (
           OnError: ex => new Exceptional<RR>(ex),
           OnSuccess: r => f(r)
        );

        // LINQ

        public static Exceptional<R> Select<T, R>(this Exceptional<T> @this
           , Func<T, R> map) => @this.Map(map);

        public static Exceptional<RR> SelectMany<T, R, RR>
        (
           this Exceptional<T> @this,
           Func<T, Exceptional<R>> bind,
           Func<T, R, RR> project
        )
        => @this.Match
        (
           OnError: ex => new Exceptional<RR>(ex),
           OnSuccess: t => bind(t).Match
           (
              OnError: ex => new Exceptional<RR>(ex),
              OnSuccess: r => project(t, r)
           )
        );
    }
}
