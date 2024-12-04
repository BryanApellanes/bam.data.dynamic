using Bam.Data.Dynamic.Data;
using Bam.Data;
using Bam.Data.Repositories;

namespace Bam.Data.Dynamic;

public interface IData
{
    GetPropertyResult GetProperty(string propertyName);
    GetPropertyResult<TProp?> GetProperty<TProp>(string propertyName);

    SetPropertyResult SetProperty(string propertyName, object value);
}