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
    public async Task CreateDescriptorsForNestedDictionary()
    {
        DynamicTypeManager dynamicTypeManager = Get<DynamicTypeManager>();
        
    }
}