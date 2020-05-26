using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Controllers.Search_Module.Stemmer
{
    public partial class SnowballPorterStemmer : IPorterStemmer
    {
        public string Step0RemoveSPluralSuffix(string word)
        {
            // Ordered from longest to shortest
            var suffixes = new[] { "'s'", "'s", "'" };
            foreach (var suffix in suffixes)
            {
                if (word.EndsWith(suffix))
                {
                    return ReplaceSuffix(word, suffix);
                }
            }
            return word;
        }

        public string Step1ARemoveOtherSPluralSuffixes(string word)
        {
            if (word.EndsWith("sses"))
            {
                return ReplaceSuffix(word, "sses", "ss");
            }
            if (word.EndsWith("ied") || word.EndsWith("ies"))
            {
                var restOfWord = word.Substring(0, word.Length - 3);
                if (word.Length > 4)
                {
                    return restOfWord + "i";
                }
                return restOfWord + "ie";
            }
            if (word.EndsWith("us") || word.EndsWith("ss"))
            {
                return word;
            }
            if (word.EndsWith("s"))
            {
                if (word.Length < 3)
                {
                    return word;
                }

                // Skip both the last letter ('s') and the letter before that
                for (var i = 0; i < word.Length - 2; i++)
                {
                    if (IsVowel(word[i]))
                    {
                        return word.Substring(0, word.Length - 1);
                    }
                }
            }
            return word;
        }

        public string Step1BRemoveLySuffixes(string word, int r1)
        {
            foreach (var suffix in new[] { "eedly", "eed" }.Where(word.EndsWith))
            {
                if (SuffixInRegion1(word, r1, suffix))
                {
                    return ReplaceSuffix(word, suffix, "ee");
                }
                return word;
            }

            foreach (var suffix in new[] { "ed", "edly", "ing", "ingly" }.Where(word.EndsWith))
            {
                var trunc = ReplaceSuffix(word, suffix);//word.Substring(0, word.Length - suffix.Length);
                if (trunc.Any(IsVowel))
                {
                    if (new[] { "at", "bl", "iz" }.Any(trunc.EndsWith))
                    {
                        return trunc + "e";
                    }
                    if (Doubles.Any(trunc.EndsWith))
                    {
                        return trunc.Substring(0, trunc.Length - 1);
                    }
                    if (IsShortWord(trunc))
                    {
                        return trunc + "e";
                    }
                    return trunc;
                }
                return word;
            }

            return word;
        }

        public string Step1CReplaceSuffixYWithIIfPreceededWithConsonant(string word)
        {
            if ((word.EndsWith("y") || word.EndsWith("Y"))
                && word.Length > 2
                && IsConsonant(word[word.Length - 2]))
            {
                return word.Substring(0, word.Length - 1) + "i";
            }
            return word;
        }

        public string Step2ReplaceSuffixes(string word, int r1)
        {
            var suffixes = new Dictionary<string, string>
                {
                    {"ization", "ize"},
                    {"ational", "ate"},
                    {"ousness", "ous"},
                    {"iveness", "ive"},
                    {"fulness", "ful"},
                    {"tional", "tion"},
                    {"lessli", "less"},
                    {"biliti", "ble"},
                    {"entli", "ent"},
                    {"ation", "ate"},
                    {"alism", "al"},
                    {"aliti", "al"},
                    {"fulli", "ful"},
                    {"ousli", "ous"},
                    {"iviti", "ive"},
                    {"enci", "ence"},
                    {"anci", "ance"},
                    {"abli", "able"},
                    {"izer", "ize"},
                    {"ator", "ate"},
                    {"alli", "al"},
                    {"bli", "ble"}
                };
            foreach (var suffix in suffixes)
            {
                if (word.EndsWith(suffix.Key))
                {
                    string final;
                    if (SuffixInRegion1(word, r1, suffix.Key)
                        && TryReplace(word, suffix.Key, suffix.Value, out final))
                    {
                        return final;
                    }
                    return word;
                }
            }

            if (word.EndsWith("ogi")
                && SuffixInRegion1(word, r1, "ogi")
                && word[word.Length - 4] == 'l')
            {
                return ReplaceSuffix(word, "ogi", "og");
            }

            if (word.EndsWith("li") & SuffixInRegion1(word, r1, "li"))
            {
                if (LiEndings.Contains(word[word.Length - 3]))
                {
                    return ReplaceSuffix(word, "li");
                }
            }

            return word;
        }

        public string Step3ReplaceSuffixes(string word, int r1, int r2)
        {
            var suffixes = new Dictionary<string, string>
                {
                    {"ational", "ate"},
                    {"tional", "tion"},
                    {"alize", "al"},
                    {"icate", "ic"},
                    {"iciti", "ic"},
                    {"ical", "ic"},
                    {"ful", null},
                    {"ness", null}
                };
            foreach (var suffix in suffixes.Where(s => word.EndsWith(s.Key)))
            {
                string final;
                if (SuffixInRegion1(word, r1, suffix.Key)
                    && TryReplace(word, suffix.Key, suffix.Value, out final))
                {
                    return final;
                }
            }

            if (word.EndsWith("ative"))
            {
                if (SuffixInRegion1(word, r1, "ative") && SuffixInRegion2(word, r2, "ative"))
                {
                    return ReplaceSuffix(word, "ative", null);
                }
            }

            return word;
        }

        public string Step4RemoveSomeSuffixesInR2(string word, int r2)
        {
            foreach (var suffix in new[]
                {
                    "al", "ance", "ence", "er", "ic", "able", "ible", "ant",
                    "ement", "ment", "ent", "ism", "ate", "iti", "ous",
                    "ive", "ize"
                })
            {
                if (word.EndsWith(suffix))
                {
                    if (SuffixInRegion2(word, r2, suffix))
                    {
                        return ReplaceSuffix(word, suffix);
                    }
                    return word;
                }
            }

            if (word.EndsWith("ion") &&
                SuffixInRegion2(word, r2, "ion") &&
                new[] { 's', 't' }.Contains(word[word.Length - 4]))
            {
                return ReplaceSuffix(word, "ion");
            }
            return word;
        }

        public string Step5RemoveEorLSuffixes(string word, int r1, int r2)
        {
            if (word.EndsWith("e") &&
                (SuffixInRegion2(word, r2, "e") ||
                    (SuffixInRegion1(word, r1, "e") &&
                        !EndsInShortSyllable(ReplaceSuffix(word, "e")))))
            {
                return ReplaceSuffix(word, "e");
            }

            if (word.EndsWith("l") &&
                SuffixInRegion2(word, r2, "l") &&
                word.Length > 1 &&
                word[word.Length - 2] == 'l')
            {
                return ReplaceSuffix(word, "l");
            }

            return word;
        }

    }
}
