using Bam.Test;

namespace Bam.Data.Dynamic.Tests.Unit;

[UnitTestMenu("DynamicTypeDescriptorShould")]
public class DynamicTypeDescriptorShould : UnitTestMenuContainer
{
    [UnitTest]
    public void CompareByTypeNameAndAcceptNull()
    {
        When.A<DynamicTypeDescriptor>("is compared with null, another type and descriptors by type name",
            () => new DynamicTypeDescriptor { TypeName = "Widget" },
            (descriptor) => new EqualityOutcome(
                OtherType: descriptor.Equals("Widget"),
                SameName: descriptor.Equals(new DynamicTypeDescriptor { TypeName = "Widget" }),
                DifferentName: descriptor.Equals(new DynamicTypeDescriptor { TypeName = "Gadget" }),
                Null: descriptor.Equals(null)))
        .TheTest
        .ShouldPass<EqualityOutcome>((because, _, outcome) =>
        {
            because.ItsTrue("null is not equal", !outcome.Null);
            because.ItsTrue("another type is not equal", !outcome.OtherType);
            because.ItsTrue("the same type name is equal", outcome.SameName);
            because.ItsTrue("a different type name is not equal", !outcome.DifferentName);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    private sealed record EqualityOutcome(bool Null, bool OtherType, bool SameName, bool DifferentName);
}
