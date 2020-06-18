using Microsoft.VisualStudio.TestTools.UnitTesting;
using IptApis.Controllers.Search_Module.TextProcessingHelpers;
using System.Collections.Generic;
using System;

namespace IptApis.Controllers.Search_Module.Indexes.UnitTest
{
    [TestClass]
    public class VectorTests
    {
        [TestMethod]
        public void test()
        {
            var text = "Articles in the English language are the definite article the and the indefinite articles a and an.";

            var expected = new Vector(text);
            Dictionary<string, Index> dictionary = new Dictionary<string, Index>();
            
            var index = new Index();
            index.Word = Preprocessor.GetInstance().Preprocess("Articles");
            index.AddOccurrence(0); index.AddOccurrence(8); index.AddOccurrence(13);
            index.Count = 3;
            dictionary.Add(Preprocessor.GetInstance().Preprocess("Articles"), index);

            index = new Index();
            index.Word = Preprocessor.GetInstance().Preprocess("English");
            index.AddOccurrence(3);
            index.Count = 1;
            dictionary.Add(Preprocessor.GetInstance().Preprocess("English"), index);

            index = new Index();
            index.Word = Preprocessor.GetInstance().Preprocess("language");
            index.AddOccurrence(4);
            index.Count = 1;
            dictionary.Add(Preprocessor.GetInstance().Preprocess("language"), index);

            index = new Index();
            index.Word = Preprocessor.GetInstance().Preprocess("definite");
            index.AddOccurrence(7);
            index.Count = 1;
            dictionary.Add(Preprocessor.GetInstance().Preprocess("definite"), index);

            index = new Index();
            index.Word = Preprocessor.GetInstance().Preprocess("indefinite");
            index.AddOccurrence(12);
            index.Count = 1;
            dictionary.Add(Preprocessor.GetInstance().Preprocess("indefinite"), index);

            var actual = new Vector(dictionary);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void test1()
        {
            var text = "Articles Articles the English.";

            var expected = new Vector(text);
            Dictionary<string, Index> dictionary = new Dictionary<string, Index>();

            var index = new Index();
            index.Word = Preprocessor.GetInstance().Preprocess("Articles");
            index.AddOccurrence(0); index.AddOccurrence(1);
            dictionary.Add(Preprocessor.GetInstance().Preprocess("Articles"), index);

            index = new Index();
            index.Word = Preprocessor.GetInstance().Preprocess("English");
            index.AddOccurrence(3);
            dictionary.Add(Preprocessor.GetInstance().Preprocess("English"), index);

            var actual = new Vector(dictionary);

            Assert.AreEqual(expected, actual);
        }

    }
}
