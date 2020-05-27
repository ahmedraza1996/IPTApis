using System;
using System.Collections.Generic;
using IptApis.Controllers.Search_Module.TextProcessingHelpers;

namespace IptApis.Controllers.Search_Module.Indexes
{
    public class WordsVector : IEquatable<WordsVector>
    {
        Dictionary<string, WordDocuments> Values;

        public WordsVector()
        {
            Values = new Dictionary<string, WordDocuments>();
        }

        public WordsVector(int documentIndex, Vector vector)
        {
            Values = new Dictionary<string, WordDocuments>();
            Update(documentIndex, vector);
        }

        public WordsVector(Dictionary<string, WordDocuments> Values)
        {
            this.Values = Values;
        }


        public void Update(int documentIndex, Vector vector)
        {
            foreach (var token in vector.Dictionary.Keys)
            {
                Index index = vector.Dictionary[token];
                if (WordExisits(token))
                {
                    UpdateToken(documentIndex ,token, index);
                }
                else
                {
                    AddToken(documentIndex, token, index);
                }
            }

        }

        private bool WordExisits(string word)
        {
            return Values.ContainsKey(word);
        }

        private void AddToken(int documentIndex, string word, Index index)
        {
            WordDocuments wordDocuments = new WordDocuments(documentIndex, index);
            Values.Add(word, wordDocuments);
        }

        private void UpdateToken(int documentIndex, string word, Index index)
        {
            Values[word].Update(documentIndex, index);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            WordsVector vector = (WordsVector)obj;
            return Equals(vector);
        }

        public bool Equals(WordsVector vector)
        {
            try
            {
                if (Values.Count != vector.Values.Count)
                {
                    return false;
                }
                foreach (var key in Values.Keys)
                {
                    var index = Values[key];
                    var index2 = vector.Values[key];
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


    }

}