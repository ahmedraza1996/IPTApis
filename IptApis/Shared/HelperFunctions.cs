using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace IptApis.Shared
{
    public class HelperFunctions
    {

        public static bool checkDateFormat(string date)
        {
            Regex regex = new Regex(@"^\d+$");
            return regex.IsMatch(date);
        }


        public static bool dateVerfication(string start_date,string end_date)
        {
            return int.Parse(start_date) < int.Parse(end_date);
        }
    }
}