﻿using System;
using System.Collections.Generic;

namespace IptApis.Controllers.Search_Module.Indexes
{
    public class DocumentsVector : IEquatable<DocumentsVector>
    {
        public Dictionary<int, Vector> Value;

        public DocumentsVector()
        {
            Value = new Dictionary<int, Vector>();
        }

        public List<int> DocumentsIndex()
        {
            List<int> keys = new List<int>();
            foreach (var pair in Value)
            {
                keys.Add(pair.Key);
            }
            return keys;
        }
        public HashSet<int> DocumentsSet()
        {
            HashSet<int> keys = new HashSet<int>();
            foreach (var pair in Value)
            {
                keys.Add(pair.Key);
            }
            return keys;
        }
        public Vector GetVectorOfDocumentIndex(int documentIndex)
        {
            return Value[documentIndex];
        }

        public DocumentsVector(int documentIndex, Vector vector)
        {
            Value = new Dictionary<int, Vector>();
            Update(documentIndex, vector);
        }
        public DocumentsVector(Dictionary<int, Vector> Value) 
        {
            this.Value = Value;
        }

        public void Update(int documentIndex, Vector vector)
        {
            Value.Add(documentIndex, vector);
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            DocumentsVector vector = (DocumentsVector)obj;
            return Equals(vector);
        }

        public bool Equals(DocumentsVector documentsVector)
        {
            try
            {
                if (Value.Count != documentsVector.Value.Count)
                {
                    return false;
                }
                foreach (var key in Value.Keys)
                {
                    var index = Value[key];
                    var index2 = documentsVector.Value[key];
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