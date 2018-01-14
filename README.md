## Unre - a simple undo/redo library for .Net On Multi-platform
========================================

[![Build status](https://ci.appveyor.com/api/projects/status/Unrehogemoge)](https://ci.appvayor.com/api/Unrehogemoge)

TargetFrameworks
-------------
^.Net Core2.0
^.Net Standard2.0
^.NET 4.5

Multi-platform-oriented

Release Notes
-------------

[Located at Unre](https://github.com/popopopopopopopopopopopo/Unre)

Features
--------
Unre is a [NuGet library](https://www.nuget.org/packages/Unre/) that you can add in to your project that will extend your `T where T : class` class.

And,
We are planning to update the document in the future.

Execute Do/Undo/Redo a Instance Repository(Recommend)
------------------------------------------------------------

```csharp
public UnreRepository Repository = new UnreRepository<T>();
```
Example usage:

```csharp
public class Person
{
    public int? Age { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
}            

var person = new Person();
var repo = new UnreRepository<Person>();

Console.WriteLine(person.Id.ToString());
repo.Do(person);
repo.Do(new Person());
var undoPerson = repo.Undo();
Console.WriteLine(undoPerson.Id.ToString());

var isEqual = undoPerson.Equals(person);
Console.WriteLine(isEqual.ToString());

Console.ReadKey();

```

Execute Do/Undo/Redo a static Repository(Not Recommend)
------------------------------------------------------------


We recommend using this pattern under limited circumstances, 
such as classes used only in one place.

```csharp
public UnreRepository Repository = UnreRepository<T>.Instance;
```
Example usage:

```csharp
public class Person
{
    public int? Age { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
}            

var person = new Person();
var repo = UnreRepository<Person>.Instance;

Console.WriteLine(person.Id.ToString());
repo.Do(person);
repo.Do(new Person());
var undoPerson = repo.Undo();
Console.WriteLine(undoPerson.Id.ToString());

var isEqual = undoPerson.Equals(person);
Console.WriteLine(isEqual.ToString());

Console.ReadKey();

```

For More Documents
---------------------
[With Action](WithAction.md)

[Set Commands And Raise](SetCommandsAndRaise.md)

License
---------------------
MIT


Who is using this?
---------------------
Unre is in production use at some enterprise systems.

How To Read
---------------------

An a ̄

[ja-jp]
あんあー
