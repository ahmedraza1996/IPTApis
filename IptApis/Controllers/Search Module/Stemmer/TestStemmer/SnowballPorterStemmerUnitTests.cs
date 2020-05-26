using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IptApis.Controllers.Search_Module.Stemmer.UnitTest
{
    [TestClass]
    public class EnglishPorter2StemmerUnitTest
    {
        [TestMethod]
        public void Stem_WithBatchData_StemsAllWordsCorrectly()
        {

            var stemmer = new SnowballPorterStemmer();

            using (var reader = new StreamReader(@"../Controllers/Search Module/Stemmer/TestStemmer/data.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    string expected = values[1];
                    var stemmed = stemmer.Stem(values[0]).Stemmed;
                    Assert.AreEqual(expected, stemmed);
                }
            }

        }
    }
}