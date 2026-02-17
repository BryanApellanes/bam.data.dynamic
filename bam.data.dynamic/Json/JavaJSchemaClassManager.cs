namespace Bam.Schema.Json
{
    public class JavaJSchemaClassManager : JSchemaClassManager
    {
#pragma warning disable CS0169, CS0414
        private List<string> _truncations = null!;
#pragma warning restore CS0169, CS0414

        public JavaJSchemaClassManager() : base("@type", "javaType", "class", "className")
        {            
            SetClassNameMunger("javaType", javaType =>
            {
                string[] split = javaType.DelimitSplit(".");
                string typeName = split[^1];
                if (typeName.EndsWith("Entity"))
                {
                    typeName = typeName.Truncate("Entity".Length);
                }

                return typeName;
            });
        }
    }
}