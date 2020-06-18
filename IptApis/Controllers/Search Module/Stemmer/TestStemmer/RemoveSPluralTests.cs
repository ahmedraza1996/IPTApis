using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IptApis.Controllers.Search_Module.Stemmer.UnitTest
{
    [TestClass]
    public class RemoveSPluralTests
    {
        [TestMethod]
        public void RemoveSPluralSuffix_WithWordEndingInApostrophe_RemovesSuffix()
        {
            const string word = "holy'";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step0RemoveSPluralSuffix(word);

            // Assert
            Assert.AreEqual("holy", actual);
        }

        [TestMethod]
        public void RemoveSPluralSuffix_WithWordEndingInApostropheS_RemovesSuffix()
        {
            const string word = "holy's";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step0RemoveSPluralSuffix(word);

            // Assert
            Assert.AreEqual("holy", actual);
        }

        [TestMethod]
        public void RemoveSPluralSuffix_WithWordEndingInApostropheSApostrophe_RemovesSuffix()
        {
            const string word = "holy's'";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step0RemoveSPluralSuffix(word);

            // Assert
            Assert.AreEqual("holy", actual);
        }
    }
}