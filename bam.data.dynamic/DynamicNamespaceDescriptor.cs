using Bam.Net.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data.Dynamic.Data;

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
