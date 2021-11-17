# NavigatorCSharp

A [safe navigation](https://en.wikipedia.org/wiki/Safe_navigation_operator) library for C#,
it allows to check nested objects and collections for values while also providing a way to
obtain the current navigation path.

## Examples

Basic example:
```
  var foo = new Foo { Bar = new Bar { Prop = "Hey!" } };
  var navigation = NavigationFactory.Create(foo).For(f => f.Bar.Prop);
  
  // This will return true if "Prop" can be reached, i.e., "Bar" is not null
  if (navigation.TryGetValue(out var propValue))
  {
    // Notice that a valid navigation can lead to a null value
    if (propValue != null)
    {
      // Prints "Bar.Prop: Hey!"
      Console.WriteLine($"{navigation.GetPath()}: {propValue}");
    }
    else
    {
      Console.WriteLine($"{navigation.GetPath()} could be reached but it's null, darn it!");
    }
  }
  else
  {
    Console.WriteLine($"Could not read '{navigation.GetPath()}'");
  }
```

Or using exception handling:
```
  // This is always safe
  // And you can chain "For" calls
  var navigation = NavigationFactory.Create(foo).For(f => f.Bar).For(b => b.Prop);
  
  try
  {
    Console.WriteLine($"{navigation.GetPath()}: {navigation.GetValue() ?? "null"}");
  }
  catch (InvalidNavigationException)
  {
    Console.WriteLine($"Could not read '{navigation.GetPath()}'");
  }
```

It also supports collections, `ForEach` returns an IEnumerable of navigations and `GetPath()` will include the current index:
```
  var model = new MyModel { SomeCollection = new [] { "Hey!", default, "And so on..." } };
  
  // "values" will contain "SomeCollection[0]: Hey!", "SomeCollection[1]: Nothing here", "SomeCollection[2]: And so on...", 
  var values = NavigationFactory.Create(model)
    .ForEach(m => m.SomeCollection)
    .Select(itemNavigation =>
    {
      var itemString = itemNavigation.TryGetValue(out var itemValue) && itemValue != null ? itemValue  : "Nothing here";
      return $"{itemNavigation.GetPath()}: {itemString}";
    })
    .ToArray();
```

And you can use conditions to invalidate navigations:
```
  var foo = new Foo { Bar = new Bar { Prop = "Hey!" } };
  // Throws an InvalidNavigationException
  var navigation = NavigationFactory.Create(foo).For(f => f.Bar.Prop).When(p => p != "Hey!").GetValue();
```
