# Every delegate lambda contains the whole graph of the object it creates again

**Pure.DI master (71f24f4)** and **2.5.7**, .NET SDK 10.0.403, macOS. Repro project attached (`dotnet build`, about 4 s, 10,483 generated lines).

Follow-up to #175 and #178, and the reason the `Func<T1, T>` delegates of a real app can't be generated: the body of every delegate lambda contains the full construction of the object it creates, including that object's own delegate lambdas. A chain of view models that each open the next one through two delegates is therefore generated 2^N times. That happens even when the delegate's argument reaches only the created object (`ctx.Let`), where each view model exists in only two variants.

## Minimal code

Every view model consumes the same services. It opens the next one either with an overridden `Ai` or without one.

```csharp
using Pure.DI;

var composition = new Composition();
Console.WriteLine(composition.Root.GetType().Name);

public sealed class A0;
// ... same pattern ...
public sealed class A9;

public sealed class Vm0(A0 a0, /* ... */ A9 a9, Func<A0, Vm1> openWith, Func<Vm1> open);
public sealed class Vm1(A0 a0, /* ... */ A9 a9, Func<A1, Vm2> openWith, Func<Vm2> open);
// ... same pattern ...
public sealed class Vm9(A0 a0, /* ... */ A9 a9, Func<A9, Vm10> openWith, Func<Vm10> open);
public sealed class Vm10(A0 a0, /* ... */ A9 a9);

partial class Composition
{
    private static void Setup() => DI.Setup(nameof(Composition))
        // The argument reaches only the created view model
        .Bind<Func<TT1, TT>>().As(Lifetime.PerBlock).To(ctx => new Func<TT1, TT>(arg1 =>
        {
            ctx.Let<TT1>(arg1);
            ctx.Inject<TT>(ctx.Tag, out var value);
            return value;
        }))
        .Bind<A0>().To<A0>()
        // ... every Ai ...
        .Bind<A9>().To<A9>()
        .Root<Vm0>("Root");
}
```

## Expected

With `ctx.Let`, `VmN` is created in two ways: by `Func<AN-1, VmN>` with the overridden `AN-1`, or by `Func<VmN>` with everything bound. The output should grow linearly with the number of view models, for example with one local function per variant that the lambdas call.

## Actual

| view models | generated lines, `ctx.Let` delegates | generated lines, built-in delegates (`-p:BuiltInFunc=true`) | constructions of the last view model |
|---|---|---|---|
| 6 | 883 | 1,135 | 64 |
| 8 | 2,803 | 3,823 | 256 |
| 10 (attached) | 10,483 | 14,575 | 1,024 |
| 12 | 41,203 | 57,583 | 4,096 |

Master and 2.5.7 give the same numbers. With 3 view models, the last one is constructed 8 times, and only 2 of those differ:

```csharp
return new Vm3(new A0(), new A1(), new A2());
return new Vm3(new A0(), new A1(), overriddenA2);
return new Vm3(new A0(), new A1(), new A2());
return new Vm3(new A0(), new A1(), overriddenA2);
return new Vm3(new A0(), new A1(), new A2());
return new Vm3(new A0(), new A1(), overriddenA2);
return new Vm3(new A0(), new A1(), new A2());
return new Vm3(new A0(), new A1(), overriddenA2);
```

With the built-in delegates (`ctx.Override`), the rewrite also has to tell apart every combination of arguments passed along the way. In a real app the view models open each other in cycles, so on master the rewrite never finishes: after 2.5 minutes it had made 529,000 rewrites (one per distinct set of overrides, 5,012 copies of one view model) at a depth of 470 nodes. With `ctx.Let` delegates the same rewrite takes 1 s, which makes this code size the next problem.
