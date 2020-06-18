using System;
using System.Collections.Generic;
using IptApis.Controllers.Search_Module.TextProcessingHelpers;

namespace IptApis.Controllers.Search_Module.Indexes
{
    public class Vector : IEquatable<Vector>
    {
        public Dictionary<string, Index> Dictionary;

        public Vector(string documentString)
        {
            Dictionary = new Dictionary<string, Index>();
            foreach(var token in Tokenizer.TokenizeWord(documentString))
            {
                if (WordExisits(token))
                {
                    UpdateVector(token);
                }
                else
                {
                    AddTokenToVector(token);
                }
            }
        }

        public Vector(Dictionary<string, Index> Dictionary)
        {
            this.Dictionary = Dictionary;
        }

        private bool WordExisits(Token token)
        {
            return Dictionary.ContainsKey(token.Word);
        }

        private void AddTokenToVector(Token token)
        {
            Index index = new Index(token);
            Dictionary.Add(token.Word, index);
        }

        private void UpdateVector(Token token)
        {
            Index index;
            Dictionary.TryGetValue(token.Word, out index);
            index.AddOccurrence(token.Position);
            Dictionary[token.Word] = index;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            Vector vector = (Vector)obj;
            return Equals(vector);
        }

        public bool Equals(Vector vector)
        {
            try
            {
                if (Dictionary.Count != vector.Dictionary.Count)
                {
                    return false;
                }
                foreach(var key in Dictionary.Keys)
                {
                    var index = Dictionary[key];
                    var index2 = vector.Dictionary[key];
                    if (!index.Equals(index2))
                    {
                        return false;
                    }
                }
            } catch
            {
                return false;
            }
            return true;
        }
    }
}