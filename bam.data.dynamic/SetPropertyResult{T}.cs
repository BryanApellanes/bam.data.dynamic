namespace Bam.Data.Dynamic.Data;

public class SetPropertyResult<TValue> : SetPropertyResult
{
    private TValue _value = default!;
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
            base.Value = value!;
        }
    }
}