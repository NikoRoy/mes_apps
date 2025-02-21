using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary
{
    [Serializable]
    public class StoredProcedureException : Exception
    {
        public StoredProcedureException(string s) : base(s) { }
        public StoredProcedureException(string s, System.Exception inner) : base(s, inner) { }

    }
}
