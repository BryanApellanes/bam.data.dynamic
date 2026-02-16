# bam.data.dynamic

Dynamic data type management and code generation from runtime data structures, JSON Schema, and OpenAPI specifications.

## Overview

`bam.data.dynamic` provides infrastructure for working with data whose shape is not known at compile time. It can infer CLR types from dictionaries, JSON documents, YAML files, and CSV files at runtime, then generate concrete C# types and DAO (Data Access Object) assemblies on the fly using Roslyn compilation.

The library has two main axes of functionality. First, the `DynamicData` / `DynamicTypeManager` / `DynamicObject` family lets you wrap a `Dictionary<object, object>` as a typed object, creating type descriptors that capture property names and types from the dictionary keys and values. Second, the `DaoAssemblyGenerator` / `JSchemaDaoAssemblyGenerator` pipeline takes schema definitions (either extracted from existing databases or generated from JSON Schema files) and produces compiled DAO assemblies complete with CRUD methods.

A rich JSON Schema subsystem (`Bam.Schema.Json` namespace) can load `.json` or `.yaml` schema files, parse class and property names, resolve references, generate enum types, and produce `DaoSchemaDefinition` objects suitable for feeding into the DAO code generator. This makes it possible to go from an OpenAPI specification all the way to a running database-backed data layer without writing any C# model classes by hand.

## Key Classes

| Class | Description |
|---|---|
| `DynamicData` | Wraps a `Dictionary<object, object>` with get/set property accessors that return result objects. Implements `IData`. |
| `DynamicTypeManager` | Creates `DynamicTypeDescriptor` instances from dictionaries, inferring type names via pluggable `IDynamicTypeNameResolver`. |
| `DynamicTypeDescriptor` | A persistable descriptor (extends `KeyedAuditRepoData`) that records a type name and its list of property descriptors. |
| `DynamicObject` | Static factory that compiles a dictionary into a real CLR type/assembly at runtime using Roslyn, returning a `dynamic` instance. |
| `DynamicDataModel` | Model class for rendering a dynamic type from a dictionary, capturing namespace, type name, properties, and reference types. |
| `DynamicDatabase` | A dynamic CRUD interface to a `Database`, supporting Create/Retrieve/Update/Delete with anonymous-object query specs. |
| `DaoAssemblyGenerator` | Generates DAO assemblies from a `DaoSchemaDefinition` by extracting the schema, generating code, and compiling with Roslyn. |
| `JSchemaDaoAssemblyGenerator` | Extends `DaoAssemblyGenerator` to generate DAOs from JSON Schema files, including enum generation. |
| `JSchemaClassManager` | Loads JSON Schema files (JSON or YAML), resolves class names, and returns `JSchemaClass` objects for code generation. |
| `JSchemaSchemaDefinitionGenerator` | Converts a set of `JSchemaClass` definitions into a `DaoSchemaDefinition` for DAO code generation. |
| `DataInstance` | A persistable entity (extends `KeyedAuditRepoData`) representing a single data instance with root/parent/instance hashes. |
| `DynamicDataManager` | (Currently commented out) Orchestrates processing of CSV, JSON, and YAML data files into type descriptors and data instances. |

## Dependencies

### Project References
- bam.application
- bam.base
- bam.configuration
- bam.data.objects
- bam.data.repositories
- bam.data.schema
- bam.data.config
- bam.data.firebird
- bam.data.mssql
- bam.data.mysql
- bam.data.oracle
- bam.data.postgres
- bam.data
- bam.generators
- bam.logging
- bam.storage

### Package References
- CsvHelper 33.1.0
- Newtonsoft.Json 13.0.4
- Newtonsoft.Json.Schema 4.0.1
- YamlDotNet 16.3.0
- Microsoft.CodeAnalysis (direct assembly reference)

### Target Framework
- net10.0

## Usage Examples

### Creating a dynamic object from a dictionary
```csharp
Dictionary<object, object> data = new Dictionary<object, object>
{
    { "Name", "John" },
    { "Age", 30 },
    { "TypeName", "Person" }
};

// Create a real CLR type and instance at runtime
dynamic person = DynamicObject.For("Person", data);
Console.WriteLine(person.Name); // "John"
Console.WriteLine(person.Age);  // 30
```

### Using DynamicData for property access
```csharp
var dynamicData = new DynamicData(data);
GetPropertyResult result = dynamicData.GetProperty("Name");
if (result.Success)
{
    Console.WriteLine(result.Value); // "John"
}

dynamicData.SetProperty("Name", "Jane");
```

### Creating type descriptors
```csharp
var resolver = new DynamicTypeNameResolver();
var enumResolver = new EnumerableItemDynamicTypeNameResolver();
var manager = new DynamicTypeManager(resolver, enumResolver);

DynamicTypeDescriptor descriptor = manager.CreateTypeDescriptor("Person", data);
// descriptor.TypeName == "Person"
// descriptor.Properties contains DynamicTypePropertyDescriptor entries
```

### Dynamic database CRUD
```csharp
var dynDb = new DynamicDatabase(database, schemaNameMap);

// Insert
dynDb.Create(new { Table = "Customer", Name = "Acme", City = "Springfield" });

// Query
IEnumerable<dynamic> results = dynDb.Retrieve(new {
    Table = "Customer",
    Where = new { Name = "Acme" }
});

// Update
dynDb.Update(new { Table = "Customer", Name = "Acme Corp",
    Where = new { Name = "Acme" } });

// Delete
dynDb.Delete(new { Table = "Customer",
    Where = new { Name = "Acme Corp" } });
```

## Known Gaps / Not Yet Implemented

- **`EnumerableItemDynamicTypeNameResolver`**: Both methods (`ResolveItemTypeName` for arrays and enumerables) throw `NotImplementedException`.
- **`DynamicDataManager`**: The entire class body is commented out. It was designed to orchestrate processing of CSV, JSON, and YAML data files with background thread queues but is not currently active.
- **`Sql.ExecuteDynamicReader`**: The `DbDataReader` is hardcoded to `null` with a TODO to fix the reader execution. The method body for building dynamic types is also commented out.
- **`DynamicDatabase.ParseWhere`**: Has a TODO to enable multiple operators (contains, doesn't contain, starts with, etc.) beyond simple equality.
