using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MESFeedClientLibrary
{
    [Serializable]
    public class UNumberException : System.Exception
    {
        private static readonly string regularExpression = @"\b(Employee|not\found)\b";

        public UNumberException() { }
        public UNumberException(string s) : base(s) { }
        public UNumberException(string s, System.Exception inner) : base(s, inner) { }

        public static bool IsUNumberException(string s)
        {
            Regex rx = new Regex(UNumberException.regularExpression);
            if (rx.IsMatch(s))
                return true;
            return false;
        }
    }
}
