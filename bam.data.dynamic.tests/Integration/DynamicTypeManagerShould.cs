using Bam.Data.Dynamic;
using Bam.Test;

namespace Bam.Application.Integration;

[UnitTestMenu("DynamicTypeManager should")]
public class DynamicTypeManagerShould : UnitTestMenuContainer
{
    public DynamicTypeManagerShould()
    {
        Configure((svcRegistry) =>
        {
            svcRegistry
                .For<IDynamicTypeNameResolver>().Use<DynamicTypeNameResolver>()
                .For<IEnumerableItemDynamicTypeNameResolver>().Use<EnumerableItemDynamicTypeNameResolver>();
        });
    }

    [UnitTest]
    public void CreateDescriptorsForNestedDictionary()
    {
        When.A<DynamicTypeManager>("is resolved from service registry",
            () => Get<DynamicTypeManager>(),
            (dynamicTypeManager) => dynamicTypeManager)
        .TheTest
        .ShouldPass(because =>
        {
            because.TheResult.IsNotNull();
        })
        .SoBeHappy()
        .UnlessItFailed();
    }
}
