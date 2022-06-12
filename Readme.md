# RecordUtilities

> Save your time to determine the type as a ```record``` type in C# 9 and above.  

## WHY?
In C# 9 and above, Microsoft implemented makes compiler embed clone method (called '```<Clone>$```') to all reference record types.  So you can find a method name that equals it.  
However, the clone method hasn't existed in the value record type.  To solve this problem, this lightweight project uses the ```[CompilerGenerated]``` attributes to figure out some specific members whether or not to be a record type.

See those articles in Stackoverflow.  
- [How to copy/clone records in C# 9?](https://stackoverflow.com/a/64307423)   
- [How to check if type is a record?](https://stackoverflow.com/a/64810188)  


### HOW?
Add ```RecordHelpers.cs``` to your project first.

```csharp
using System.Reflection;

public record RecordType(int value);

// The method IsRecord will return true when the input type is record type, otherwise, false.
var sample1 = RecordHelpers.IsRecord<RecordType>(); 
var sample2 = RecordHelpers.IsRecord(typeof(RecordType));
var sample3 = typeof(RecordType).IsRecord(); 

// ArgumentNullException will be throw.
_ = default(Type).IsRecord();
```
 