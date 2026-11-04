using Pure.DI;

var composition = new Composition();
Console.WriteLine(composition.Root.GetType().Name);

// Services that every view model consumes
public sealed class A0;
public sealed class A1;
public sealed class A2;
public sealed class A3;
public sealed class A4;
public sealed class A5;
public sealed class A6;
public sealed class A7;
public sealed class A8;
public sealed class A9;

// Each view model opens the next one with or without overriding one service
public sealed class Vm0(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, Func<A0, Vm1> openWith, Func<Vm1> open);
public sealed class Vm1(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, Func<A1, Vm2> openWith, Func<Vm2> open);
public sealed class Vm2(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, Func<A2, Vm3> openWith, Func<Vm3> open);
public sealed class Vm3(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, Func<A3, Vm4> openWith, Func<Vm4> open);
public sealed class Vm4(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, Func<A4, Vm5> openWith, Func<Vm5> open);
public sealed class Vm5(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, Func<A5, Vm6> openWith, Func<Vm6> open);
public sealed class Vm6(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, Func<A6, Vm7> openWith, Func<Vm7> open);
public sealed class Vm7(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, Func<A7, Vm8> openWith, Func<Vm8> open);
public sealed class Vm8(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, Func<A8, Vm9> openWith, Func<Vm9> open);
public sealed class Vm9(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, Func<A9, Vm10> openWith, Func<Vm10> open);
public sealed class Vm10(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9);

partial class Composition
{
    private static void Setup() => DI.Setup(nameof(Composition))
#if !BUILT_IN_FUNC
        // The argument reaches only the created view model
        .Bind<Func<TT1, TT>>().As(Lifetime.PerBlock).To(ctx => new Func<TT1, TT>(arg1 =>
        {
            ctx.Let<TT1>(arg1);
            ctx.Inject<TT>(ctx.Tag, out var value);
            return value;
        }))
#endif
        .Bind<A0>().To<A0>()
        .Bind<A1>().To<A1>()
        .Bind<A2>().To<A2>()
        .Bind<A3>().To<A3>()
        .Bind<A4>().To<A4>()
        .Bind<A5>().To<A5>()
        .Bind<A6>().To<A6>()
        .Bind<A7>().To<A7>()
        .Bind<A8>().To<A8>()
        .Bind<A9>().To<A9>()
        .Root<Vm0>("Root");
}
