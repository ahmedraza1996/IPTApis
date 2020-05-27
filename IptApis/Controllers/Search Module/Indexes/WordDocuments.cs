using System;
using System.Collections.Generic;

namespace IptApis.Controllers.Search_Module.Indexes
{
    public class WordDocuments : IEquatable<WordDocuments>
    {
        public string Word;
        public Dictionary<int, Index> DocumentsDictionary;
        public int WordCount;
        public int DocumentCount;

        public WordDocuments(int documentIndex, Index index)
        {
            Word = index.Word;
            WordCount = 0;
            DocumentCount = 0;
            DocumentsDictionary = new Dictionary<int, Index>();
            Update(documentIndex, index);
        }

        public HashSet<int> GetDocuments()
        {
            HashSet<int> documents = new HashSet<int>();

            foreach(var key in DocumentsDictionary.Keys)
            {
                documents.Add(key);
            }

            return documents;
        }

        public bool Equals(WordDocuments wordDocuments)
        {
            if (DocumentsDictionary.Count != wordDocuments.DocumentsDictionary.Count)
                return false;
            return EqualHelper(wordDocuments) && (Word.Equals(wordDocuments.Word)) && (WordCount == wordDocuments.WordCount) && (DocumentCount == wordDocuments.DocumentCount);
        }

        private bool EqualHelper(WordDocuments wordDocuments)
        {
            try
            {
                if (DocumentsDictionary.Count != wordDocuments.DocumentsDictionary.Count)
                {
                    return false;
                }
                foreach (var key in DocumentsDictionary.Keys)
                {
                    var index = DocumentsDictionary[key];
                    var index2 = wordDocuments.DocumentsDictionary[key];
                    if (!index.Equals(index2))
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void Update(int documentIndex, Index index)
        {
            DocumentsDictionary.Add(documentIndex, index);
            WordCount += index.Count;
            DocumentCount += 1;
        }

    }
}