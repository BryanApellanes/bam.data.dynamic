using System.Reflection;
using Bam.Console;
using Bam.Test;

namespace Bam.Data.Dynamic.Tests.Unit;

/// <summary>
/// Reference paths for Dao compilation skip assemblies that have no file, and work before any are set
/// (bam.data.dynamic#5).
/// </summary>
[UnitTestMenu("DaoAssemblyGenerator references should")]
public class DaoAssemblyGeneratorReferencesShould : UnitTestMenuContainer
{
    [UnitTest]
    public void BeEmptyBeforeAnyAreSet()
    {
        When.A<ReferenceExposingGenerator>("has no reference assemblies set",
            () => new ReferenceExposingGenerator(),
            (generator) => generator.Paths)
        .TheTest
        .ShouldPass<string[]>((because, _, paths) =>
        {
            because.ItsTrue("there are no reference paths", paths.Length == 0);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void SkipAnAssemblyLoadedFromBytes()
    {
        When.A<ReferenceExposingGenerator>("references an assembly loaded from bytes and one loaded from a file",
            () => new ReferenceExposingGenerator(),
            (generator) =>
            {
                Assembly fileLoaded = typeof(DaoAssemblyGenerator).Assembly;
                Assembly bytesLoaded = Assembly.Load(File.ReadAllBytes(typeof(DaoAssemblyGeneratorReferencesShould).Assembly.Location));
                generator.SetReferences(bytesLoaded, fileLoaded);
                return new ReferencePathsOutcome(generator.Paths, new FileInfo(fileLoaded.Location).FullName);
            })
        .TheTest
        .ShouldPass<ReferencePathsOutcome>((because, _, outcome) =>
        {
            because.ItsTrue("only the file-loaded assembly is referenced", outcome.Paths.Length == 1);
            because.ItsTrue("the reference is that assembly's file", outcome.Paths[0].Equals(outcome.FileLoadedPath));
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    private sealed class ReferenceExposingGenerator : DaoAssemblyGenerator
    {
        // neither collaborator is used to compute reference paths
        public ReferenceExposingGenerator() : base(null!, null!)
        {
        }

        public string[] Paths => ReferenceAssemblyPaths;

        public void SetReferences(params Assembly[] assemblies)
        {
            ReferenceAssemblies = assemblies;
        }
    }

    private sealed record ReferencePathsOutcome(string[] Paths, string FileLoadedPath);
}
