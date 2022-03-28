using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using System.Configuration;

namespace MM.Core
{
    public static class Extensions
    {
        public static DataTable AsDataTable<T>(this IEnumerable<T> enumberable)
        {
            DataTable dtReturn = new DataTable();

            PropertyInfo[] oProps = null;

            if (enumberable == null)
            {
                return dtReturn;
            }
            foreach (T rec in enumberable)
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if (((colType.IsGenericType) && (object.ReferenceEquals(colType.GetGenericTypeDefinition(), typeof(Nullable<>)))))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    //System.reflectiopn for PropertyInfo.
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
    }
}
