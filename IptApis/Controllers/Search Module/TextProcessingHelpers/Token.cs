using System;

namespace IptApis.Controllers.Search_Module.TextProcessingHelpers
{
    public class Token : IEquatable<Token>
    {
        public readonly string Word;
        public readonly int Position;

        public Token(string word, int position)
        {
            Preprocessor preprocessor = Preprocessor.GetInstance();
            Word = preprocessor.Preprocess(word);
            Position = position;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            Token token = (Token)obj;
            return (Word == token.Word) && (Position == token.Position);
        }

        public bool Equals(Token token)
        {
            return (Word == token.Word) && (Position == token.Position);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Word, Position).GetHashCode();
        }

    }
}