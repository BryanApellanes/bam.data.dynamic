using Bam.Data.Dynamic.Data;
using Bam.Data.SQLite;
using Bam.Net;
using Bam.Test;
using System.Reflection;
using System.Reflection.Emit;

namespace Bam.Data.Dynamic.Tests.Unit;

[UnitTestMenu("DynamicTypeBuilder should", "dtb")]
public class DynamicTypeBuilderShould : UnitTestMenuContainer
{
    [UnitTest]
    public void BuildTypeFromPropertyNames()
    {
        string typeName = $"Test_{8.RandomLetters()}";
        string[] propertyNames = new[] { "FirstName", "LastName", "Age" };

        When.A<string>("builds a dynamic type from property names",
            typeName,
            (tn) =>
            {
                Type dynamicType = tn.BuildDynamicType("DynamicTypeBuilderTests", propertyNames);
                return dynamicType;
            })
        .TheTest
        .ShouldPass(because =>
        {
            because.TheResult.IsNotNull();
            Type type = (Type)because.Result;
            because.ItsTrue("has FirstName property", type.GetProperty("FirstName") != null);
            because.ItsTrue("has LastName property", type.GetProperty("LastName") != null);
            because.ItsTrue("has Age property", type.GetProperty("Age") != null);
            because.ItsTrue("property count is 3", type.GetProperties().Length == 3);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void BuildTypeAndSetProperties()
    {
        string typeName = $"Test_{8.RandomLetters()}";

        When.A<string>("builds type and sets property values",
            typeName,
            (tn) =>
            {
                Type dynamicType = tn.BuildDynamicType("DynamicTypeBuilderTests", "Name", "Value");
                object instance = dynamicType.Construct();
                instance.Property("Name", "TestName");
                instance.Property("Value", "TestValue");
                return instance;
            })
        .TheTest
        .ShouldPass(because =>
        {
            because.TheResult.IsNotNull();
            object instance = because.Result;
            Type type = instance.GetType();
            because.ItsTrue("Name equals TestName", "TestName".Equals(type.GetProperty("Name")?.GetValue(instance)));
            because.ItsTrue("Value equals TestValue", "TestValue".Equals(type.GetProperty("Value")?.GetValue(instance)));
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void BuildTypeFromDictionary()
    {
        string typeName = $"Test_{8.RandomLetters()}";
        Dictionary<string, object> data = new()
        {
            { "City", "Seattle" },
            { "Population", 750000 },
            { "IsCapital", false }
        };

        When.A<Dictionary<string, object>>("builds type from string dictionary",
            data,
            (d) =>
            {
                Type dynamicType = d.ToDynamicType(typeName);
                object instance = dynamicType.Construct();
                foreach (string key in d.Keys)
                {
                    instance.Property(key, d[key]);
                }
                return new object[] { dynamicType, instance };
            })
        .TheTest
        .ShouldPass(because =>
        {
            because.TheResult.IsNotNull();
            object[] results = (object[])because.Result;
            Type type = (Type)results[0];
            object instance = results[1];

            because.ItsTrue("has City property", type.GetProperty("City") != null);
            because.ItsTrue("has Population property", type.GetProperty("Population") != null);
            because.ItsTrue("has IsCapital property", type.GetProperty("IsCapital") != null);
            because.ItsTrue("City equals Seattle", "Seattle".Equals(type.GetProperty("City")?.GetValue(instance)));
            because.ItsTrue("Population equals 750000", 750000.Equals(type.GetProperty("Population")?.GetValue(instance)));
            because.ItsTrue("IsCapital equals false", false.Equals(type.GetProperty("IsCapital")?.GetValue(instance)));
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void CacheDynamicTypes()
    {
        string typeName = $"Test_{8.RandomLetters()}";

        When.A<string>("caches dynamic types in DynamicTypeStore",
            typeName,
            (tn) =>
            {
                Type first = tn.BuildDynamicType("CacheTest", "Prop1");
                Type second = tn.BuildDynamicType("CacheTest", "Prop1");
                return new object[] { first, second };
            })
        .TheTest
        .ShouldPass(because =>
        {
            because.TheResult.IsNotNull();
            object[] results = (object[])because.Result;
            Type first = (Type)results[0];
            Type second = (Type)results[1];
            because.ItsTrue("cached type is same instance", ReferenceEquals(first, second));
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void BuildPersistedAssemblyAndSaveToDisk()
    {
        string typeName = $"Test_{8.RandomLetters()}";
        string outputPath = Path.Combine(Path.GetTempPath(), $"{typeName}.dll");

        After.Setup(reg =>
        {
            reg.For<string>().Use(typeName);
        })
        .When<string>("builds a persisted assembly and saves to disk", (tn, reg) =>
        {
            Type dynamicType = tn.BuildDynamicType("PersistedTest", out AssemblyBuilder assemblyBuilder, persist: true, "Name", "Age");

            string fullName = $"PersistedTest.{tn}";
            Extensions.SaveDynamicAssembly(fullName, outputPath);

            return new object[] { dynamicType, outputPath };
        })
        .TheTest
        .ShouldPass(because =>
        {
            because.TheResult.IsNotNull();
            object[] results = (object[])because.Result;
            Type type = (Type)results[0];
            string path = (string)results[1];

            because.ItsTrue("type has Name property", type.GetProperty("Name") != null);
            because.ItsTrue("type has Age property", type.GetProperty("Age") != null);
            because.ItsTrue("assembly file exists on disk", File.Exists(path));
            because.ItsTrue("assembly file is not empty", new FileInfo(path).Length > 0);

            // cleanup
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void BuildTypeWithAttributeFilter()
    {
        When.A<TestTypeWithAttributes>("builds type filtered by attribute",
            new TestTypeWithAttributes(),
            (instance) =>
            {
                Type dynamicType = instance.BuildDynamicType<SerializablePropertyAttribute>();
                return dynamicType;
            })
        .TheTest
        .ShouldPass(because =>
        {
            because.TheResult.IsNotNull();
            Type type = (Type)because.Result;
            because.ItsTrue("has Name property (serializable)", type.GetProperty("Name") != null);
            because.ItsTrue("does not have Secret property (not serializable)", type.GetProperty("Secret") == null);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ExecuteDynamicReaderOnSqlite()
    {
        string dbName = $"dynreader_{8.RandomLetters()}";
        string dbDir = Path.Combine(Path.GetTempPath(), "bam_tests");

        After.Setup(reg =>
        {
            SQLiteDatabase db = new SQLiteDatabase(dbDir, dbName);
            db.ExecuteSql("CREATE TABLE IF NOT EXISTS TestItems (Id INTEGER PRIMARY KEY, ItemName TEXT, Quantity INTEGER)");
            db.ExecuteSql("INSERT INTO TestItems (Id, ItemName, Quantity) VALUES (1, 'Widget', 100)");
            db.ExecuteSql("INSERT INTO TestItems (Id, ItemName, Quantity) VALUES (2, 'Gadget', 250)");
            db.ExecuteSql("INSERT INTO TestItems (Id, ItemName, Quantity) VALUES (3, 'Doohickey', 50)");
            reg.For<SQLiteDatabase>().Use(db);
        })
        .When<SQLiteDatabase>("executes dynamic reader and returns typed results", (db, reg) =>
        {
            List<dynamic> results = "SELECT Id, ItemName, Quantity FROM TestItems ORDER BY Id"
                .ExecuteDynamicReader(db)
                .ToList();
            return new object[] { results, db };
        })
        .TheTest
        .ShouldPass(because =>
        {
            because.TheResult.IsNotNull();
            object[] results = (object[])because.Result;
            List<dynamic> items = (List<dynamic>)results[0];
            SQLiteDatabase db = (SQLiteDatabase)results[1];

            because.ItsTrue("returned 3 items", items.Count == 3);

            dynamic first = items[0];
            Type firstType = first.GetType();
            because.ItsTrue("first item has Id property", firstType.GetProperty("Id") != null);
            because.ItsTrue("first item has ItemName property", firstType.GetProperty("ItemName") != null);
            because.ItsTrue("first item has Quantity property", firstType.GetProperty("Quantity") != null);

            // cleanup
            try
            {
                db.ReleaseAllConnections();
                string dbFile = db.DatabaseFile.FullName;
                if (File.Exists(dbFile))
                {
                    File.Delete(dbFile);
                }
            }
            catch { }
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ExecuteDynamicReaderWithNoResults()
    {
        string dbName = $"dynreader_empty_{8.RandomLetters()}";
        string dbDir = Path.Combine(Path.GetTempPath(), "bam_tests");

        After.Setup(reg =>
        {
            SQLiteDatabase db = new SQLiteDatabase(dbDir, dbName);
            db.ExecuteSql("CREATE TABLE IF NOT EXISTS EmptyTable (Id INTEGER PRIMARY KEY, Name TEXT)");
            reg.For<SQLiteDatabase>().Use(db);
        })
        .When<SQLiteDatabase>("executes dynamic reader on empty table", (db, reg) =>
        {
            List<dynamic> results = "SELECT Id, Name FROM EmptyTable"
                .ExecuteDynamicReader(db)
                .ToList();
            return new object[] { results, db };
        })
        .TheTest
        .ShouldPass(because =>
        {
            because.TheResult.IsNotNull();
            object[] results = (object[])because.Result;
            List<dynamic> items = (List<dynamic>)results[0];
            SQLiteDatabase db = (SQLiteDatabase)results[1];

            because.ItsTrue("returned 0 items", items.Count == 0);

            // cleanup
            try
            {
                db.ReleaseAllConnections();
                string dbFile = db.DatabaseFile.FullName;
                if (File.Exists(dbFile))
                {
                    File.Delete(dbFile);
                }
            }
            catch { }
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ExecuteDynamicReaderWithParameters()
    {
        string dbName = $"dynreader_params_{8.RandomLetters()}";
        string dbDir = Path.Combine(Path.GetTempPath(), "bam_tests");

        After.Setup(reg =>
        {
            SQLiteDatabase db = new SQLiteDatabase(dbDir, dbName);
            db.ExecuteSql("CREATE TABLE IF NOT EXISTS ParamItems (Id INTEGER PRIMARY KEY, Category TEXT, Price REAL)");
            db.ExecuteSql("INSERT INTO ParamItems (Id, Category, Price) VALUES (1, 'Electronics', 99.99)");
            db.ExecuteSql("INSERT INTO ParamItems (Id, Category, Price) VALUES (2, 'Clothing', 29.99)");
            db.ExecuteSql("INSERT INTO ParamItems (Id, Category, Price) VALUES (3, 'Electronics', 149.99)");
            reg.For<SQLiteDatabase>().Use(db);
        })
        .When<SQLiteDatabase>("executes dynamic reader with parameters", (db, reg) =>
        {
            var param = db.CreateParameter("@category", "Electronics");
            List<dynamic> results = "SELECT Id, Category, Price FROM ParamItems WHERE Category = @category"
                .ExecuteDynamicReader(db, param)
                .ToList();
            return new object[] { results, db };
        })
        .TheTest
        .ShouldPass(because =>
        {
            because.TheResult.IsNotNull();
            object[] results = (object[])because.Result;
            List<dynamic> items = (List<dynamic>)results[0];
            SQLiteDatabase db = (SQLiteDatabase)results[1];

            because.ItsTrue("returned 2 items (Electronics only)", items.Count == 2);

            // cleanup
            try
            {
                db.ReleaseAllConnections();
                string dbFile = db.DatabaseFile.FullName;
                if (File.Exists(dbFile))
                {
                    File.Delete(dbFile);
                }
            }
            catch { }
        })
        .SoBeHappy()
        .UnlessItFailed();
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class SerializablePropertyAttribute : Attribute { }

public class TestTypeWithAttributes
{
    [SerializableProperty]
    public string Name { get; set; } = "Test";

    public string Secret { get; set; } = "Hidden";
}
