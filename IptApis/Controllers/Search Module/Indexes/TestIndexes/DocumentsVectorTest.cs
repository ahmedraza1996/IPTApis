using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace IptApis.Controllers.Search_Module.Indexes.UnitTest
{
    [TestClass]
    public class DocumentVectorTests
    {
        [TestMethod]
        public void test()
        {
            string[] sentences = {"Articles Articles the English.",
                "Articles in the Arabic language.",
                "I love Playing cricket."};
            var expected = new DocumentsVector();
            for (int i=0; i<3; i++)
            {
                var vector = new Vector(sentences[i]);
                expected.Update(i, vector);
            }

            Dictionary<int, Vector> DV_dictionary = new Dictionary<int, Vector>();

            DV_dictionary.Add(0, new Vector("Articles Articles the English."));
            DV_dictionary.Add(1, new Vector("Articles in the Arabic language."));
            DV_dictionary.Add(2, new Vector("I love Playing cricket."));

            var actual = new DocumentsVector(DV_dictionary);

            Assert.AreEqual(expected, actual);

        }
    }
}