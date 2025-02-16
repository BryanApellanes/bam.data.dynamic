using Bam.Console;
using Bam.OpenApi;
using Bam.Test;

namespace Bam.Data.Dynamic.Tests.Unit;

[UnitTestMenu("DynamicTypeManagerShould")]
public class OpenApiDynamicTypeManagerShould : UnitTestMenuContainer
{
    public const string SPEC = "./management-minimal.yaml";
    public const string WRITTEN = "./management-minimal-written.yaml";
    public const string TX = "./management-minimal-tx.yaml";

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
    public async Task CreateTypeDescriptors()
    {
        FileInfo rawFile = new FileInfo(SPEC);
        FileInfo writtenFile = new FileInfo(WRITTEN);
        Dictionary<object, object>? rawData = rawFile.FromYamlFile<Dictionary<object, object>>();
        rawData.ToYamlFile(writtenFile);

        OpenApiSpecNavigator specNavigator = new OpenApiSpecNavigator(writtenFile.FullName);

        Dictionary<object, object>? schemas = specNavigator["components", "schemas"];

        OpenApiDynamicTypeManager dynamicTypeManager = Get<OpenApiDynamicTypeManager>();

        Dictionary<object, object>? inlineEnum = specNavigator["components", "schemas", "UserFactorActivateResponse"];
        if (inlineEnum != null)
        {
            DynamicTypeDescriptor descriptor =  dynamicTypeManager.CreateTypeDescriptor("UserFactorActivateResponse", inlineEnum.GetValue("properties"));
            Message.PrintLine(descriptor.TypeName);
        }

        List<DynamicTypeDescriptor> dynamicTypeDescriptors = dynamicTypeManager.CreateTypeDescriptors(schemas).ToList();
        List<DynamicTypeDescriptor> inlineEnums = dynamicTypeDescriptors.Where(t => t.Properties.Where(p =>
        {
            if (p is OpenApiDynamicTypePropertyDescriptor dtp)
            {
                return dtp.IsInlineEnum;
            }

            return false;
        }).Any()).ToList();

        foreach (DynamicTypeDescriptor descriptor in inlineEnums)
        {
            Message.PrintLine($"{descriptor.TypeName} : {string.Join(",", descriptor.Properties.Select(p=> p.PropertyName))}");
        }
    }
    

    private void PrintKeyValuePairs(Dictionary<object, object>? dictionary)
    {
        if (dictionary == null)
        {
            return;
        }
        foreach (object key in dictionary.Keys)
        {
            object value = dictionary[key];
            Message.PrintLine($"\t{key} (type={key.GetType().Name}): {value.GetType().Name} {value ?? "null"}");
        }
    }
    
}