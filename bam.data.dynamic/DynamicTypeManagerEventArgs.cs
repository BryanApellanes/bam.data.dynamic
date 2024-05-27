using Bam.Data.Dynamic.Data;
using Bam.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Data.Dynamic;

namespace Bam.Data.Dynamic
{
    [Serializable]
    public class DynamicTypeManagerEventArgs : EventArgs
    {
        public DynamicTypeDescriptor[] DynamicTypeDescriptors { get; set; }
        public DynamicTypePropertyDescriptor[] DynamicTypePropertyDescriptors { get; set; }
        public string TypeName { get; set; }
        public string FoundTypes { get; set; }
    }
}
