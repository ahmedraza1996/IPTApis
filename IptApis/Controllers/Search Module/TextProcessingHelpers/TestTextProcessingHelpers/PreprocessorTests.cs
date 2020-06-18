using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IptApis.Controllers.Search_Module.TextProcessingHelpers.UnitTest
{
    [TestClass]
    public class PreprocessorTests
    {
        [TestMethod]
        public void test_a()
        {
            var expected = Preprocessor.GetInstance().Preprocess("a");
            Assert.AreEqual(expected, "");
        }

        [TestMethod]
        public void test_A()
        {
            var expected = Preprocessor.GetInstance().Preprocess("A");
            Assert.AreEqual(expected, "");
        }

        [TestMethod]
        public void test_are()
        {
            var expected = Preprocessor.GetInstance().Preprocess("          are    ");
            Assert.AreEqual(expected, "");
        }

        [TestMethod]
        public void test_are1()
        {
            var expected = Preprocessor.GetInstance().Preprocess("          are1,..213    ");
            Assert.AreEqual(expected, "");
        }

        [TestMethod]
        public void test_are2()
        {
            var expected = Preprocessor.GetInstance().Preprocess("          are1    ");
            Assert.AreEqual(expected, "");
        }

        [TestMethod]
        public void test_match()
        {
            var expected = Preprocessor.GetInstance().Preprocess("          matched    ");
            Assert.AreEqual(expected, "match");
        }

    }
}