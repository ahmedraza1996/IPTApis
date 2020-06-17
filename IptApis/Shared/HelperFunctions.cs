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


        public static bool dateVerfication(int start_date,int end_date)
        {
            return start_date < end_date;
        }
    }
}