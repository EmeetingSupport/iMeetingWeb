using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace MM.Data
{
    public class ClsMain
    {
        public bool checkColumExist(string columnName, IDataReader dr)
        {

            for (int i = 0; i < dr.FieldCount; i++)
            {

                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

    }
}
