using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IptApis.Controllers.Search_Module.QueryParser
{
    public class QueryParser
    {
        private static QueryParser Instance;
        private Regex BooleanQueryRegex;
        private Regex ProximityQueryRegex;
        private Regex GeneralTextQueryRegex;

        private QueryParser()
        {
            BooleanQueryRegex = new Regex(@"^[\w\s!&|()]+$");
            ProximityQueryRegex = new Regex(@"^(\w+)\s(\w+)\s*/\s*(\d+)$");
            GeneralTextQueryRegex = new Regex(@"^[\w\s]+$");
        }

        public static QueryParser GetInstance()
        {
            if (Instance == null)
            {
                Instance = new QueryParser();
            }
            return Instance;
        }

        public IList<string> Parse(string query)
        {
            if (ProximityQueryRegex.IsMatch(query))
            {

            }
            else if (GeneralTextQueryRegex.IsMatch(query))
            {

            }
            else if (BooleanQueryRegex.IsMatch(query))
            {

            }
            else
            {
                throw new Exception("Invalid Query");
            }
            return new List<string>();
        } 
    }
}