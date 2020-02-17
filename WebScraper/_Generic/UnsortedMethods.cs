using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace MyWebScraper
{
    public class UnsortedMethods
    {
        public static IEnumerable<string> ToCsv<T>(string separator, IEnumerable<T> objectlist)
        {
            PropertyInfo[] fields = typeof(T).GetProperties();
            yield return String.Join(separator, fields.Select(f => f.Name).ToArray());
            foreach (var o in objectlist)
            {
                yield return string.Join(separator, fields.Select(f => (f.GetValue(o) ?? "").ToString()).ToArray());
            }
        }

        public static IEnumerable<string> ToCsvIgnoreLists<T>(string separator, IEnumerable<T> objectlist)
        {
            FieldInfo[] allFields = typeof(T).GetFields();

            IEnumerable<System.Reflection.FieldInfo> fields = from f in allFields
                                                              where f.FieldType != typeof(Dictionary<string, int>)
                                                              select f;
            yield return String.Join(separator, fields.Select(f => f.Name).ToArray());

            foreach (var o in objectlist)
            {
                yield return string.Join(separator, fields.Select(f => (f.GetValue(o) ?? "").ToString()).ToArray());
            }
        }
    }

    public class Counter
    {
        public int count = new int();
        public void increment()
        {
            count = +1;
        }
    }

}

