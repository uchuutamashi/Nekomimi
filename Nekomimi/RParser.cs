using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nekomimi
{
    /// <summary>
    ///  This is used to parse the IForm of a rule.
    ///  e.g. {0}.{1.TYPE}={1}
    ///  e.g. RETURN({1.PARENT}.TYPE)
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
            string current = result;
            do
            {
                current = result;
                //Extract content in { } and do replacements
                var terms = Utils.ExtractFromBracket(result);
                foreach (string term in terms)
                {
                    if (term.Contains("."))
                    {
                        string num = Utils.LSplit(term, ".");
                        string prop = Utils.RSplit(term, ".");

                        result = result.Replace("{" + term + "}", args[Convert.ToInt32(num)].GetProperty(prop));
                    }
                    else
                    {
                        result = result.Replace("{" + term + "}", args[Convert.ToInt32(term)].Name);
                    }

                }

            } while (result != current);



            return result;
        }
    }
}
