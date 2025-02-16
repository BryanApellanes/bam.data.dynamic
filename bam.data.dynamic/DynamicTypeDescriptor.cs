using Bam.Data.Repositories;
using Bam.Data.Dynamic.Data;

namespace Bam.Data.Dynamic
{
    [Serializable]
    public class DynamicTypeDescriptor: KeyedAuditRepoData
    {
        public ulong DynamicNamespaceDescriptorId { get; set; }
        public virtual DynamicNamespaceDescriptor? DynamicNamespaceDescriptor { get; set; }

        [CompositeKey]
        public string? TypeName { get; set; }

        private List<DynamicTypePropertyDescriptor>? _properties;
        public virtual List<DynamicTypePropertyDescriptor> Properties
        {
            get => _properties ??= new List<DynamicTypePropertyDescriptor>();
            set => _properties = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is DynamicTypeDescriptor other)
            {
                return other?.TypeName?.Equals(TypeName) == true && PropertiesEqual(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TypeName, Properties);
        }

        private bool PropertiesEqual(DynamicTypeDescriptor other)
        {
            if (other.Properties.Count != Properties.Count)
            {
                return false;
            }

            foreach (DynamicTypePropertyDescriptor descriptor in Properties)
            {
                if (!other.Properties.Contains(descriptor))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
