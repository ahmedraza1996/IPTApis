using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;

namespace IptApis.Controllers.Search_Module.TextProcessingHelpers.UnitTest
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void test()
        {
            var text = "Articles in the English language are the definite article the and the indefinite articles a and an.";
            var expected = Tokenizer.TokenizeWord(text);
            var actual = new List<Token>();
            actual.Add(new Token(Preprocessor.GetInstance().Preprocess("Articles"), 0));
            actual.Add(new Token(Preprocessor.GetInstance().Preprocess("English"), 3));
            actual.Add(new Token(Preprocessor.GetInstance().Preprocess("language"), 4));
            actual.Add(new Token(Preprocessor.GetInstance().Preprocess("definite"), 7));
            actual.Add(new Token(Preprocessor.GetInstance().Preprocess("article"), 8));
            actual.Add(new Token(Preprocessor.GetInstance().Preprocess("indefinite"), 12));
            actual.Add(new Token(Preprocessor.GetInstance().Preprocess("articles"), 13));
            CollectionAssert.AreEqual((ICollection)expected, actual);
        }

    }
}