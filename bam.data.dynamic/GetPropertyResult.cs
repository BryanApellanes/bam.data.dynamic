namespace Bam.Data.Dynamic;

public class GetPropertyResult
{
    public object Parent { get; set; } = null!;
    public string PropertyName { get; set; } = null!;
    public object? Value { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
}