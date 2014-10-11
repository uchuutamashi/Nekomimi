using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nekomimi
{
    static class RParser
    {
        static public string Parse(string rule, List<Concept> args)
        {
            string result = rule;
            var terms = Utils.ExtractFromBracket(rule);
            foreach (string term in terms)
            {
                if (term.Contains("."))
                {
                    string num = Utils.LSplit(term, ".");
                    string prop = Utils.RSplit(term, ".");

                    result.Replace("{" + term + "}", args[Convert.ToInt32(num)].GetProperty(prop));
                }
                else
                {
                    result.Replace("{" + term + "}", args[Convert.ToInt32(term)].Name());
                }

            }
            return result;
        }
    }
}
