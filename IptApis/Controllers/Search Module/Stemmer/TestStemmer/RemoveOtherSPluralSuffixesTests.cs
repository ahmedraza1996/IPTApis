using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IptApis.Controllers.Search_Module.Stemmer.UnitTest
{
    [TestClass]
    public class RemoveOtherSPluralSuffixesTests
    {
        [TestMethod]
        public void RemoveOtherSPluralSuffix_WithWordEndingInSses_ReplaceWithSs()
        {
            const string word = "assesses";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1ARemoveOtherSPluralSuffixes(word);

            // Assert
            Assert.AreEqual("assess", actual);
        }

        [TestMethod]
        public void RemoveOtherSPluralSuffix_WithLongWordEndingInIes_ReplaceWithI()
        {
            const string word = "cries";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1ARemoveOtherSPluralSuffixes(word);

            // Assert
            Assert.AreEqual("cri", actual);
        }

        [TestMethod]
        public void RemoveOtherSPluralSuffix_WithShortWordEndingInIes_ReplaceWithIe()
        {
            const string word = "ties";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1ARemoveOtherSPluralSuffixes(word);

            // Assert
            Assert.AreEqual("tie", actual);
        }

        [TestMethod]
        public void RemoveOtherSPluralSuffix_WithLongWordEndingInIed_ReplaceWithI()
        {
            const string word = "cried";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1ARemoveOtherSPluralSuffixes(word);

            // Assert
            Assert.AreEqual("cri", actual);
        }

        [TestMethod]
        public void RemoveOtherSPluralSuffix_WithShortWordEndingInIed_ReplaceWithIe()
        {
            const string word = "tied";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1ARemoveOtherSPluralSuffixes(word);

            // Assert
            Assert.AreEqual("tie", actual);
        }

        [TestMethod]
        public void RemoveOtherSPluralSuffix_EndingInSAndContainingAVowelRightBefore_LeavesTheS()
        {
            const string word = "gas";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1ARemoveOtherSPluralSuffixes(word);

            // Assert
            Assert.AreEqual("gas", actual);
        }

        [TestMethod]
        public void RemoveOtherSPluralSuffix_EndingInSAndContainingAVowelEarlierInWord_DeletesTheS()
        {
            const string word = "gaps";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1ARemoveOtherSPluralSuffixes(word);

            // Assert
            Assert.AreEqual("gap", actual);
        }

        [TestMethod]
        public void RemoveOtherSPluralSuffix_EndingInSAndContainingAVowelRightBeforeAndEarlierInWord_DeletesTheS()
        {
            const string word = "kiwis";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1ARemoveOtherSPluralSuffixes(word);

            // Assert
            Assert.AreEqual("kiwi", actual);
        }

        [TestMethod]
        public void RemoveOtherSPluralSuffix_EndingInSs_LeavesWordAlone()
        {
            const string word = "assess";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1ARemoveOtherSPluralSuffixes(word);

            // Assert
            Assert.AreEqual("assess", actual);
        }

        [TestMethod]
        public void RemoveOtherSPluralSuffix_EndingInUs_LeavesWordAlone()
        {
            const string word = "consensus";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1ARemoveOtherSPluralSuffixes(word);

            // Assert
            Assert.AreEqual("consensus", actual);
        }
    }
}