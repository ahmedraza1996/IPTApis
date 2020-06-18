using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IptApis.Controllers.Search_Module.Stemmer.UnitTest
{
    [TestClass]
    public class RegionTests
    {
        [TestMethod]
        public void GetRegion1_WithWordContainingRegion1AndRegion2_ProvidesCorrectRangeForRegion1()
        {
            // Arrange
            const string word = "beautiful";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.GetRegion1(word);

            // Assert
            Assert.AreEqual(5, actual);
        }

        [TestMethod]
        public void GetRegion2_WithWordContainingRegion1AndRegion2_ProvidesCorrectRangeForRegion2()
        {
            // Arrange
            const string word = "beautiful";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.GetRegion2(word);

            // Assert
            Assert.AreEqual(7, actual);
        }

        [TestMethod]
        public void GetRegion1_WithWordContainingOnlyRegion1_ProvidesCorrectRangeForRegion1()
        {
            // Arrange
            const string word = "beauty";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.GetRegion1(word);

            // Assert
            Assert.AreEqual(5, actual);
        }

        [TestMethod]
        public void GetRegion2_WithWordContainingOnlyRegion1_ProvidesRangeWithLength0()
        {
            // Arrange
            const string word = "beauty";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.GetRegion2(word);

            // Assert
            Assert.AreEqual(0, actual - word.Length);
        }

        [TestMethod]
        public void GetRegion1_WithWordContainingNeitherRegion_ProvidesRangeWithLength0()
        {
            // Arrange
            const string word = "beau";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.GetRegion1(word);

            // Assert
            Assert.AreEqual(0, actual - word.Length);
        }

        [TestMethod]
        public void GetRegion2_WithWordContainingNeitherRegion_ProvidesRangeWithLength0()
        {
            // Arrange
            const string word = "beau";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.GetRegion2(word);

            // Assert
            Assert.AreEqual(0, actual - word.Length);
        }
    }
}