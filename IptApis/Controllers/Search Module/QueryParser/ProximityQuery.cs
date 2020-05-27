using IptApis.Controllers.Search_Module.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IptApis.Controllers.Search_Module.QueryParser
{
    public class ProximityQuery
    {
        public IList<int> Parse(string query, WordsVector wordsVector)
        {
            var ProximityQueryRegex = new Regex(@"^(\w+)\s(\w+)\s*/\s*(\d+)$");
            var groups = ProximityQueryRegex.Match(query).Groups;
            var operand1 = groups[1].Value;
            var operand2 = groups[2].Value;
            var displacement = Convert.ToInt32(groups[3].Value);

            List<int> answer = new List<int>();

            try
            {
                var document1 = wordsVector.GetVector()[operand1];
                var document2 = wordsVector.GetVector()[operand2];

                var commonDocuments = document1.GetDocuments();
                commonDocuments.IntersectWith(document2.GetDocuments());

                foreach(int doc in commonDocuments)
                {
                    var list1 = document1.DocumentsDictionary[doc].Positions;
                    var list2 = document2.DocumentsDictionary[doc].Positions;

                    foreach(var position in list1)
                    {
                        if (list2.Contains(position + displacement) || list2.Contains(position - displacement))
                        {
                            answer.Add(doc);
                            break;
                        }
                    }
                }
                answer.Sort();
                return answer;
            }
            catch
            {
                return new List<int>();
            }

        }
    }