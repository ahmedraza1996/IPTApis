using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Controllers.Search_Module.Stemmer
{
    public partial class SnowballPorterStemmer : IPorterStemmer
    {
        /// <summary>
        /// Based off of the improved Porter2 algorithm:
        /// http://snowball.tartarus.org/algorithms/english/stemmer.html
        /// Template Credits to:
        /// https://github.com/nemec/porter2-stemmer
        /// </summary>

        public char[] Vowels { get; } = "aeiouy".ToArray();

        public string[] Doubles { get; } = { "bb", "dd", "ff", "gg", "mm", "nn", "pp", "rr", "tt" };

        public char[] LiEndings { get; } = "cdeghkmnrt".ToArray();

        public int GetRegion1(string word)
        {
            // Exceptional forms
            foreach (var except in ExceptionsRegion1.Where(word.StartsWith))
            {
                return except.Length;
            }
            return GetRegion(word, 0);
        }

        /// <summary>
        /// Region2 is the region after the first non-vowel following a vowel in Region1, or the end of the word if there is no such non-vowel. 
        /// </summary>
        /// <param name="word">string</param>
        /// <returns>int</returns>
        public int GetRegion2(string word)
        {
            var Region1 = GetRegion1(word);
            return GetRegion(word, Region1);
        }


        public StemmedWord Stem(string word)
        {
            var original = word;
            if (word.Length <= 2)
            {
                return new StemmedWord(word, word);
            }

            word = TrimStartingApostrophe(word.ToLowerInvariant());

            string except;
            if (Exceptions.TryGetValue(word, out except))
            {
                return new StemmedWord(except, original);
            }

            word = MarkYsAsConsonants(word);

            var r1 = GetRegion1(word);
            var r2 = GetRegion2(word);

            word = Step0RemoveSPluralSuffix(word);
            word = Step1ARemoveOtherSPluralSuffixes(word);

            if (ExceptionsRegion2.Contains(word))
            {
                return new StemmedWord(word, original);
            }

            word = Step1BRemoveLySuffixes(word, r1);
            word = Step1CReplaceSuffixYWithIIfPreceededWithConsonant(word);
            word = Step2ReplaceSuffixes(word, r1);
            word = Step3ReplaceSuffixes(word, r1, r2);
            word = Step4RemoveSomeSuffixesInR2(word, r2);
            word = Step5RemoveEorLSuffixes(word, r1, r2);

            return new StemmedWord(word.ToLowerInvariant(), original);
        }
    }

    
}