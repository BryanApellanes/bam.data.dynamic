namespace Bam.Data.Dynamic
{
    public class PropertyDescriptor
    {
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public override string ToString()
        {
            return $"{Type}:{Name}";
        }
    }
}
