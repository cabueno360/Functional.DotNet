# `TaskExt` Documentation

## Overview
The `TaskExt` class in Functional.DotNet provides extensions for working with `Task<T>` objects in a functional programming style. It includes methods for applying functions, mapping, and handling exceptions in an elegant way.

## Key Methods and Their Use

### `Apply<T, R>`
- **Purpose**: Applies a function (`Task<Func<T, R>>`) to a value (`Task<T>`).
- **Usage Example**:

    ```csharp
    Task<Func<int, string>> funcTask = Task.FromResult<int, string>(x => x.ToString());
    Task<int> valueTask = Task.FromResult(123);
    Task<string> resultTask = funcTask.Apply(valueTask);
    string result = await resultTask;  // "123"
    ```
  ***Query Expression***
    ```csharp
        Task<Func<int, string>> funcTask = Task.FromResult<int, string>(x => x.ToString());
        Task<int> valueTask = Task.FromResult(123);

        Task<string> resultTask = from func in funcTask
                                from value in valueTask
                                select func(value);

        string result = await resultTask;  // "123"
    ```

### `Map<T, R>`
- **Purpose**: Transforms the result of a `Task<T>` using a function (`Func<T, R>`).
- **Usage Example**:

    ```csharp
    Task<int> task = Task.FromResult(5);
    Task<string> result = task.Map(x => x.ToString());
    string resultValue = await result;  // "5"
    ```

   ***Query Expression***
    ```csharp
        Task<int> task = Task.FromResult(5);

        Task<string> result = from x in task
                            select x.ToString();

        string resultValue = await result;  // "5"
    ```

### `Map<T, R>` with Fault Handling
- **Purpose**: Maps the result of a `Task<T>` or handles exceptions using provided functions.
- **Usage Example**:

    ```csharp
    Task<int> task = Task.FromResult(5);
    Task<string> result = task.Map(
        Faulted: ex => "Error",
        Completed: x => x.ToString());
    string resultValue = await result;  // "5"
    ```


    ***Query Expression***

    ```csharp
        Task<int> task = Task.FromResult(5);

        Task<string> result = from x in task
                            let success = x.ToString()
                            catch (Exception ex)
                            let faulted = "Error"
                            select success;

        string resultValue = await result;  // "5"
    ```

### `Recover<T>`
- **Purpose**: Recovers from a faulted `Task<T>` by providing a fallback value.
- **Usage Example**:

    ```csharp
    Task<int> task = Task.FromException<int>(new Exception("Failure"));
    Task<int> recoveredTask = task.Recover(ex => 0);
    int resultValue = await recoveredTask;  // 0
    ```

    ***Query Expression***

    ```csharp
        Task<int> task = Task.FromException<int>(new Exception("Failure"));

        Task<int> recoveredTask = from x in task.Recover(ex => 0)
                                select x;

        int resultValue = await recoveredTask;  // 0
    ```    

### `SelectMany<T, R, RR>`
- **Purpose**: Chains asynchronous operations and projects the result.
- **Usage Example**:

    ```csharp
    Task<int> task = Task.FromResult(5);
    Task<string> result = task.SelectMany(
        x => Task.FromResult(x.ToString()),
        (x, y) => $"{x}:{y}");
    string resultValue = await result;  // "5:5"
    ```

    ***Query Expression***

    ```csharp
        Task<int> task = Task.FromResult(5);

        Task<string> result = from x in task
                              from y in Task.FromResult(x.ToString())
                              select $"{x}:{y}";

        string resultValue = await result;  // "5:5"
    ```

### `Join<T, U, K, V>`
- **Purpose**: Performs a join operation between two tasks.
- **Usage Example**:

    ```csharp
    Task<int> task1 = Task.FromResult(1);
    Task<int> task2 = Task.FromResult(1);
    Task<string> result = task1.Join(
        task2,
        x => x,
        y => y,
        (x, y) => $"{x}-{y}");
    string resultValue = await result;  // "1-1"
    ```

    ***Query Expression***

    ```csharp
        Task<int> task1 = Task.FromResult(1);
        Task<int> task2 = Task.FromResult(1);

        Task<string> result = from x in task1
                            from y in task2
                            select $"{x}-{y}";

        string resultValue = await result;  // "1-1"
    ```

### `GroupJoin<T, U, K, V>`
- **Purpose**: Performs a group join operation between two tasks.
- **Usage Example**:

    ```csharp
    Task<int> task1 = Task.FromResult(1);
    Task<int[]> task2 = Task.FromResult(new[] { 1, 2, 3 });
    Task<IEnumerable<int>> result = task1.GroupJoin(
        task2,
        x => x,
        y => y,
        (x, group) => group.Where(g => g == x));
    IEnumerable<int> resultValue = await result;  // [1]
    ```

    ***Query Expression***

    ```csharp
        Task<int> task1 = Task.FromResult(1);
        Task<int[]> task2 = Task.FromResult(new[] { 1, 2, 3 });

        Task<IEnumerable<int>> result = from x in task1
                                        from group in task2
                                        select group.Where(g => g == x);

        IEnumerable<int> resultValue = await result;  // [1]       
    ```

# Overview: Advantages of Using `TaskExt` Methods

In modern asynchronous programming with C#, the use of `Task` objects is ubiquitous. However, handling asynchronous operations can become cumbersome and error-prone without the right tools. The `TaskExt` methods provided in the `Functional.DotNet` library offer several advantages that improve code readability, maintainability, and functionality compared to using standard `Task` methods alone.

## Enhanced Functional Composition

- **Functional Style**: `TaskExt` methods embrace a functional programming style, enabling cleaner and more expressive code. Methods like `Map`, `Apply`, and `SelectMany` facilitate the composition of asynchronous operations in a way that is more natural and aligned with functional programming principles.
  
- **Chaining Operations**: By providing methods to chain operations seamlessly, `TaskExt` reduces boilerplate code and avoids the need for nested `await` calls. This makes the code more readable and easier to reason about.

## Error Handling and Fault Tolerance

- **Graceful Error Handling**: With methods like `Recover` and `RecoverWith`, `TaskExt` simplifies error handling in asynchronous workflows. These methods allow you to provide fallback values or alternative tasks in case of exceptions, making your code more robust and resilient.

- **Separation of Concerns**: `TaskExt` helps separate error handling logic from the main business logic, improving the clarity and maintainability of the code.

## Improved Efficiency

- **Optimized Await**: `TaskExt` methods use `ConfigureAwait(false)` to avoid capturing the synchronization context, which is especially beneficial in library code and non-UI scenarios. This can lead to performance improvements by reducing unnecessary context switches.

## Enhanced LINQ Integration

- **LINQ Expressions**: `TaskExt` integrates well with LINQ, allowing you to perform complex queries and transformations in a concise and declarative manner. This aligns with the LINQ-to-Objects paradigm, making it easier to work with asynchronous data streams.

## Cleaner Asynchronous Patterns

- **Readability**: Methods like `Map`, `SelectMany`, and `GroupJoin` provide a more readable and expressive way to handle asynchronous transformations and joins. This reduces the cognitive load on developers and helps prevent common pitfalls associated with manual task handling.

- **Consistency**: By using `TaskExt`, you ensure consistency in how asynchronous tasks are handled throughout your codebase. This uniform approach simplifies maintenance and reduces the likelihood of bugs.

