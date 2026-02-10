using Bam.Console;
using Bam.Test;

namespace Bam.Data.Dynamic.Tests.Unit;

[UnitTestMenu("Dictionary Should")]
public class DictionaryShould : UnitTestMenuContainer
{
    [UnitTest]
    public void ConvertToDto()
    {
        Dictionary<object, object> data = new Dictionary<object, object>()
        {
            { "Name", "name Value" },
            {"TypeName", "MyTestDynamicType"},
            {"Age", 800}
        };

        string testTypeName = "MyTestDynamicType";

        When.A<Dictionary<object, object>>("converts to a dynamic DTO",
            data,
            (d) =>
            {
                dynamic? obj = d.ToDynamic(testTypeName);
                return obj;
            })
        .TheTest
        .ShouldPass(because =>
        {
            because.TheResult.IsNotNull();
            dynamic obj = because.Result;
            because.ItsTrue("type name matches", testTypeName.Equals(obj?.GetType().Name));
            because.ItsTrue("Name equals expected", data["Name"].Equals(obj?.Name));
            because.ItsTrue("TypeName equals expected", data["TypeName"].Equals(obj?.TypeName));
            because.ItsTrue("Age equals expected", data["Age"].Equals(obj?.Age));
        })
        .SoBeHappy()
        .UnlessItFailed();
    }
}
