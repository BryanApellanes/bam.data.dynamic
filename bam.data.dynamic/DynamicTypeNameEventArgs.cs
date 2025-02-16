namespace Bam.Data.Dynamic
{
    public class DynamicTypeNameEventArgs: EventArgs
    {
        public string Input { get; set; }
        public string ResolvedTypeName { get; set; }
        public DynamicTypeFormats Format { get; set; }
    }
}
