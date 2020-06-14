using IptApis.Controllers.Search_Module.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IptApis.Controllers.Search_Module.QueryParser
{
    public class GeneralTextQueryParser
    {

        public static IList<int> Parse(string query, WordsVector wordsVector, DocumentsVector documentsVector)
        {
            var query_tfidf = ComputeQueryTfIdfVector(query, wordsVector, documentsVector.DocumentsIndex().Count);

            var documentsIndex = DocumentsOccuredInQuery(wordsVector, query_tfidf);

            var similarities = new Dictionary<int,double>();

            foreach (var index in documentsIndex)
            {
                var similarity = ComputeCosineSimilarity(
                    documentsVector.GetVectorOfDocumentIndex(index), 
                    query_tfidf);
                similarities.Add(index, similarity);
            }
            return SortTheDictionaryOnValue(similarities);
        }

        private static List<int> SortTheDictionaryOnValue(Dictionary<int, double> dictionary)
        {
            var sortedList = dictionary.ToList();

            sortedList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            var keys = sortedList.Select(x => x.Key).ToList();
            return keys;
        }

        private static List<int> DocumentsOccuredInQuery(WordsVector wordsVector, Vector queryVector)
        {
            List<int> documents = new List<int>();
            foreach(var pair in queryVector.Dictionary)
            {
                var key = pair.Key;
                if (wordsVector.GetVector().ContainsKey(key))
                {
                    var newDocuments = wordsVector.GetVector()[key].GetDocuments();
                    documents = AddDocuments(documents, newDocuments);   
                }
            }
            return documents;
        }

        private static List<int> AddDocuments(List<int> prevdocuments, HashSet<int> newDocuments)
        {
            HashSet<int> documents = prevdocuments.ToHashSet();
            documents.UnionWith(newDocuments);
            return documents.ToList();
        }

        private static Vector ComputeQueryTfIdfVector(string query, WordsVector wordsVector, int totalDocuments)
        {
            Vector vector = new Vector(query);

            foreach (var pair in vector.Dictionary)
            {
                var key = pair.Key;
                double inverseDocumentFrequency = 0;
                if (wordsVector.GetVector().ContainsKey(key))
                {
                    var documentFrequency = wordsVector.GetVector()[key].DocumentCount;
                    inverseDocumentFrequency = Math.Log10(documentFrequency) / totalDocuments;
                }
                vector.Dictionary[key].Count *= inverseDocumentFrequency; 
            }

            return vector;
        }

        private static double ComputeCosineSimilarity(Vector document, Vector query)
        {
            double numerator, magnitude_q, magnitude_d;

            numerator = ComputeMagnitude(query, document);
            magnitude_q = Math.Sqrt(ComputeMagnitude(query, query));
            magnitude_d = Math.Sqrt(ComputeMagnitude(document, document));

            double denominator = magnitude_d * magnitude_q;
            return (numerator + 0.5) / (denominator + 1.0);
        }

        private static double ComputeMagnitude(Vector vector1, Vector vector2)
        {
            double magnitude = 0.0;
            foreach(var pair in vector1.Dictionary)
            {
                var key = pair.Key;
                if (vector2.Dictionary.ContainsKey(key))
                {
                    magnitude += vector1.Dictionary[key].Count * vector2.Dictionary[key].Count;
                }
            }
            return magnitude;
        }


    }
}