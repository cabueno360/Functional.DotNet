using Orleans;
using Orleans.CodeGeneration;
using System;
using System.Linq;
using static Functional.DotNet.F;
namespace Functional.DotNet.Monad
{
    /// <summary>
    /// This class is used on the client 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Success"></param>
    /// <param name="Message"></param>
    /// <param name="Status"></param>
    /// <param name="Data"></param>
    [GenerateSerializer]
    public sealed record Result<T>
    {
        [Id(0)]
        public bool IsSuccess { get; init; }

        [Id(1)]
        public string Message { get; init; }

        [Id(2)]
        public int Status { get; init; }

        [Id(3)]
        public T? Data { get; init; }

        // Computed property; no need to serialize
        public bool IsFailure => !IsSuccess;

        // Replace Option<Exception> with a serializable type
        [Id(4)]
        public Exception? Exception { get; init; } = null;

        // Constructor
        public Result(bool isSuccess, string message, int status, T? data, Exception? exception = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Status = status;
            Data = data;
            Exception = exception;
        }
    }

    public sealed class Result
    {
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
