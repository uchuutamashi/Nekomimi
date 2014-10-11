using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nekomimi
{
    /// <summary>
    ///  This is used to parse the IForm of a rule.
    ///  e.g. {0}.{1.TYPE}={1}
    /// </summary>
    static class RParser
    {
        /// <summary>
        /// The main function
        /// </summary>
        /// <param name="rule">String representing the rule</param>
        /// <param name="args">Arguments</param>
        /// <returns>Parsed string</returns>

        static public string Parse(string rule, List<Concept> args)
        {
            string result = rule;

            //Extract content in { } and do replacements
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
