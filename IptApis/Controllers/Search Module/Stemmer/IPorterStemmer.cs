using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IptApis.Controllers.Search_Module.Stemmer
{
    interface IPorterStemmer : IStemmer
    {
        char[] Vowels { get; }

        string[] Doubles { get; }

        char[] LiEndings { get; }

        /// <summary>
        /// R1 is the region after the first non-vowel following a vowel, 
        /// or the end of the word if there is no such non-vowel. 
        /// This definition may be modified for certain exceptional words.
        /// </summary>
        int GetRegion1(string word);

        /// <summary>
        /// R2 is the region after the first non-vowel following a vowel in 
        /// R1, or the end of the word if there is no such non-vowel.
        /// </summary>
        int GetRegion2(string word);

    }
}
