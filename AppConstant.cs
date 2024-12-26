using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public class AppConstant
    {

        public const String DevConnectionString = "Data Source=SQL5112.site4now.net;Initial Catalog=db_9ab8b7_324dda12247;User Id=db_9ab8b7_324dda12247_admin;Password=csF4ChaS;";
        public const String ProductionConnectionString = "";
        public const String TestConnectionString = "";

        public enum UserTable
        {

            userId,
            firstName,
            lastName,
            userlevel
        }
    }

    
}