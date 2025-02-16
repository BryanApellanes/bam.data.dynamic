namespace Bam.Data.Dynamic
{
    public class DataFile
    {
        public DataFile()
        {
            Namespace = DynamicNamespaceDescriptor.DefaultNamespace;
        }

        public string Namespace { get; set; }
        public string TypeName { get; set; }
        public FileInfo FileInfo { get; set; }
    }
}
