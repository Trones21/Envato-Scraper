using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebScraper._Generic
{
        public static class ParameterizedQueries
        {
            public static int CountInsertedToday(WebScrapeDbContext dbcontext, string SchemaTable, string DateColumn)
            {

                using (dbcontext)
                {
                    SqlParameter TableParam = new SqlParameter("@Table", SchemaTable);
                    SqlParameter DateFieldParam = new SqlParameter("@Date", DateColumn);

                    var sql = String.Format(@"select count({0}) from {1} where {0} = Convert(date, GetDate())", DateColumn, SchemaTable);
                    return dbcontext.Database.SqlQuery<int>(sql).First<int>();
                }

            }
        }    
}
