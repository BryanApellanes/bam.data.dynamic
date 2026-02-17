using Bam.Data.Dynamic.Data;

namespace Bam.Data.Dynamic
{
    [Serializable]
    public class DynamicTypeManagerEventArgs : EventArgs
    {
        public DynamicTypeDescriptor[] DynamicTypeDescriptors { get; set; } = null!;
        public DynamicTypePropertyDescriptor[] DynamicTypePropertyDescriptors { get; set; } = null!;
        public string TypeName { get; set; } = null!;
        public string FoundTypes { get; set; } = null!;
    }
}
