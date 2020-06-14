using IptApis.Controllers.Search_Module.Indexes;
using IptApis.Controllers.Search_Module.TextProcessingHelpers;
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
            ProximityQueryRegex = new Regex(@"^(\w+)\s(\w+)\s*\/\s*(\d+)$");
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

        public IList<int> Parse(string _query, WordsVector wordsVector, DocumentsVector documentsVector, HashSet<int> universalSet)
        {
            var query = Preprocessor.GetInstance().PreprocessQuery(_query);
            if (ProximityQueryRegex.IsMatch(query))
            {
                return ProximityQuery.Parse(query, wordsVector);
            }
            else if (GeneralTextQueryRegex.IsMatch(query))
            {
                return GeneralTextQueryParser.Parse(query, wordsVector, documentsVector);
            }
            else if (BooleanQueryRegex.IsMatch(query))
            {
                return BooleanQueryParser.Parse(query, universalSet, wordsVector);
            }
            

            else
            {
                throw new Exception("Invalid Query");
            }
        } 
    }
}