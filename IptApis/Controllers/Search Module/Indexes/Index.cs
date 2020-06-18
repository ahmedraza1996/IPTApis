using System;
using System.Collections.Generic;
using IptApis.Controllers.Search_Module.TextProcessingHelpers;

namespace IptApis.Controllers.Search_Module.Indexes
{
    public class Index : IEquatable<Index>
    {
        public string Word;
        public double Count;
        public IList<int> Positions;

        public Index()
        {
            Word = "";
            Count = 0;
            Positions = new List<int>();
        }

        public Index(Token token)
        {
            Count = 1;
            Word = token.Word;
            Positions = new List<int>();
            Positions.Add(token.Position);
        }

        public void AddOccurrence(int position)
        {
            Count += 1;
            Positions.Add(position);
        }

        public bool Equals(Index index)
        {
            if (Positions.Count != index.Positions.Count)
                return false;
            for(int i=0; i<Positions.Count; i++)
            {
                if (Positions[i] != index.Positions[i])
                    return false;
            }
            return (Word.Equals(index.Word)) && (Count == index.Count);
        }
    }
}