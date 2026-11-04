# Pure.DI: every delegate lambda contains the whole graph of the object it creates again

Minimal repro for Pure.DI master (71f24f4) and 2.5.7: a chain of 10 view models, each opening the next one through two delegates. The last view model is constructed 1,024 times in the generated code (10,483 lines), although it only exists in two variants.

[`ISSUE.md`](ISSUE.md) is the text to paste into a Pure.DI GitHub issue; it has the measurements from 6 to 12 levels.

```
dotnet build                                                                                   # 2.5.7 from nuget.org
dotnet build -p:PureDIVersion=<master build version> -p:RestoreAdditionalProjectSources=<folder with the package>
dotnet build -p:BuiltInFunc=true                                                               # built-in Func<T1, T> (ctx.Override) instead of the ctx.Let binding
python3 generate.py <levels>                                                                   # regenerates Program.cs with another depth
```
