using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Controllers.Search_Module.Stemmer
{
    public struct StemmedWord
    {
        public readonly string Stemmed;

        public readonly string Unstemmed;

        public StemmedWord(string value, string unstemmed)
        {
            Stemmed = value;
            Unstemmed = unstemmed;
        }

    }
}