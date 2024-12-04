namespace Bam.Data.Dynamic;

public class GetPropertyResult
{
    public object Parent { get; set; }
    public string PropertyName { get; set; }
    public object? Value { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
}