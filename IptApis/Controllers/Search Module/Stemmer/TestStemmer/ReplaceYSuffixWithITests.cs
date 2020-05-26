using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IptApis.Controllers.Search_Module.Stemmer.UnitTest
{
    [TestClass]
    public class ReplaceYSuffixWithITests
    {
        [TestMethod]
        public void ReplaceYSuffix_PreceededByConsonant_ReplacesSuffixWithI()
        {
            const string word = "cry";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1CReplaceSuffixYWithIIfPreceededWithConsonant(word);

            // Assert
            Assert.AreEqual("cri", actual);
        }

        [TestMethod]
        public void ReplaceYSuffix_PreceededByConsonantAsFirstLetterOfWord_DoesNotReplaceSuffix()
        {
            const string word = "by";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1CReplaceSuffixYWithIIfPreceededWithConsonant(word);

            // Assert
            Assert.AreEqual("by", actual);
        }

        [TestMethod]
        public void ReplaceYSuffix_NotPreceededyConsonant_DoesNotReplaceSuffix()
        {
            const string word = "say";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1CReplaceSuffixYWithIIfPreceededWithConsonant(word);

            // Assert
            Assert.AreEqual("say", actual);
        }
    }
}