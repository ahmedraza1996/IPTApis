using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IptApis.Controllers.Search_Module.TextProcessingHelpers;

namespace IptApis.Controllers.Search_Module.QueryParser
{
    public class InfixToPostfixConvertor
    {
        private static bool NotGreaterPrecedence(string stackPeekSymbol, string currentSybmol)
        {
            Dictionary<string, int> Precedence = new Dictionary<string, int>();
            Precedence.Add("|", 1);
            Precedence.Add("&", 2);
            Precedence.Add("!", 3);
            Precedence.Add("(", 4);

            try
            {
                var a = Precedence[currentSybmol];
                var b = Precedence[stackPeekSymbol];
                return a <= b;
            }
            catch
            {
                return false;
            }
        }

        public static IList<string> Convert(string query)
        {
            query = query.Replace("(", " ( ").Replace(")", " ) ").Replace("&", " & ").Replace("|", " | ").Replace("!", " ! ");
            query = Regex.Replace(query, @"\s+", @" ").Trim();

            string[] tokens = query.Split(' ');
            List<string> postfix = new List<string>();
            Stack<string> operatorsStack = new Stack<string>();

            Dictionary<string, int> Precedence = new Dictionary<string, int>();
            Precedence.Add("|", 1);
            Precedence.Add("&", 2);
            Precedence.Add("!", 3);
            Precedence.Add("(", 4);

            int bracketLevels = 0;

            try
            {
                foreach (var _token in tokens)
                {
                    var token = _token.Trim();
                    if (token == "")
                    {
                        continue;
                    }
                    else if (!Precedence.ContainsKey(token) && token != ")")
                    {
                        token = Preprocessor.GetInstance().Preprocess(token);
                        if (token == "")
                        {
                            continue;
                        }
                        else
                        {
                            postfix.Add(token);
                        }
                    }
                    else if (token == "(")
                    {
                        bracketLevels += 1;
                    }
                    else if (token == ")")
                    {
                        if (bracketLevels == 0 || operatorsStack.Count == 0)
                        {
                            new Exception("Invalid infix expression! Tried to close a bracket which has no opening bracket");
                        }
                        else if (operatorsStack.Count > 0)
                        {
                            postfix.Add(operatorsStack.Pop());
                            bracketLevels -= 1;
                        }
                    }
                    else
                    {
                        while (operatorsStack.Count > 0 && bracketLevels == 0 && NotGreaterPrecedence(operatorsStack.Peek(), token))
                        {
                            postfix.Add(operatorsStack.Pop());
                        }
                        operatorsStack.Push(token);
                    }
                }
                if (bracketLevels > 0)
                {
                    new Exception("Invalid infix expression! A bracket has not been closed");
                }
                if (bracketLevels < 0)
                {
                    new Exception("Invalid infix expression! Tried to close a bracket which has no opening bracket");
                }
                while (operatorsStack.Count > 0)
                {
                    postfix.Add(operatorsStack.Pop());
                }

            }
            catch
            {
                throw new Exception("Invalid boolean query");
            }



            return postfix;
        }
    }
}