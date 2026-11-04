import sys

# generate.py <levels>: rewrites Program.cs with a chain of view models that each open the next one through two delegates
k = int(sys.argv[1])
services = ", ".join(f"A{j} a{j}" for j in range(k))
lines = ["using Pure.DI;", "", "var composition = new Composition();", "Console.WriteLine(composition.Root.GetType().Name);", "",
         "// Services that every view model consumes"]
lines += [f"public sealed class A{j};" for j in range(k)]
lines += ["", "// Each view model opens the next one with or without overriding one service"]
lines += [f"public sealed class Vm{i}({services}, Func<A{i}, Vm{i + 1}> openWith, Func<Vm{i + 1}> open);" for i in range(k)]
lines += [f"public sealed class Vm{k}({services});", "",
          "partial class Composition", "{", "    private static void Setup() => DI.Setup(nameof(Composition))",
          "#if !BUILT_IN_FUNC",
          "        // The argument reaches only the created view model",
          "        .Bind<Func<TT1, TT>>().As(Lifetime.PerBlock).To(ctx => new Func<TT1, TT>(arg1 =>",
          "        {",
          "            ctx.Let<TT1>(arg1);",
          "            ctx.Inject<TT>(ctx.Tag, out var value);",
          "            return value;",
          "        }))",
          "#endif"]
lines += [f"        .Bind<A{j}>().To<A{j}>()" for j in range(k)]
lines += ['        .Root<Vm0>("Root");', "}", ""]
open("Program.cs", "w").write("\n".join(lines))
