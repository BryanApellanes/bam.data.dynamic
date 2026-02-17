namespace Bam.Data.Dynamic
{
    public class DynamicTypeNameEventArgs: EventArgs
    {
        public string Input { get; set; } = null!;
        public string ResolvedTypeName { get; set; } = null!;
        public DynamicTypeFormats Format { get; set; }
    }
}
