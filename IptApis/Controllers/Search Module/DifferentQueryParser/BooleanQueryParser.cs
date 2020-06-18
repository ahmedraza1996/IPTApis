using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IptApis.Controllers.Search_Module.Indexes;
using System.Linq;


namespace IptApis.Controllers.Search_Module.QueryParser
{
    public class BooleanQueryParser
    {
        public static IList<int> Parse(string query, HashSet<int> universalSet, WordsVector wordsVector)
        {
            var PostfixQuery = InfixToPostfixConvertor.Convert(query);

            Stack<HashSet<int>> stack = new Stack<HashSet<int>>();

            foreach(var item in PostfixQuery)
            {
                if (item == "!")
                {
                    var operand = stack.Pop();

                    var remaining = new HashSet<int>(universalSet);
                    remaining.ExceptWith(operand);
                    stack.Push(remaining);

                }
                else if (item == "&")
                {
                    var operand1 = stack.Pop();
                    var operand2 = stack.Pop();
                    operand1.IntersectWith(operand2);
                    stack.Push(operand1);
                }
                else if (item == "|")
                {
                    var operand1 = stack.Pop();
                    var operand2 = stack.Pop();
                    operand1.UnionWith(operand2);
                    stack.Push(operand1);
                }
                else
                {
                    try
                    {
                        var documents = wordsVector.GetVector()[item].GetDocuments();
                        stack.Push(documents);
                    } catch
                    {
                        stack.Push(new HashSet<int>());
                    }
                }
            }
            var answer = stack.Pop().ToList();
            answer.Sort();
            return answer;
        }
    }
}