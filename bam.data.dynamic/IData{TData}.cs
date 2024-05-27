using bam.data.dynamic.Data;
using Bam.Data;
using Bam.Data.Repositories;

namespace Bam.Data.Dynamic;

public interface IData<TData>
{
    GetPropertyResult GetProperty(string propertyName);
    GetPropertyResult<TProp> GetProperty<TProp>(string propertyName);

    SetPropertyResult SetProperty(string propertyName, object value);

    IDao ToDao();

    IRepoData ToRepoData();
}