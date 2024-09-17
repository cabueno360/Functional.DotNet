
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using static Functional.DotNet.F;
namespace Functional.DotNet.Monad
{
 
    public readonly record struct Result<T>
    {
        
        public bool IsSuccess { get; init; }

 
        public string Message { get; init; }

 
        public int Status { get; init; }

 
        public T? Value { get; init; } = default!;

        public bool HasValue { get; init; } = default!;

        // Computed property; no need to serialize
        public bool IsFailure => !IsSuccess;

        // Replace Option<Exception> with a serializable type
        
        public Exception? Exception { get; init; } = null;

        // Constructor
        public Result(bool isSuccess, string message, int status, T? value, Exception? exception = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Status = status;
            Value = value;
            Exception = exception;
        }

        /// <summary>
        /// Creates an instance of Result monad containing value.
        /// </summary>
        /// <param name="value">Value to store inside the instance. If it is null then Exception property is set to ArgumentNullException.</param>
        public Result(T? value)
        {
            if (value is null)
            {
                Exception = new ArgumentNullException(nameof(value));
                HasValue = false;
            }
            else
            {
                Exception = null;
                Value = value;
                HasValue = true;
            }
        }

        /// <summary>
        /// Creates an instance of Result monad containing Exception.
        /// </summary>
        /// <param name="ex">Exception to store inside the instance.</param>
        public Result(Exception ex) => (Exception, HasValue) = (ex, false);

        /// <summary>
        /// Deconstructs this Result into a tuple.
        /// </summary>
        /// <param name="isValue">Returns true if Value is present, false if Exception is present.</param>
        /// <param name="value">Returns the value stored inside the monad. The value is default(T) if there is an Exception present.</param>
        /// <param name="exception">Gets the Exception. Returns null if there is a Value present.</param>
        public void Deconstruct(out bool isValue, out T? value, out Exception? exception) =>
            (isValue, value, exception) = (HasValue, Value, Exception);

        /// <summary>
        /// Executes one of the given funcs based on whether a value is present and returns its result.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="whenValue">Func to execute on Value, if present.</param>
        /// <param name="whenEx">Func to execute on Exception, if present.</param>
        /// <returns>Result of the func that gets executed.</returns>
        public TResult? Match<TResult>(Func<T, TResult?> OnSuccess, Func<Exception, TResult?> OnFailure)
            => HasValue ? OnSuccess(Value) : OnFailure(Exception);

        /// <summary>
        /// Executes one of the given asynchronous funcs based on whether a value is present and returns the task which will eventually yield a result.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="whenValue">Asynchronous func to execute on Value, if present.</param>
        /// <param name="whenEx">Asynchronous func to execute on Exception, if present.</param>
        /// <returns>Task which will yield the result on completion.</returns>
        public async Task<TResult> Match<TResult>(Func<T, Task<TResult>> OnSuccess, Func<Exception, Task<TResult>> OnFailure)
            => HasValue ? await OnSuccess(Value) : await OnFailure(Exception);

        /// <summary>
        /// Projects the Value, if present, using the supplied func.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of projection operation.</typeparam>
        /// <param name="f">The func which operates on Value. It is executed inside a try/catch block.</param>
        /// <returns>
        /// The result of the projection operation as a Result. 
        /// If the object contains an exception then the result also contains the same exception.
        /// If the func is null then the result contains ArgumentNullException.
        /// If the func throws an exception then the result contains that exception.
        /// </returns>
        public Result<TResult> Map<TResult>(Func<T, TResult?> f) => HasValue ? Result.Try(f, Value) : new(Exception);

        /// <summary>
        /// Projects the Value, if present, using the supplied asynchronous func.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of projection operation.</typeparam>
        /// <param name="f">The func which operates on Value asynchronously.</param>
        /// <returns>
        /// Task which yields the result of the projection operation as a Result. 
        /// If the object contains an exception then the result also contains the same exception.
        /// If the func is null then the result contains ArgumentNullException.
        /// If the func throws an exception then the result contains that exception.
        /// If the task gets cancelled then the result contains OperationCancelledException.
        /// </returns>
        public async Task<Result<TResult>> Map<TResult>(Func<T, Task<TResult?>> f)
            => HasValue ? await Result.Try(f, Value) : new(Exception);

        ///// <summary>
        ///// Projects the Value, if present, using the supplied func which also returns a Result.
        ///// Bind flattens the result. Calling Map using the func will return a Result of a Result.
        ///// </summary>
        ///// <typeparam name="TResult">The type of the raw value of the projection operation.</typeparam>
        ///// <param name="f">The func which operates on Value.</param>
        ///// <returns>
        ///// The result of the projection operation as a Result.
        ///// If the object contains an exception then the result also contains the same exception.
        ///// If the func is null then the result contains ArgumentNullException.
        ///// If the func throws an exception then the result contains that exception.
        ///// </returns>
        //public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> f)
        //    => HasValue ? Result.Try(f, Value).Flatten() : new(Exception);

        /// <summary>
        /// Projects the Value, if present, using the supplied asynchronous func.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of projection operation.</typeparam>
        /// <param name="f">The func which operates on Value asynchronously.</param>
        /// <returns>
        /// Task which yields the result of the projection operation as a Result. 
        /// If the object contains an exception then the result also contains the same exception.
        /// If the func is null then the result contains ArgumentNullException.
        /// If the func throws an exception then the result contains that exception.
        /// If the task gets cancelled then the result contains OperationCancelledException.
        /// </returns>
        public async Task<Result<TResult>> Bind<TResult>(Func<T, Task<Result<TResult>>> f)
        {
            if (HasValue)
            {
                try
                {
                    return await f(Value);
                }
                catch (Exception ex)
                {
                    return new(ex);
                }
            }
            return new(Exception);
        }

        /// <summary>
        /// Defaults a Result to contain the specified value, if it contains an exception.
        /// </summary>
        /// <param name="val">Default value.</param>
        /// <returns>Same instance of the Result if it contains a value. Otherwise a new Result containing the specified default value.</returns>
        public Result<T> DefaultIfException(T val) => HasValue ? this : new(val);

        /// <summary>
        /// Defaults a Result to contain a value computed lazily at runtime, if it contains an exception.
        /// </summary>
        /// <param name="f">func which provides the default value. It is executed only if Value is not present.</param>
        /// <returns>
        /// Same instance of the Result if it contains Value. Otherwise a new Result containing the result of func evaluation.
        /// If the func is null then the result contains ArgumentNullException.
        /// If the func throws an exception then the object contains that exception.
        /// </returns>
        public Result<T> DefaultIfException(Func<T?> f) => HasValue ? this : Result.Try(f);

        /// <summary>
        /// Defaults a Result to contain a value computed lazily and asynchronously at runtime, if it contains an exception.
        /// </summary>
        /// <param name="f">func which provides the default value asynchronously. It is executed only if Value is not present.</param>
        /// <returns>
        /// Task which yields the same instance of the Result if it contains Value. Otherwise value provided by the func as Result. 
        /// If the func throws an exception then the result contains that exception.
        /// If the func is null then the result contains ArgumentNullException.
        /// If the task gets cancelled then the result contains OperationCancelledException.
        /// </returns>
        public async Task<Result<T>> DefaultIfException(Func<Task<T?>> f) => HasValue ? this : await Result.Try(f);
    }


