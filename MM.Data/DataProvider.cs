﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Core;

namespace MM.Data
{
 public class DataProvider
 {
     #region "ConnectionString"
     public string ConnectionString
     {
         get
         {
             return MM.Core.Config.GetConnectionString("ConnectionString");
         }
     }
     #endregion
 }
}
