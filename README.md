[![Build status](https://ci.appveyor.com/api/projects/status/jwu4cocyv2q7vfda/branch/main?svg=true)](https://ci.appveyor.com/project/pierregillon/fluentasync/branch/main)

# FluentAsync
This modest library provides **extension methods** on *Task* and *IAsyncEnumerable*, to create fluent calls, 
without the need to declare intermediate variables or use parenthesis.

The syntax is similar to *Linq* or *the builder pattern* : it is all about **fluently chaining method calls**.

## Syntactic sugar
As developer, we write a lot of code. Our aim is to produce compact and clean code to allow the best readability.
Theses extension methods are only ***syntactic sugar*** and do not provide any different behaviour or better performance 
of the underneath concepts.

## Nuget package

You can install [the package from NuGet](https://www.nuget.org/packages/FluentAsync).

```shell
dotnet add package FluentAsync
```

## Examples

Chaining async linq extension methods :

```csharp
    Task<IEnumerable<int>> asyncNumbers = Task.FromResult(Enumerable.Range(0, 100));

    string result = await asyncNumbers
        .ChainWith()
        .WhereAsync(x => x % 20 == 0)
        .OrderByDescendingAsync(x => x)
        .SelectAsync(x => $"Element is {x}")
        .AggregateAsync((x, y) => x + ", " + y);

    result
        .Should()
        .Be("Element is 80, Element is 60, Element is 40, Element is 20, Element is 0");
```

Producing IAsyncEnumerable from standard collection and asynchronously (but sequencially) enumerate the results :
```csharp

    var results = await _websites
        .SelectAsync(DownloadPage)
        .EnumerateAsync();

    results
        .Should()
        .BeEquivalentTo(
            "fake page content of https://eatorganic.com",
            "fake page content of https://savetheplanet.com",
            "fake page content of https://doyourpart.net"
        );
```
## Concrete example
The following example is to compare the code with and without FluentAsync, in a real case.
Let's say we want to parse a log file to analyse which errors occured during program execution.
```
    ...
    [INFO] The command d6q66qsdf has been digested.
    [INFO] The command d9qqsdfqsf44 has been digested.
 >  [ERROR] Failed to process the command : unable to find a correct handler
    [INFO] The command 54f5qs4df has been digested.
    [INFO] The command aa8f8hn5sdfg has been digested.
    ...
```

### Without FluentAsync
We need to introduce variables to allow our to code to be readable.
The downside is to break the method chain flow.

```csharp
    [Fact]
    public async Task Select_distinct_errors_in_a_log_file_without_async_extension()
    {
        IEnumerable<string> lines = await ReadAllLinesOfLogFileAsync();

        IEnumerable<string> filteredLines = lines
            .Select(x => new {
                Header = Regex.Match(x, HEADER_PATTERN).Value,
                Description = Regex.Replace(x, HEADER_PATTERN, string.Empty).Trim()
            })
            .Where(x => x.Header == "[ERROR]")
            .Where(x => !x.Description.ToLower().Contains("unhandled"))
            .Select(x => x.Description);

        string[] errorLines = RemoveDuplicatedLines(filteredLines).ToArray();

        errorLines
            .Should()
            .BeEquivalentTo(
                "Failed to process the command : unable to find a correct handler",
                "Unable to process the command : invalid cast exception."
            );
    }
```

### With FluentAsync
We can easily chain the async methods :

```csharp
    [Fact]
    public async Task Select_distinct_errors_in_a_log_file()
    {
        IReadOnlyCollection<string> lines = await ReadAllLinesOfLogFileAsync()
            .ChainWith()
            .SelectAsync(x => new {
                Header = Regex.Match(x, HEADER_PATTERN).Value,
                Description = Regex.Replace(x, HEADER_PATTERN, string.Empty).Trim()
            })
            .WhereAsync(x => x.Header == "[ERROR]")
            .WhereAsync(x => !x.Description.ToLower().Contains("unhandled"))
            .SelectAsync(x => x.Description)
            .PipeAsync(RemoveDuplicatedLines)
            .OrderByAsync()
            .EnumerateAsync();

        lines
            .Should()
            .BeEquivalentTo(
                "Failed to process the command : unable to find a correct handler",
                "Unable to process the command : invalid cast exception."
            );
    }
```

Have a look on the [source code](/FluentAsync.Tests/Examples/AsynchronouslyReadFileAndChainActions.cs).

## Under the hood

### Non covariant Task
Extending ```Task<T>``` and ```Task<IEnumerable<T>>``` can be very tricky because it is not an interface type 
and so do no support **[covariance](https://docs.microsoft.com/en-us/dotnet/standard/generics/covariance-and-contravariance)**.

It implies that all types which are derived from ```IEnumerable<T>```, and provided in ```Task```, for example, ```Task<List<T>>```
do not have the extension methods declared on ```Task<IEnumerable<T>>```.

```csharp
Task<List<int>> task = Task.FromResult(new List<int>{ 1, -2, 3 });

var result = await task.WhereAsync( x => x > 0); // NOT FOUND
```

### Solution: a covariant ITask\<T\>
If you look closely to the ```.ChainWith()``` method, you'll find out it returns a ```ITask\<T\>```.
```csharp
   Task<List<T>> task = Task.FromResult(new List<int>{ 1, -2, 3 });
   ITask<List<T>> task = task.ChainWith();
```
The main idea here is to wrap a Task to an equivalent that support covariant conversion :
```interface ITask<out T>```.
We can now define extension methods on ```ITask<IEnumerable<T>>``` and they will be available for all subtypes.

```csharp
ITask<List<int>> task = Task.FromResult(new List<int>{ 1, -2, 3 }).ChainWith();

var result = await task.WhereAsync( x => x > 0); // FOUND
```
```ITask<T>``` declares also the ```.GetAwaiter()``` method to be used with the key word ```await```.

# License
This repository is under [the MIT license](/LICENSE.md).