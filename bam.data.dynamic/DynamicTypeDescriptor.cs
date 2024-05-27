using Bam.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Data.Dynamic.Data;

namespace Bam.Data.Dynamic
{
    [Serializable]
    public class DynamicTypeDescriptor: KeyedAuditRepoData
    {
        public DynamicTypeDescriptor()
        { }

        public ulong DynamicNamespaceDescriptorId { get; set; }
        public virtual DynamicNamespaceDescriptor DynamicNamespaceDescriptor { get; set; }

        [CompositeKey]
        public string TypeName { get; set; }

        public virtual List<DynamicTypePropertyDescriptor> Properties { get; set; }
    }
}
