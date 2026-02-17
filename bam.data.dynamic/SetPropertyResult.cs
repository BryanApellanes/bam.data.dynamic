namespace Bam.Data.Dynamic.Data;

public class SetPropertyResult
{
    public object Parent { get; set; } = null!;
    public string PropertyName { get; set; } = null!;
    public object Value { get; set; } = null!;
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
}