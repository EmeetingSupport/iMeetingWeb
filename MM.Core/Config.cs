﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;


namespace MM.Core
{
    public class Config
    {/// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the default connection String as specified in the provider.
        /// </summary>
        /// <returns>The connection String</returns>
        /// <remarks></remarks>
        /// -----------------------------------------------------------------------------
        public static string GetConnectionString()
        {
            return GetConnectionString("");
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the specified connection String
        /// </summary>
        /// <param name="name">Name of Connection String to return</param>
        /// <returns>The connection String</returns>
        /// <remarks></remarks>
        public static string GetConnectionString(string name)
        {
            string connectionString = "";
            
            //First check if connection string is specified in <connectionstrings> (ASP.NET 2.0 / DNN v4.x)
            if (!String.IsNullOrEmpty(name))
            {
                //ASP.NET 2 version connection string (in <connectionstrings>)
                //This will be for new v4.x installs or upgrades from v4.x
                connectionString = AesEncryption.DecryptString(WebConfigurationManager.ConnectionStrings[name].ConnectionString);
            }

            if (String.IsNullOrEmpty(connectionString))
            {
                if (!String.IsNullOrEmpty(name))
                {
                    //Next check if connection string is specified in <appsettings> (ASP.NET 1.1 / DNN v3.x)
                    //This will accomodate upgrades from v3.x
                    connectionString = GetSetting(name);
                }
            }
            return connectionString;
        }
        

        public static string GetSetting(string setting)
        {
            return WebConfigurationManager.AppSettings[setting];
        }

        public static object GetSection(string section)
        {
            return WebConfigurationManager.GetWebApplicationSection(section);
        }
    }
}
