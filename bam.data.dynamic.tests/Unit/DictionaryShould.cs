using Bam.Test;

namespace Bam.Data.Dynamic.Tests.Unit;

[UnitTestMenu("Dictionary Should")]
public class DictionaryShould : UnitTestMenuContainer
{
    [UnitTest]
    public async Task ConvertToDto()
    {
        Dictionary<object, object> data = new Dictionary<object, object>()
        {
            { "Name", "name Value" },
            {"TypeName", "MyTestDynamicType"},
            {"Age", 800}
        };

        string testTypeName = "MyTestDynamicType";
        dynamic? obj = data.ToDto(testTypeName);
        Expect.IsNotNull(obj);
        Expect.AreEqual(testTypeName, obj?.GetType().Name);
        Expect.AreEqual(obj?.Name, data["Name"]);
        Expect.AreEqual(obj?.TypeName, data["TypeName"]);
        Expect.AreEqual(obj?.Age, data["Age"]);
    }
    
}