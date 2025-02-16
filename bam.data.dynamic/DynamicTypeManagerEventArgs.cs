using Bam.Data.Dynamic.Data;

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
