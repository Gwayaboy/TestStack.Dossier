Breaking Changes
================

Version 3.3
-----------
The classes inheriting from `FileDictionarySource` have been marked as obsolete and will be removed in version 4. Instead you should use the new `Words` class, passing in the name of a file dictionary (either one of the built-in ones or one that you create). All the built-in ones are listed in the FromDictionary class where the constants match the filename embedded into Dossier. So, for example, instead of using the `GeoCountrySource` class you would instead use `Words(FromDictionary.GeoCountry)`.

All of the file dictionaries have been added to the AnonymousValueFixture class as equivalence class extension methods. So, for example, in your builder class you can now call:

```c#
Any.InternetURL();
Any.LoremIpsum();
Any.ColourName();
```

Picking functionality has been added which allows you to select items from a list according to different strategies. Currently, two strategies have been added, `RandomItemFrom` and `RepeatingSequenceFrom`:

```c#
var names = new Words(FromDictionary.PersonNameFirst).Data;
var days = new List<string> {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday"};
var customers = Builder<Customer>
	.CreateListOfSize(15)
	.All()
	.Set(x => x.Name, Pick.RandomItemFrom(names).Next)
	.Set(x => x.Day, Pick.RepeatingSequenceFrom(days).Next)
	.BuildList();
```

Version 3.0
-----------

The signature of `IAnonymousValueSupplier` has changed from:

```c#
public interface IAnonymousValueSupplier
{
    bool CanSupplyValue(Type type, string propertyName);
    TValue GenerateAnonymousValue<TObject, TValue>(AnonymousValueFixture any, string propertyName);
}
```

To:

```c#
public interface IAnonymousValueSupplier
{
    bool CanSupplyValue(Type type, string propertyName);
    object GenerateAnonymousValue(AnonymousValueFixture any, Type type, string propertyName);
}
```

Note: the `CanSupplyValue` method and the `GenerateAnonymousValue` method are no longer generic.

### Reason

In order to implement the `BuildUsing` method that allows you to build an object by convention in one line rather than having to call the constructor yourself we needed to have a non-generic version of the methods. This change actually ended up making the anonymous value suppliers slightly easier to implement (no longer any need for type casting).

### Fix

If you have any custom anonymous value suppliers change the signature of your `CanSupplyValue` and `GenerateAnonymousValue` methods so they are no longer generic.

Breaking change from NTestDataBuilder -> TestStack.Dossier 2.0
--------------------------------------------------------------

Namespace has changed from NTestDataBuilder to TestStack.Dossier.

### Reason

The project has been renamed.

### Fix

Do a global find and replace of `using NTestDataBuilder` with `using TestStack.Dossier`.

Breaking change from NTestDataBuilder -> TestStack.Dossier 2.0
--------------------------------------------------------------

When you don't `Set` a default value for a property that you later `Get` in your builder it will now generate an anonymous value for that property rather than throwing an exception.

### Reason

This is part of the work to make anonymous values a first class citizen of the library and to make it easier and quicker to tersely set up builders. This avoids the need to have boilerplate `Set` calls in your builder constructor and also means that when you generate a list of objects each object will have a different value (by default).

### Fix

The old behaviour of throwing an exception if a value hasn't been specified is no longer supported - use NTestDataBuilder version 1 if you want that or raise an issue on GitHub to explain your use case.

If you want to fix a static value for a property then by all means you can still use `Set` calls in your builder constructor. If you aren't happy with the default anonymous value that is generated for a property you can use the `Any` property to generate a value from a different equivalence class in combination with a `Set` call in your builder constructor.

Breaking change from NTestDataBuilder -> TestStack.Dossier 2.0
--------------------------------------------------------------

The way that lists are generated no longer uses NBuilder - the new syntax is backwards compatible with NBuilder except that the namespace you need to include is different. You can also refactor your list generation to be a lot more terse, but that is optional. Any `BuildList` extension methods you created will now need to be deleted since they are no longer needed. You also need to ensure that all of the methods you call are marked virtual so the list generation can proxy those method calls.

### Reason
In order to support a new, much terser syntax for generating lists we rewrote the list generation code ourselves. You can now do this:

```c#
	var customers = CustomerBuilder.CreateListOfSize(3)
		.TheFirst(1).WithFirstName("Robert")
		.TheLast(1).WithEmail("matt@domain.tld")
		.BuildList();
```

That's instead of this syntax (which still works as well):

```c#
	var customers = CustomerBuilder.CreateListOfSize(3)
		.TheFirst(1).With(b => b.WithFirstName("Robert"))
		.TheLast(1).With(b => b.WithEmail("matt@domain.tld"))
		.BuildList();
```

You also no longer need a custom extension method for the `BuildList` method so you will need to delete any of these that you have created. If you don't use NBuilder's features outside of the list generation you may uninstall the NBuilder package.

### Fix

Simply add the following to the files that generate lists of builders and change your builder modification methods to be virtual and the existing syntax should work:

```
using TestStack.Dossier.Lists;
```

Assuming you aren't using NBuilder for anything other than generating lists of entities with NTestDataBuilder 1.0 you should be able to do a global find and replace against `using FizzWare.NBuilder;`.

If you uninstall the NBuilder package then you will need to remove the using statements for that library too.

Also, remove any `BuildList` extension methods you created.
