using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
                var tmp = Find(Object.Name(), Object.Type());
                if (tmp != null)
                {
                    // Object has the same name-type combination with an existing Rule
                    // Hence there is a direct contradiction
                    if (tmp != Object)
                    {
                        Contradiction.Throw(Contradiction.ERRCODE.DIRECT, new Rule[2] { Object, tmp });
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
        /// Find Rule in RuleBase by Name
        /// e.g. Find("Apple")
        /// </summary>
        /// <param name="Name">Name</param>
        /// <returns>The first occurance of Rule that has the specified name. If none is found, a null object is returned.</returns>
        static public Rule Find(string Name)
        {
            lock (rbMUTEX)
            {
                foreach (Rule c in mRules)
                {
                    if (c.Name() == Name)
                    {
                        return c;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Find Rule in RuleBase by Name-Type combination
        /// e.g. Find("Apple", "Fruit")
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="Type">Type</param>
        /// <returns>The Rule that has the specified Name-Type combination. If none is found, a null object is returned.</returns>
        static public Rule Find(string Name, string Type)
        {
            lock (rbMUTEX)
            {
                foreach (Rule c in mRules)
                {
                    if (c.Name() == Name && c.GetProperty("TYPE") == Type)
                    {
                        return c;
                    }
                }

                return null;
            }
        }
    }
}
