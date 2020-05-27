using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IptApis.Controllers.Search_Module.TextProcessingHelpers
{
    public class Tokenizer
    {
        public static IList<Token> TokenizeWord(string text)
        {
            var words = Regex.Split(text, @"[,\.;&!:\(\)\s]");
            var tokens = new List<Token>();
            int count = 0;
            foreach (var word in words)
            {
                var token = new Token(word, count);
                if (token.Word != "")
                {
                    tokens.Add(token);
                }
                count += 1;
            }

            return tokens;
        }

        public static IList<Token> TokenizeSentences(string text)
        {
            var sentences = Regex.Split(text, @"(?<=[\.!\?])\s+");
            var tokens = new List<Token>();
            int count = 0;
            foreach (var sentence in sentences)
            {
                var token = new Token(sentence, count);
                if (token.Word != "")
                {
                    tokens.Add(token);
                }
                count += 1;
            }

            return tokens;
        }
    }
}