using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraigslistJobApplier
{
    public class PunctuationInsensitiveComparer : IEqualityComparer<String>
    {
        public bool Equals(String x, String y)
        {
            x = new String(x.Where(c => !Char.IsPunctuation(c)).ToArray());
            y = new String(y.Where(c => !Char.IsPunctuation(c)).ToArray());
            return String.Equals(x, y, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(String s)
        {
            s = s.ToLower();
            s = new String(s.Where(c => !Char.IsPunctuation(c)).ToArray());
            return s.GetHashCode();
        }
    }
}
