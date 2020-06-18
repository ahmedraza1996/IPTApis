using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Controllers.Search_Module.Stemmer
{
    public partial class SnowballPorterStemmer : IPorterStemmer
    {
        public char[] Alphabet { get; } = "abcdefghijklmnopqrstuvwxyz\'".ToArray();

        private readonly char[] NonShortConsonants = "wxY".ToArray();

        private readonly Dictionary<string, string> Exceptions = new Dictionary<string, string>
            {
                {"skis", "ski"},
                {"skies", "sky"},
                {"dying", "die"},
                {"lying", "lie"},
                {"tying", "tie"},
                {"idly", "idl"},
                {"gently", "gentl"},
                {"ugly", "ugli"},
                {"early", "earli"},
                {"only", "onli"},
                {"singly", "singl"},
                {"sky", "sky"},
                {"news", "news"},
                {"howe", "howe"},
                {"atlas", "atlas"},
                {"cosmos", "cosmos"},
                {"bias", "bias"},
                {"andes", "andes"}
            };

        private readonly string[] ExceptionsRegion2 = new[]
            {
                "inning", "outing", "canning", "herring", "earring",
                "proceed", "exceed", "succeed"
            };

        private readonly string[] ExceptionsRegion1 = new[]
            {
                "gener", "arsen", "commun"
            };

        private bool IsVowel(char c)
        {
            return Vowels.Contains(c);
        }

        private bool IsConsonant(char c)
        {
            return !Vowels.Contains(c);
        }

        private bool SuffixInRegion1(string word, int Region1, string suffix)
        {
            return Region1 <= word.Length - suffix.Length;
        }

        private bool SuffixInRegion2(string word, int Region2, string suffix)
        {
            return Region2 <= word.Length - suffix.Length;
        }

        private string ReplaceSuffix(string word, string oldSuffix, string newSuffix = null)
        {
            if (oldSuffix != null)
            {
                word = word.Substring(0, word.Length - oldSuffix.Length);
            }

            if (newSuffix != null)
            {
                word += newSuffix;
            }
            return word;
        }

        private bool TryReplace(string word, string oldSuffix, string newSuffix, out string final)
        {
            if (word.Contains(oldSuffix))
            {
                final = ReplaceSuffix(word, oldSuffix, newSuffix);
                return true;
            }
            final = word;
            return false;
        }

        /// <summary>
        /// The English stemmer treats apostrophe as a letter, removing it from the beginning of a word, where it might have stood for an opening quote, from the end of the word, where it might have stood for a closing quote, or been an apostrophe following s.
        /// </summary>
        /// <param name="word">string</param>
        /// <returns>string</returns>
        private string TrimStartingApostrophe(string word)
        {
            if (word.StartsWith("'"))
            {
                word = word.Substring(1);
            }
            return word;
        }

        private int GetRegion(string word, int begin)
        {
            var foundVowel = false;
            for (var i = begin; i < word.Length; i++)
            {
                if (IsVowel(word[i]))
                {
                    foundVowel = true;
                    continue;
                }
                if (foundVowel && IsConsonant(word[i]))
                {
                    return i + 1;
                }
            }

            return word.Length;
        }

        /// <summary>
        /// Define a short syllable in a word as either (a) a vowel followed 
        /// by a non-vowel other than w, x or Y and preceded by a non-vowel, 
        /// or * (b) a vowel at the beginning of the word followed by a non-vowel. 
        /// </summary>
        /// <param name="word">string</param>
        /// <returns>bool</returns>
        public bool EndsInShortSyllable(string word)
        {
            if (word.Length < 2)
            {
                return false;
            }

            // a vowel at the beginning of the word followed by a non-vowel
            if (word.Length == 2)
            {
                return IsVowel(word[0]) && IsConsonant(word[1]);
            }

            return IsVowel(word[word.Length - 2])
                   && IsConsonant(word[word.Length - 1])
                   && !NonShortConsonants.Contains(word[word.Length - 1])
                   && IsConsonant(word[word.Length - 3]);
        }

        /// <summary>
        /// A word is called short if it ends in a short syllable, and if Region1 is null.
        /// </summary>
        /// <param name="word"></param>
        /// <returns>bool</returns>
        public bool IsShortWord(string word)
        {
            return EndsInShortSyllable(word) && GetRegion1(word) == word.Length;
        }

        /// <summary>
        /// Set initial y, or y after a vowel, to Y
        /// </summary>
        /// <param name="word">string</param>
        /// <returns>string</returns>
        public string MarkYsAsConsonants(string word)
        {
            var chars = word.ToCharArray();
            for (var i = 0; i < chars.Length; i++)
            {
                if (i == 0)
                {
                    if (chars[i] == 'y')
                    {
                        chars[i] = 'Y';
                    }
                }
                else if (Vowels.Contains(chars[i - 1]) && chars[i] == 'y')
                {
                    chars[i] = 'Y';
                }
            }
            return new string(chars);
        }
    }

}