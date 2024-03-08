using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlcumWeb.Araclar
{
    public class ConnectionStrings
    {
        public static string OlcumConnection => "data source=192.168.41.107;initial catalog=Olcum;persist security info=True;user id=recore;password=Recore*4512;";
        public static string DikimhaneConnection => "data source=192.168.41.107;initial catalog=Dikimhane;persist security info=True;user id=recore;password=Recore*4512;";

        //public static string OlcumConnection => "data source=176.236.9.210;initial catalog=Olcum;persist security info=True;user id=recore;password=Recore*4512;";
        //public static string DikimhaneConnection => "data source=176.236.9.210;initial catalog=Dikimhane;persist security info=True;user id=recore;password=Recore*4512;";

    }
}