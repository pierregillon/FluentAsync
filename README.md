[![Build status](https://ci.appveyor.com/api/projects/status/jwu4cocyv2q7vfda/branch/main?svg=true)](https://ci.appveyor.com/project/pierregillon/fluentasync/branch/main)

# FluentAsync
This modest library provides **extension methods** on *Task* and *IAsyncEnumerable*, to create fluent calls, 
without the need to declare intermediate variables or use parenthesis.

The syntax is similar to *Linq* or *the builder pattern* : it is all about **fluently chaining method calls**.

## Syntactic sugar
As developer, we write a lot of code. Our aim is to produce compact and clean code to allow the best readability.
Theses extension methods are only ***syntactic sugar*** and do not provide any different behaviour or better performance 
of the underneath concepts.

## Examples

Chaining async linq extension methods :

```csharp
    Task<IEnumerable<int>> asyncNumbers = Task.FromResult(Enumerable.Range(0, 100));

    string result = await asyncNumbers
        .WhereAsync(x => x % 20 == 0)
        .OrderByDescendingAsync(x => x)
        .SelectAsync(x => $"Element is {x}")
        .AggregateAsync((x, y) => x + ", " + y);

    result
        .Should()
        .Be("Element is 80, Element is 60, Element is 40, Element is 20, Element is 0");
```

Producing IAsyncEnumerable and asynchronously (but sequencially) enumerate the results :
```csharp

    var results = await _websites
        .SelectAsync(DownloadPage)
        .EnumerateAll();

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

## Limitations
The extension methods are declared on ***Task\<IEnumerable\<T>>*** which is not an interface type 
and so do no support **[covariance](https://docs.microsoft.com/en-us/dotnet/standard/generics/covariance-and-contravariance)**.

It implies that all types which are derived from ***IEnumerable\<T>***, and provided in Task, for example, ***Task<List\<T>>***
do not have the extension methods.

```csharp
Task<List<int>> task = Task.FromResult(new List<int>{ 1, -2, 3 });

var result = await task.WhereAsync( x => x > 0); // NOT FOUND
```

As a workaround, as long as .NET do not provide ***ITask\<out T>*** (so supporting covariance), your can do :

```csharp
Task<List<int>> task = Task.FromResult(new List<int>{ 1, -2, 3 });

var result = await task
    .AsEnumerable()
    .WhereAsync( x => x > 0); // FOUND
```

The current types that have the AsEnumerable() are :
* List\<T>
* T[]
* IReadOnlyCollection\<T>
* IReadOnlyList\<T>
* HashSet\<T>
* ICollection\<T>
* Collection\<T>
* IGrouping\<TKey, T>

If you are using a derived type from ***IEnumerable\<T>*** which is not present from ubove, you can still do :

```csharp
Task<List<int>> task = Task.FromResult(new List<int>{ 1, -2, 3 });

var result = await task
    .PipeAsync(x => (IEnumerable<int>)x)
    .WhereAsync( x => x > 0); // FOUND
```