    /// <summary>
    /// Static class providing utility methods to create an instance of Result monad.
    /// </summary>
    public static class Result
    {
        /// <summary>
        /// Creates an instance of Result from an exception.
        /// </summary>
        /// <typeparam name="T">Type of T</typeparam>
        /// <param name="ex">Exception object.</param>
        /// <returns>A Result object which contains the given exception.</returns>
        public static Result<T> Exception<T>(Exception ex) => new(ex);

        /// <summary>
        /// Creates an instance of Result from a value.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="value">Value object.</param>
        /// <returns>A Result object which contains the given value.</returns>
        public static Result<T> Value<T>(T? value) => new(value);

        /// <summary>
        /// Amplifies the result of a function call to a Result. The function is executed inside a try/catch block.
        /// </summary>
        /// <typeparam name="T">Return type of the func.</typeparam>
        /// <param name="f">Function to execute. </param>
        /// <returns>
        /// A Result object which contains the func's result or exception thrown by it.
        /// If the func is null then it contains ArgumentNullException.
        /// </returns>
        public static Result<T> Try<T>(this Func<T?> f)
        {
            try
            {
                return new(f());
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }



        /// <summary>
        /// Amplifies the result of an asynchronous function call to a Result. 
        /// The function is executed and resulting task awaited inside a try/catch block.
        /// </summary>
        /// <typeparam name="T">Type of the result yielded by the asynchronous function.</typeparam>
        /// <param name="f">Asynchronous function to execute.</param>
        /// <returns>
        /// A task which yields a Result, which contains the func's result or exception thrown by it. 
        /// Awaiting the returned task is guaranteed to not fail even if the original task is faulted or cancelled.
        /// If the func is null then it contains ArgumentNullException.
        /// If the task gets cancelled then the result contains OperationCancelledException.
        /// </returns>
        public static async Task<Result<T>> Try<T>(Func<Task<T?>> f)
        {
            try
            {
                return new(await f());
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        /// <summary>
        /// Amplifies the result of a function call on given value to a Result. The function is executed inside a try/catch block.
        /// </summary>
        /// <typeparam name="T1">Type of input value to the func.</typeparam>
        /// <typeparam name="T2">Type of value returned by the function.</typeparam>
        /// <param name="f">Function to execute.</param>
        /// <param name="val">Input parameter to the func.</param>
        /// <returns>
        /// A Result containing the result on successful execution or exception thrown by it.
        /// If the func is null then it contains NullReferenceException.
        /// </returns>
        public static Result<T2> Try<T1, T2>(Func<T1, T2?> f, T1 val) => Try(() => f(val));

        /// <summary>
        /// Amplifies the result of an asynchronous function call on given value to a Result. 
        /// The function is executed and resulting task awaited inside a try/catch block.
        /// </summary>
        /// <typeparam name="T1">Type of input value to the asynhronous function.</typeparam>
        /// <typeparam name="T2">Type of the result yielded by the asynchronous function.</typeparam>
        /// <param name="f">Asynchronous function to execute on value.</param>
        /// <param name="val">Input parameter to the func.</param>
        /// <returns>
        /// A task which yields a Result containing the result on successful execution or exception thrown by it.
        /// If the func is null then it contains NullReferenceException.
        /// If the task gets cancelled then the result contains OperationCancelledException.
        /// </returns>
        public static async Task<Result<T2>> Try<T1, T2>(Func<T1, Task<T2?>> f, T1 val) => await Try(() => f(val));

        public static Result<T> Unauthorized<T>(string message = "") => new(false, message, 401, default);

        public static Result<T> BadRequest<T>(string message = "") => new(false, message, 400, default);

        public static Result<T> BadRequest<T>(string message, Exception exception) =>
            new(false, message, 400, default)
            {
                Exception = exception
            };

        public static Result<T> NotFound<T>(string message = "") => new(false, message, 404, default);

        public static Result<T> Error<T>(string message) => new(false, message, 500, default);

        public static Result<T> Error<T>(string message, Exception exception) =>
            new(false, message, 500, default)
            {
                Exception = exception
            };


        public static Result<T> Success<T>(T data) => new(true, string.Empty, 200, data);
    }

}
