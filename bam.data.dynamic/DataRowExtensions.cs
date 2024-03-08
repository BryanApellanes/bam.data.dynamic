using Bam.Data.Dynamic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Data.Dynamic
{
    public static class DataRowExtensions
    {
        public static dynamic ToDynamic(this DataRow row, string typeName, string nameSpace = null)
        {
            return row.ToDictionary().ToDynamic(typeName, nameSpace);
        }

        public static Dictionary<object, object> ToDictionary(this DataRow row)
        {
            Dictionary<object, object> result = new Dictionary<object, object>();
            foreach (DataColumn column in row.Table.Columns)
            {
                result.Add(column.ColumnName, row[column]);
            }

            return result;
        }
    }
}
