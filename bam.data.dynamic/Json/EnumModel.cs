namespace Bam.Schema.Json
{
    public class EnumModel
    {
        public EnumModel()
        {
        }

        public EnumModel(JSchemaClass jSchemaClass, string nameSpace)
        {
            Namespace = nameSpace;
            Name = jSchemaClass.ClassName;
            Values = jSchemaClass.GetEnumNames().ToArray();
        }

        public string Namespace { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string[] Values { get; set; } = null!;
    }
}