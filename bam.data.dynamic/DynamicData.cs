using Amazon.Runtime.Internal.Transform;
using Bam.Data.Dynamic.Data;

namespace Bam.Data.Dynamic;

public class DynamicData : IData
{
    public DynamicData(Dictionary<object, object> data)
    {
        this.Data = data;
    }

    public Dictionary<object, object> Data { get; }

    public Dictionary<object, object> this[string key]
    {
        get
        {
            if (Data[key] is Dictionary<object, object>  value)
            {
                return value;
            }

            return new Dictionary<object, object>()
            {
                { key, Data[key] }
            };
        }
    }
    
    public GetPropertyResult GetProperty(string propertyName)
    {
        try
        {
            Data.TryGetValue(propertyName, out object? value);
            return new GetPropertyResult()
            {
                Parent = this,
                PropertyName = propertyName,
                Success = true,
                Value = value
            };
        }
        catch (Exception ex)
        {
            return new GetPropertyResult()
            {
                Parent = this,
                Message = ex.Message,
                PropertyName = propertyName,
                Success = false
            };
        }
    }

    public GetPropertyResult<TProp?> GetProperty<TProp>(string propertyName)
    {
        try
        {
            Data.TryGetValue(propertyName, out object? value);
            return new GetPropertyResult<TProp?>()
            {
                Parent = this,
                PropertyName = propertyName,
                Success = true,
                Value = value == null ? default: (TProp)value
            };
        }
        catch (Exception ex)
        {
            return new GetPropertyResult<TProp?>()
            {
                Parent = this,
                Message = ex.Message,
                PropertyName = propertyName,
                Success = false
            };
        }
    }

    public SetPropertyResult SetProperty(string propertyName, object value)
    {
        try
        {
            Data[propertyName] = value;
            return new SetPropertyResult()
            {
                Parent = this,
                PropertyName = propertyName,
                Success = true,
                Value = value
            };
        }
        catch (Exception ex)
        {
            return new SetPropertyResult()
            {
                Message = ex.Message,
                Parent = this,
                PropertyName = propertyName,
                Success = false,
                Value = value
            };
        }
    }
}