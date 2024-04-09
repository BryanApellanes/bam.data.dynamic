namespace bam.data.dynamic.Data;

public class SetPropertyResult<TValue> : SetPropertyResult
{
    private TValue _value;
    public new TValue Value
    {
        get
        {
            if (base.Value != null)
            {
                _value = (TValue)base.Value;
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