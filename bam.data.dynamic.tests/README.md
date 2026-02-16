# bam.data.dynamic.tests

Unit and integration tests for the bam.data.dynamic library.

## Overview

This project contains tests for the dynamic data type management functionality provided by `bam.data.dynamic`. Tests are written using the Bam test framework with `[UnitTestMenu]` attributes and the `When.A<T>()` fluent API, and are executed via `BamConsoleContext.StaticMain` (a menu-driven test runner, not xUnit/NUnit).

The tests cover dictionary-to-DTO conversion, dynamic type descriptor creation from the service registry, and OpenAPI specification parsing to generate type descriptors from schema definitions.

## Key Classes

| Class | Description |
|---|---|
| `DictionaryShould` | Unit tests verifying that a `Dictionary<object, object>` can be converted to a dynamic DTO with correct property values and type name. |
| `DynamicTypeManagerShould` | Integration tests verifying that `DynamicTypeManager` can be resolved from the service registry and can create descriptors for nested dictionaries. |
| `OpenApiDynamicTypeManagerShould` | Unit tests verifying that `OpenApiDynamicTypeManager` can parse an OpenAPI YAML spec and produce type descriptors from its component schemas. |

## Dependencies

### Project References
- bam.base
- bam.openapi
- bam.test
- bam.data.dynamic

### Package References
- NSubstitute 5.3.0

### Target Framework
- net10.0 (Exe)

## Running Tests

```bash
dotnet run --project bam.data.dynamic.tests.csproj -- --ut
```

**Important**: Use `--ut` (not `/ut`) when running from Git Bash, as Git Bash rewrites `/ut` to a filesystem path.

## Known Gaps / Not Yet Implemented

- The `DynamicTypeManagerShould.CreateDescriptorsForNestedDictionary` test only verifies that the manager can be resolved from DI; it does not yet test actual nested dictionary descriptor creation.
