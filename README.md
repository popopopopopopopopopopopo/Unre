## Unre - a simple undo/redo library for .Net
========================================

[![Build status](https://ci.appveyor.com/api/projects/status/Unrehogemoge)](https://ci.appvayor.com/api/Unrehogemoge)

Release Notes
-------------

[Located at Unre](https://github.com/popopopopopopopopopopopo/Unre)

Features
--------
Unre is a [NuGet library](https://www.nuget.org/packages/Unre/) that you can add in to your project that will extend your `T where T : class` class.

Execute Do/Undo/Redo a static Repository
------------------------------------------------------------

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


License
---------------------
MIT


Who is using this?
---------------------
Unre is in production use at some enterprise systems.