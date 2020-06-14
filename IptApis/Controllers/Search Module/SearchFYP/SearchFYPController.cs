using IptApis.Controllers.Search_Module.Indexes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace IptApis.Controllers.Search_Module.SearchFYP
{
    public class SearchFYPController : ApiController
    {

        WordsVector wordsVector;
        DocumentsVector documentsVector;

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Hello World");
        }

        public HttpResponseMessage GetEmbeddings()
        {

            return Request.CreateResponse(HttpStatusCode.OK, "Hello World");
        }

        [HttpPost]
        public HttpResponseMessage UploadFile(FYPSearchModel model)
        {
            WordsVector wordsVector = ReadWordVectorFile();
            DocumentsVector documentsVector = ReadDocumentVectorFile();

            Vector vector = new Vector(model.description);

            int index = documentsVector.Value.Count + 1;

            wordsVector.Update(index, vector);
            documentsVector.Update(index, vector);

            UpdateWordVectorFile(wordsVector);
            UpdateDocumentVectorFile(documentsVector);

            return Request.CreateResponse(HttpStatusCode.OK, "Hello World");
        }

        public HttpResponseMessage UploadFile(string title) 
        {
            FYPSearchModel model = new FYPSearchModel();
            model.description = title;
            return UploadFile(model);
        }


        private void UpdateWordVectorFile(WordsVector vector)
        {
            var path = "~/Controllers/Search Module/Content/WordsDocument.txt";
            var filePath = HttpContext.Current.Server.MapPath(path);
            var json = new JavaScriptSerializer().Serialize(vector);
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(json);
            }

        }

        private void UpdateDocumentVectorFile(DocumentsVector vector)
        {
            var path = "~/Controllers/Search Module/Content/DocumentsVector.txt";
            var filePath = HttpContext.Current.Server.MapPath(path);

            XmlSerializer serializer = new XmlSerializer(typeof(DocumentsVector));
            

  //          var json = new JavaScriptSerializer().Serialize(vector);
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, vector);
//                writer.WriteLine(json);
            }
        }

        private WordsVector ReadWordVectorFile()
        {
            try
            {
                var path = "~/Controllers/Search Module/Content/WordsDocument.txt";
                var filePath = HttpContext.Current.Server.MapPath(path);
                using (StreamReader reader = new StreamReader(filePath))
                {
                    var data = reader.ReadLine();
                    WordsVector vector = new WordsVector();
                    var json = new JavaScriptSerializer().Deserialize(data, vector.GetType());
                    return vector;
                }

            }
            catch
            {
                return new WordsVector();
            }
        }
        private DocumentsVector ReadDocumentVectorFile()
        {
            try
            {
                var path = "~/Controllers/Search Module/Content/DocumentsVector.txt";
                var filePath = HttpContext.Current.Server.MapPath(path);
                using (StreamReader reader = new StreamReader(filePath))
                {
                    var data = reader.ReadLine();
                    DocumentsVector vector = new DocumentsVector();
                    var json = new JavaScriptSerializer().Deserialize(data, vector.GetType());
                    return vector;
                }

            }
            catch
            {
                return new DocumentsVector();
            }
        }

    }
}