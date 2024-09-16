namespace Functional.DotNet.Extensions.NoElevate;

public static class CombinatorsExt
{
    /// <summary>
    /// Projects the source value into a new form by applying a specified function.
    /// </summary>
    /// <param name="source">The source value.</param>
    /// <param name="func">A transform function to apply to the source value.</param>
    /// <typeparam name="TSource">The type of the source value.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by <paramref name="func"/>.</typeparam>
    /// <returns>The result of invoking <paramref name="func"/> on <paramref name="source"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="func"/> is <c>null</c>.</exception>
    /// <example>
    /// The following example demonstrates how to use the <see cref="Map{TSource, TResult}"/> method:
    /// <code>
    /// int number = 5;
    /// string result = number.Map(n => $"Number is {n}");
    /// // result: "Number is 5"
    /// </code>
    /// </example>
    public static TResult Map<TSource, TResult>(this TSource source, Func<TSource, TResult> func) =>
        func(source);
    
    /// <summary>
    /// Splits the source value into multiple parts, processes each part with the provided functions, and then joins the results.
    /// </summary>
    /// <param name="this">The source value.</param>
    /// <param name="joinFunc">A function to join the processed parts.</param>
    /// <param name="partFuncs">An array of functions to process each part of the source value.</param>
    /// <typeparam name="TSource">The type of the source value.</typeparam>
    /// <typeparam name="TMiddle">The type of the intermediate results.</typeparam>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <returns>The result of applying <paramref name="joinFunc"/> to the collection of processed parts.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="joinFunc"/> or <paramref name="partFuncs"/> is <c>null</c>.
    /// </exception>
    /// <example>
    /// The following example demonstrates how to use the <see cref="Fork{TSource, TMiddle, TResult}"/> method:
    /// <code>
    /// string text = "Hello";
    /// var result = text.Fork(
    ///     parts => string.Join(", ", parts),
    ///     s => s.Length,
    ///     s => s.Count(char.IsUpper));
    /// // result: "5, 1"
    /// </code>
    /// </example>
    public static TResult Fork<TSource, TMiddle, TResult>(this TSource @this, Func<IEnumerable<TMiddle>, TResult> joinFunc,
        params Func<TSource, TMiddle>[] partFuncs) => partFuncs.Select(pf => pf(@this)).Map(joinFunc);
    
    
    /// <summary>
    /// Tries a sequence of functions on the source value and returns the first non-null result.
    /// </summary>
    /// <param name="this">The source value.</param>
    /// <param name="functions">An array of functions to try on the source value.</param>
    /// <typeparam name="TSource">The type of the source value.</typeparam>
    /// <typeparam name="TResult">The type of the result value.</typeparam>
    /// <returns>
    /// The first non-null result from applying the functions to the source value;
    /// or <c>null</c> if all results are <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="functions"/> is <c>null</c>.</exception>
    /// <example>
    /// The following example demonstrates how to use the <see cref="Alt{TSource, TResult}"/> method:
    /// <code>
    /// string input = "test@example.com";
    /// var result = input.Alt(
    ///     s => null,
    ///     s => s.Contains("@") ? s : null,
    ///     s => "Default");
    /// // result: "test@example.com"
    /// </code>
    /// </example>
    public static TResult Alt<TSource, TResult>(
        this TSource @this, 
        params Func<TSource, TResult>[] args) => 
        args
            .Select(x=>x(@this))
            .First(x=>x != null);
    

}