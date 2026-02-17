using Bam.Generators;
using Bam.Logging;
using Loggable = Bam.Logging.Loggable;

namespace Bam.Schema.Json
{
    public class JSchemaEnumGenerator : Loggable
    {
        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler GeneratingEnums = null!;

        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler GeneratedEnums = null!;

        [Verbosity(VerbosityLevel.Error)]
        public event EventHandler GeneratingEnumsException = null!;

#pragma warning disable CS0067, CS0414
        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler WritingCodeFile = null!;

        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler WroteCodeFile = null!;
#pragma warning restore CS0067, CS0414
        public ILogger Logger { get; set; } = null!;

        public string Workspace { get; set; } = null!;
        public void GenerateEnums(JSchemaSchemaDefinition jSchemaSchemaDefinition, string nameSpace)
        {
            try
            {
                FireEvent(GeneratingEnums, this, new JSchemaEnumGeneratorEventArgs(this){JSchemaSchemaDefinition = jSchemaSchemaDefinition});
                HashSet<JSchemaClass> jSchemaClasses = jSchemaSchemaDefinition.Classes;
                
                foreach (JSchemaClass jSchemaClass in jSchemaClasses.Where(c=> c.IsEnum))
                {
                    EnumModel model = new EnumModel(jSchemaClass, nameSpace);
                    string code = Handlebars.Render("Enum", model);
                    code.SafeWriteToFile(Path.Combine(Workspace, $"{nameSpace}.{model.Namespace}.cs"), true);
                }
                
                FireEvent(GeneratedEnums, this, new JSchemaEnumGeneratorEventArgs(this){JSchemaSchemaDefinition = jSchemaSchemaDefinition});
            }
            catch (Exception ex)
            {
                Logger.Error("Error generating enums: {0}", ex.Message);
                FireEvent(GeneratingEnumsException, this, new JSchemaEnumGeneratorEventArgs(this){Exception = ex});
            }
        }
    }
}