﻿using Bam.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Data.Dynamic;

namespace Bam.Data.Dynamic.Data
{
    [Serializable]
    public class DataInstance: KeyedAuditRepoData
    {
        public ulong DynamicNameSpaceDescriptorId { get; set; }
        public virtual DynamicNamespaceDescriptor DynamicNamespaceDescriptor { get; set; }

        [CompositeKey]
        public string TypeName { get; set; }


        [CompositeKey]
        public string Namespace => DynamicNamespaceDescriptor?.Namespace;

        /// <summary>
        /// The Sha256 of the original json or yaml data
        /// </summary>
        public string RootHash { get; set; }
        /// <summary>
        /// The InstanceHash of the parent of this instance or null
        /// </summary>
        public string ParentHash { get; set; }
        /// <summary>
        /// The Sha256 of this instances json
        /// </summary>
        [CompositeKey]
        public string Instancehash { get; set; }

        public virtual List<DataInstancePropertyValue> Properties { get; set; }
    }
}
