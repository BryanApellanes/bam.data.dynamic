using System.Data;

namespace Bam.Data.Dynamic
{
    public static class DataTableExtensions
    {
        public static IEnumerable<dynamic> ToDynamic(this DataTable table, string typeName, string nameSpace = null)
        {
            foreach (DataRow row in table.Rows)
            {
                yield return row.ToDynamic(typeName, nameSpace);
            }
        }
    }
}
