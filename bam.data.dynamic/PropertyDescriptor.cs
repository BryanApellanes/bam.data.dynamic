namespace Bam.Data.Dynamic
{
    public class PropertyDescriptor
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public override string ToString()
        {
            return $"{Type}:{Name}";
        }
    }
}
