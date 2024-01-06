using Bam.Data.dynamic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
