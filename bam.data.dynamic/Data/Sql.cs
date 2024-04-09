using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Data
{
    public static partial class Sql
    {
        public static IEnumerable<dynamic> ExecuteDynamicReader(this string sqlStatement, Database db, params DbParameter[] parameters)
        {
            DbDataReader reader = null;// TODO: fix this => //db.ExecuteReader(sqlStatement, commandType, dbParameters, conn);
            //onDataReaderExecuted = onDataReaderExecuted ?? ((dr) => { });
            if (reader.HasRows)
            {
                
                List<string> columnNames = GetColumnNames(reader);
                // TODO: Reimplement BuildDynamicType using current set of templates and compilers
                //Type type = sqlStatement.Sha256().BuildDynamicType("Database.ExecuteDynamicReader", columnNames.ToArray());
                while (reader.Read())
                {
                    /*
                    object next = type.Construct();
                    columnNames.Each(new { Value = next, Reader = reader }, (ctx, cn) =>
                    {
                        ReflectionExtensions.Property(ctx.Value, cn, ctx.Reader[cn]);
                    });
                    yield return next;
                    */
                }
            }
/*            if (closeConnection)
            {
                ReleaseConnection(conn);
            }
            onDataReaderExecuted(reader);*/
            yield break;
        }

        private static List<string> GetColumnNames(DbDataReader reader)
        {
            List<string> columnNames = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                columnNames.Add(reader.GetName(i));
            }

            return columnNames;
        }
    }
}
