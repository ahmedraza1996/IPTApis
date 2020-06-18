using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using IptApis.Controllers.Search_Module.TextProcessingHelpers;

namespace IptApis.Controllers.Search_Module.Indexes.UnitTest
{
    [TestClass]
    public class WordsVectorTests
    {
        [TestMethod]
        public void test()
        {
            string[] sentences = {"Articles Articles the English.",
                "Articles in the Arabic language.",
                "I love Playing cricket."};
            var expected = new WordsVector();
            for (int i = 0; i < 3; i++)
            {
                var vector = new Vector(sentences[i]);
                expected.Update(i, vector);
            }

            Dictionary<string, WordDocuments> WV_dictionary = new Dictionary<string, WordDocuments>();

            var index = new Index();
            index.Word = Preprocessor.GetInstance().Preprocess("Articles");
            index.AddOccurrence(0); index.AddOccurrence(1);
            var wordDocument = new WordDocuments(0, index);
            index = new Index();
            index.Word = Preprocessor.GetInstance().Preprocess("Articles");
            index.AddOccurrence(0);
            wordDocument.Update(1, index);
            WV_dictionary.Add(Preprocessor.GetInstance().Preprocess("Articles"), wordDocument);

            index = new Index();
            index.Word = Preprocessor.GetInstance().Preprocess("English");
            index.AddOccurrence(3);
            WV_dictionary.Add(Preprocessor.GetInstance().Preprocess("English"), new WordDocuments(0, index));

            index = new Index();
            index.Word = Preprocessor.GetInstance().Preprocess("Arabic");
            index.AddOccurrence(3);
            WV_dictionary.Add(Preprocessor.GetInstance().Preprocess("Arabic"), new WordDocuments(1, index));

            index = new Index();
            index.Word = Preprocessor.GetInstance().Preprocess("language");
            index.AddOccurrence(4);
            WV_dictionary.Add(Preprocessor.GetInstance().Preprocess("language"), new WordDocuments(1, index));

            index = new Index();
            index.Word = Preprocessor.GetInstance().Preprocess("love");
            index.AddOccurrence(1);
            WV_dictionary.Add(Preprocessor.GetInstance().Preprocess("love"), new WordDocuments(2, index));

            index = new Index();
            index.Word = Preprocessor.GetInstance().Preprocess("Playing");
            index.AddOccurrence(2);
            WV_dictionary.Add(Preprocessor.GetInstance().Preprocess("Playing"), new WordDocuments(2, index));

            index = new Index();
            index.Word = Preprocessor.GetInstance().Preprocess("cricket");
            index.AddOccurrence(3);
            WV_dictionary.Add(Preprocessor.GetInstance().Preprocess("cricket"), new WordDocuments(2, index));

            var actual = new WordsVector(WV_dictionary);

            Assert.AreEqual(expected, actual);

        }
    }
}