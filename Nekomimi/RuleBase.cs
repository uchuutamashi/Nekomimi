using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Nekomimi
{
    class RuleBase
    {
        // MUTEX used for locking the List of Rules 
        static object rbMUTEX = new object();

        // List for storing all the Rules
        static List<Rule> mRules = new List<Rule>();

        /// <summary>
        /// Load Rules from directory. Note that all sub-directories are included
        /// </summary>
        /// <param name="DirectoryPath">Directory Path</param>
        static public void Load(string DirectoryPath)
        {
            lock (rbMUTEX)
            {
                foreach (string file in Directory.EnumerateFiles(DirectoryPath, "*.nmr", SearchOption.AllDirectories))
                {
                    foreach (string line in File.ReadAllLines(file, Encoding.UTF8))
                    {
                        Add(Rule.FromString(line));
                    }
                }
            }
        }

        /// <summary>
        /// Add a new Rule to the RuleBase
        /// </summary>
        /// <param name="Object">Rule to be added</param>
        static public void Add(Rule Object)
        {
            lock (rbMUTEX)
            {
                var tmp = Find(Object.GetProdType());
                if (tmp != null)
                {
                    // Object has the same pattern with an existing Rule                    
                    if (tmp != Object)
                    {
                        Contradiction.Throw(Contradiction.ERRCODE.RULE, new Rule[2] { Object, tmp });
                        return;
                    }
                }

                if (!mRules.Contains(Object))
                {
                    mRules.Add(Object);
                }
            }
        }

        /// <summary>
        /// Find all applicable rules for a collection of Concepts
        /// </summary>
        /// <param name="list">The list of Concepts</param>
        /// <returns>A list of Rules</returns>
        static public List<Rule> FindApplicableRules(List<Concept> list)
        {
            List<Rule> results = new List<Rule>();
            Parallel.ForEach(mRules, r =>
            {
                if (r.IsMatch(list))
                {
                    results.Add(r);
                }
            });

            return results;
        }

        /// <summary>
        /// Find Rule in RuleBase by Name
        /// e.g. Find("Apple")
        /// </summary>
        /// <param name="Name">Name</param>
        /// <returns>The first occurance of Rule that has the specified name. If none is found, a null object is returned.</returns>
        static public Rule Find(string ProdType)
        {
            lock (rbMUTEX)
            {
                foreach (Rule r in mRules)
                {
                    if (r.GetProdType()==ProdType)
                    {
                        return r;
                    }
                }

                return null;
            }
        }

       
    }
}
