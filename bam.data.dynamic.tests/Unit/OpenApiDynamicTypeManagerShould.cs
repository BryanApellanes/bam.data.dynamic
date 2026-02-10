using Bam.Console;
using Bam.OpenApi;
using Bam.Test;

namespace Bam.Data.Dynamic.Tests.Unit;

[UnitTestMenu("OpenApiDynamicTypeManagerShould")]
public class OpenApiDynamicTypeManagerShould : UnitTestMenuContainer
{
    public const string SPEC = "./management-minimal.yaml";
    public const string WRITTEN = "./management-minimal-written.yaml";

    public OpenApiDynamicTypeManagerShould()
    {
        Configure((svcRegistry) =>
        {
            svcRegistry
                .For<IDynamicTypeNameResolver>().Use<DynamicTypeNameResolver>()
                .For<IEnumerableItemDynamicTypeNameResolver>().Use<EnumerableItemDynamicTypeNameResolver>();
        });
    }

    [UnitTest]
    public void CreateTypeDescriptors()
    {
        When.A<OpenApiDynamicTypeManager>("creates type descriptors from OpenAPI spec",
            () => Get<OpenApiDynamicTypeManager>(),
            (dynamicTypeManager) =>
            {
                FileInfo rawFile = new FileInfo(SPEC);
                FileInfo writtenFile = new FileInfo(WRITTEN);
                Dictionary<object, object>? rawData = rawFile.FromYamlFile<Dictionary<object, object>>();
                rawData.ToYamlFile(writtenFile);

                OpenApiSpecNavigator specNavigator = new OpenApiSpecNavigator(writtenFile.FullName);
                Dictionary<object, object>? schemas = specNavigator["components", "schemas"];

                List<DynamicTypeDescriptor> dynamicTypeDescriptors = dynamicTypeManager.CreateTypeDescriptors(schemas).ToList();
                return dynamicTypeDescriptors;
            })
        .TheTest
        .ShouldPass(because =>
        {
            because.TheResult.IsNotNull();
        })
        .SoBeHappy()
        .UnlessItFailed();
    }
}
