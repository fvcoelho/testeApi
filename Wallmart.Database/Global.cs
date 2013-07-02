using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Global
{
    public static string ConnectionString
    {
        get
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings[System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]].ConnectionString;
            }
            catch
            {
                return "";
            }
        }
    }
}
