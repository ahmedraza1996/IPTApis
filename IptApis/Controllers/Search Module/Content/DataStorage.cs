using IptApis.Controllers.Search_Module.Indexes;
using IptApis.Controllers.Search_Module.SearchFYP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Controllers.Search_Module.Content
{
    public class DataStorage
    {
        private static DataStorage Instance;
        public WordsVector wordsVector;
        public DocumentsVector documentsVector;
        public IDictionary<int, FYPSearchModel> FYP_Data;

        private DataStorage()
        {
            FYP_Data = new Dictionary<int, FYPSearchModel>();
            wordsVector = new WordsVector();
            documentsVector = new DocumentsVector();
        }

        public static DataStorage GetInstance()
        {
            if (Instance == null)
            {
                Instance = new DataStorage();
            }
            return Instance;
        }

    }
}