using System.Data.Common;
using Bam.Net;

namespace Bam.Data
{
    public static partial class Sql
    {
        public static IEnumerable<dynamic> ExecuteDynamicReader(this string sqlStatement, Database db, params DbParameter[] parameters)
        {
            DbDataReader reader = db.ExecuteReader(sqlStatement, parameters, out DbConnection conn);
            try
            {
                if (reader != null && reader.HasRows)
                {
                    List<string> columnNames = GetColumnNames(reader);
                    Type type = sqlStatement.Sha256().BuildDynamicType("Database.ExecuteDynamicReader", columnNames.ToArray());
                    while (reader.Read())
                    {
                        object next = type.Construct();
                        foreach (string cn in columnNames)
                        {
                            next.Property(cn, reader[cn]);
                        }
                        yield return next;
                    }
                }
            }
            finally
            {
                reader?.Dispose();
                db.ReleaseConnection(conn);
            }
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
