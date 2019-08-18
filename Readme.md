# GherkinSpec.Logging

## Overview

A bridge package that allows the subject-under-test in GivePenny GherkinSpec test projects to log to Microsoft's `Microsoft.Extensions.Logging.Abstractions.ILogger` and for those messages to be routed through to the test output/results.

See the [GivePenny GherkinSpec project](https://github.com/GivePenny/GherkinSpec) for background information.

## Using this package

Reference the [GivePenny.GherkinSpec.Logging](https://www.nuget.org/packages/GivePenny.GherkinSpec.Logging) package in your test project, then modify your project's `[BeforeRun]` hook to make sure that the following lines are all included:

```csharp
// Namespaces at the top ...
using GivePenny.GherkinSpec.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// ... then inside the [BeforeHook] method ...
var services = new ServiceCollection();
testRunContext.ServiceProvider = services
    .AddSingleton(testRunContext.Logger)
    .AddLogging(
	    builder => builder.AddTestLogging(testLogAccessor))
    .BuildServiceProvider();

// ... then your steps class constructor ...
public MyStepsClass(ILogger<MyTestSubject> logger)
{
    // Use the logger instance, maybe assign to a private field and use inside steps when the MyTestSubject class is instantiated.
}
```

## Useful References

* See the [GivePenny GherkinSpec documentation](https://github.com/GivePenny/GherkinSpec/docs/Hooks.md) for background reading on the `[BeforeRun]` hook.
* See the [https://github.com/GivePenny/GherkinSpec.ComplexExample](complex example repo) for an example of a BeforeRun hook that you could modify as described above.
