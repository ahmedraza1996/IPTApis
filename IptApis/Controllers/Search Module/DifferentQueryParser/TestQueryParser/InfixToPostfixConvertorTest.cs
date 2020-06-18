using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IptApis.Controllers.Search_Module.QueryParser.TestQueryParser
{
    [TestClass]
    public class InfixToPostfixConvertorTest
    {
        [TestMethod]
        public void test_infix_to_postfix()
        {
            var query = "stem&say|stem&(comput|say)";
            string[] actual = { "stem", "say", "&", "stem", "comput", "say", "|", "&", "|" };
            var expected = InfixToPostfixConvertor.Convert(query).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void test_infix_to_postfix1()
        {
            var query = "stem&say|stem&(comput|say&stem)";
            string[] actual = { "stem", "say", "&", "stem", "comput", "say", "stem", "&", "|", "&", "|" };
            var expected = InfixToPostfixConvertor.Convert(query).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void test_infix_to_postfix2()
        {
            var query = "(stem|say)&comput";
            string[] actual = { "stem", "say", "|", "comput", "&" };
            var expected = InfixToPostfixConvertor.Convert(query).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void test_infix_to_postfix3()
        {
            var query = "((stem|act)&say)&stem|comput";
            string[] actual = { "stem", "act", "|", "say", "&", "stem", "&", "comput", "|" };
            var expected = InfixToPostfixConvertor.Convert(query).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void test_infix_to_postfix4()
        {
            var query = "((stem|act)&say)&(stem|comput)";
            string[] actual = { "stem", "act", "|", "say", "&", "stem", "comput", "|", "&" };
            var expected = InfixToPostfixConvertor.Convert(query).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void test_infix_to_postfix5()
        {
            var query = "((((act&(say|stem))|stem)|love)&comput)";
            string[] actual = { "act", "say", "stem", "|", "&", "stem", "|", "love", "|", "comput", "&" };
            var expected = InfixToPostfixConvertor.Convert(query).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void test_infix_to_postfix6()
        {
            var query = "! stem & ! say";
            string[] actual = { "stem", "!", "say", "!", "&" };
            var expected = InfixToPostfixConvertor.Convert(query).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void test_infix_to_postfix7()
        {
            var query = "(!(stem) & !(say))";
            string[] actual = { "stem", "!", "say", "!", "&" };
            var expected = InfixToPostfixConvertor.Convert(query).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void test_infix_to_postfix8()
        {
            var query = "(!(!stem)) & ! say";
            string[] actual = { "stem", "!", "!", "say", "!", "&" };
            var expected = InfixToPostfixConvertor.Convert(query).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void test_infix_to_postfix9()
        {
            var query = "(!!stem) & ! say";
            string[] actual = { "stem", "!", "!", "say", "!", "&" };
            var expected = InfixToPostfixConvertor.Convert(query).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void test_infix_to_postfix10()
        {
            var query = "! stem | ! say";
            string[] actual = { "stem", "!", "say", "!", "|" };
            var expected = InfixToPostfixConvertor.Convert(query).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void test_infix_to_postfix11()
        {
            var query = "(!(stem) | !(say))";
            string[] actual = { "stem", "!", "say", "!", "|" };
            var expected = InfixToPostfixConvertor.Convert(query).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void test_infix_to_postfix12()
        {
            var query = "!(!stem)";
            string[] actual = { "stem", "!", "!" };
            var expected = InfixToPostfixConvertor.Convert(query).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

    }
}