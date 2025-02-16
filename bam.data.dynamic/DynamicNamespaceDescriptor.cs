using Bam.Data.Repositories;

namespace Bam.Data.Dynamic
{
    [Serializable]
    public class DynamicNamespaceDescriptor: KeyedAuditRepoData
    {
        public DynamicNamespaceDescriptor()
        {
        }

        public static string DefaultNamespace => "Bam.Data.Dynamic.RuntimeTypes";

        [CompositeKey]
        public string Namespace { get; set; }

        public virtual List<DynamicTypeDescriptor> Types { get; set; }
    }
}
