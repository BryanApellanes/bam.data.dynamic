using Bam.Data.Dynamic.Data;

namespace Bam.Data.Dynamic
{
    public class DynamicDataSaveResult
    {
        public DynamicDataSaveResult() { }

        public DynamicDataSaveResult(DynamicTypeDescriptor dynamicTypeDescriptor, DataInstance dataInstance)
        {
            DynamicTypeDescriptor = dynamicTypeDescriptor;
            DataInstances = new List<DataInstance>() { dataInstance };
        }

        public DynamicTypeDescriptor DynamicTypeDescriptor { get; set; }
        public List<DataInstance> DataInstances { get; set; }
    }
}
