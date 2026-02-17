namespace Bam.Schema.Json
{
    public class JSchemaEnumGeneratorEventArgs : EventArgs
    {
        public JSchemaEnumGeneratorEventArgs(JSchemaEnumGenerator generator)
        {
            this.Generator = generator;
        }
        public Exception Exception { get; set; } = null!;
        public JSchemaEnumGenerator Generator { get; set; } = null!;
        public JSchemaSchemaDefinition JSchemaSchemaDefinition { get; set; } = null!;
        public EnumModel Model { get; set; } = null!;
        public string CodeFile { get; set; } = null!;
    }
}