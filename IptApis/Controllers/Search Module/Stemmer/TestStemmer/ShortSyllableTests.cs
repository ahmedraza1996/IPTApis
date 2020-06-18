using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IptApis.Controllers.Search_Module.Stemmer.UnitTest
{
    [TestClass]
    public class ShortSyllableTests
    {
        [TestMethod]
        public void EndInShortSyllable_TestingRap_IsCountedAsShort()
        {
            // Arrange
            const string word = "rap";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.EndsInShortSyllable(word);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void EndInShortSyllable_TestingTrap_IsCountedAsShort()
        {
            // Arrange
            const string word = "trap";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.EndsInShortSyllable(word);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void EndInShortSyllable_TestingEntrap_IsCountedAsShort()
        {
            // Arrange
            const string word = "entrap";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.EndsInShortSyllable(word);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void EndInShortSyllable_TestingOw_IsCountedAsShort()
        {
            // Arrange
            const string word = "ow";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.EndsInShortSyllable(word);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void EndInShortSyllable_TestingOn_IsCountedAsShort()
        {
            // Arrange
            const string word = "on";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.EndsInShortSyllable(word);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void EndInShortSyllable_TestingAt_IsCountedAsShort()
        {
            // Arrange
            const string word = "at";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.EndsInShortSyllable(word);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void EndInShortSyllable_TestingUproot_IsNotCountedAsShort()
        {
            // Arrange
            const string word = "uproot";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.EndsInShortSyllable(word);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void EndInShortSyllable_TestingBestow_IsCountedAsShort()
        {
            // Arrange
            const string word = "bestow";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.EndsInShortSyllable(word);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void EndInShortSyllable_TestingDisturb_IsCountedAsShort()
        {
            // Arrange
            const string word = "disturb";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.EndsInShortSyllable(word);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsShortWord_TestingBed_IsCountedAsShort()
        {
            // Arrange
            const string word = "bed";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.IsShortWord(word);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsShortWord_TestingShed_IsCountedAsShort()
        {
            // Arrange
            const string word = "shed";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.IsShortWord(word);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsShortWord_TestingShred_IsCountedAsShort()
        {
            // Arrange
            const string word = "shred";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.IsShortWord(word);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsShortWord_TestingBead_IsNotCountedAsShort()
        {
            // Arrange
            const string word = "bead";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.IsShortWord(word);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsShortWord_TestingEmbed_IsNotCountedAsShort()
        {
            // Arrange
            const string word = "embed";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.IsShortWord(word);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsShortWord_TestingBeds_IsNotCountedAsShort()
        {
            // Arrange
            const string word = "beds";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.IsShortWord(word);

            // Assert
            Assert.IsFalse(actual);
        }
    }
}