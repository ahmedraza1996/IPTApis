﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IptApis.Controllers.Search_Module.Stemmer.UnitTest
{
    [TestClass]
    public class LyEndingsTests
    {
        [TestMethod]
        public void RemoveLySuffixes_EndingInEedlyAndInR1_ReplacesSuffixWithEe()
        {
            const string word = "inbreedly";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1BRemoveLySuffixes(word, stemmer.GetRegion1(word));

            // Assert
            Assert.AreEqual("inbree", actual);
        }

        [TestMethod]
        public void RemoveLySuffixes_EndingInEedAndInR1_ReplacesSuffixWithEe()
        {
            const string word = "inbreed";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1BRemoveLySuffixes(word, stemmer.GetRegion1(word));

            // Assert
            Assert.AreEqual("inbree", actual);
        }

        [TestMethod]
        public void RemoveLySuffixes_EndingInEdAndDoesNotContainVowel_LeavesWord()
        {
            const string word = "fred";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1BRemoveLySuffixes(word, stemmer.GetRegion1(word));

            // Assert
            Assert.AreEqual("fred", actual);
        }

        [TestMethod]
        public void RemoveLySuffixes_EndingInEdAndAtProceedsThat_ReplacesSuffixWithE()
        {
            const string word = "luxuriated";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1BRemoveLySuffixes(word, stemmer.GetRegion1(word));

            // Assert
            Assert.AreEqual("luxuriate", actual);
        }

        [TestMethod]
        public void RemoveLySuffixes_EndingInEdlyAndAtProceedsThat_ReplacesSuffixWithE()
        {
            const string word = "luxuriatedly";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1BRemoveLySuffixes(word, stemmer.GetRegion1(word));

            // Assert
            Assert.AreEqual("luxuriate", actual);
        }

        [TestMethod]
        public void RemoveLySuffixes_EndingInIngAndAtProceedsThat_ReplacesSuffixWithE()
        {
            const string word = "luxuriating";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1BRemoveLySuffixes(word, stemmer.GetRegion1(word));

            // Assert
            Assert.AreEqual("luxuriate", actual);
        }

        [TestMethod]
        public void RemoveLySuffixes_EndingInInglyAndAtProceedsThat_ReplacesSuffixWithE()
        {
            const string word = "luxuriated";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1BRemoveLySuffixes(word, stemmer.GetRegion1(word));

            // Assert
            Assert.AreEqual("luxuriate", actual);
        }

        [TestMethod]
        public void RemoveLySuffixes_EndingInIngAndDoubledLetterProceedsThat_RemovesDoubledLetter()
        {
            const string word = "hopping";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1BRemoveLySuffixes(word, stemmer.GetRegion1(word));

            // Assert
            Assert.AreEqual("hop", actual);
        }

        [TestMethod]
        public void RemoveLySuffixes_EndingInIngAndIsShortWord_ReplacesSuffixWithE()
        {
            const string word = "hoping";
            var stemmer = new SnowballPorterStemmer();

            // Act
            var actual = stemmer.Step1BRemoveLySuffixes(word, stemmer.GetRegion1(word));

            // Assert
            Assert.AreEqual("hope", actual);
        }
    }
}