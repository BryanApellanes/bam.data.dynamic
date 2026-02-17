namespace Bam.Data.Dynamic;

public class GetPropertyResult<T> : GetPropertyResult
{
    public static implicit operator T(GetPropertyResult<T> v)
    {
        return v.Value;
    }
    
    private T _value = default!;
    public new T Value
    {
        get
        {
            if (base.Value != null)
            {
                _value = (T)base.Value;
            }

            return _value;
        }
        set
        {
            _value = value;
            base.Value = value;
        }
    }
}