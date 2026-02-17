using Bam.Data.Repositories;

namespace Bam.Data.Dynamic.Data
{
    [Serializable]
    public class DataInstancePropertyValue: KeyedAuditRepoData
    {
        public ulong DataInstanceId { get; set; }
        public virtual DataInstance DataInstance { get; set; } = null!;
        public string RootHash { get; set; } = null!;
        public string InstanceHash { get; set; } = null!;

        [CompositeKey]
        public string ParentTypeNamespace { get; set; } = null!;

        [CompositeKey]
        public string ParentTypeName { get; set; } = null!;

        [CompositeKey]
        public string PropertyName { get; set; } = null!;

        [CompositeKey]
        public string Value { get; set; } = null!;


    }
}
