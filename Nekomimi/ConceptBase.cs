using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Nekomimi
{
    /// <summary>
    /// A shared, static database that stores all the concepts
    /// </summary>
    static class ConceptBase
    {
        // MUTEX used for locking the List of Concepts 
        static object cbMUTEX = new object();

        // List for storing all the Concepts
        static List<Concept> mConcepts = new List<Concept>();

        /// <summary>
        /// Load Concepts from directory. Note that all sub-directories are included
        /// </summary>
        /// <param name="DirectoryPath">Directory Path</param>
        static public void Load(string DirectoryPath)
        {
            lock (cbMUTEX)
            {
                foreach (string file in Directory.EnumerateFiles(DirectoryPath, "*.nmc", SearchOption.AllDirectories))
                {
                    Add(Concept.FromFile(file));
                }
            }
        }

        /// <summary>
        /// Add a new Concept to the ConceptBase
        /// </summary>
        /// <param name="Object">Concept to be added</param>
        static public void Add(Concept Object)
        {
            lock (cbMUTEX)
            {
                var tmp = Find(Object.Name, Object.Type);
                if (tmp != null)
                {
                    // Object has the same name-type combination with an existing Concept
                    // Hence there is a direct contradiction
                    if (tmp != Object)
                    {
                        Contradiction.Throw(Contradiction.ERRCODE.DIRECT,new Concept[2] { Object, tmp });
                        return;
                    }
                }

                if (!mConcepts.Contains(Object))
                {
                    mConcepts.Add(Object);
                }
            }
        }

        /// <summary>
        /// Find Concept in ConceptBase by Name. Null returned if not found
        /// e.g. Find("Apple")
        /// </summary>
        /// <param name="Name">Name</param>
        /// <returns>The first occurance of Concept that has the specified name. If none is found, a null object is returned.</returns>
        static public Concept Find(string Name)
        {
            lock (cbMUTEX)
            {
                foreach (Concept c in mConcepts)
                {
                    if (c.Name == Name)
                    {
                        return c;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Find Concept in ConceptBase by Name-Type combination. Null returned if not found.
        /// e.g. Find("Apple", "Fruit")
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="Type">Type</param>
        /// <returns>The Concept that has the specified Name-Type combination. If none is found, a null object is returned.</returns>
        static public Concept Find(string Name, string Type)
        {
            lock (cbMUTEX)
            {
                foreach (Concept c in mConcepts)
                {
                    if (c.Name == Name && c.GetProperty("TYPE") == Type)
                    {
                        return c;
                    }
                }

                return null;
            }
        }


        /// <summary>
        /// Find Concept(s) in ConceptBase that has the specified value for the specified property.
        /// e.g. Where("Color","Red")
        /// </summary>
        /// <param name="Property">Property</param>
        /// <param name="Value">Value</param>
        /// <returns>A List containing all the matching Concept(s).</returns>
        static public List<Concept> Where(string Property, string Value)
        {
            lock (cbMUTEX)
            {
                List<Concept> results = new List<Concept>();
                foreach (Concept c in mConcepts)
                {
                    if (c.GetProperty(Property) == Value)
                    {
                        results.Add(c);
                    }
                }

                return results;
            }
        }

        /// <summary>
        /// Extract all Concept(s) that can be found in a string. Note that extraction is not parsing so it is non-exclusive and there may be overlapping. 
        /// </summary>
        /// <param name="Source">string to extract from</param> 
        /// <returns>A List of Concept(s) that is seen in Source</returns>
        static public List<Concept> Extract(string Source)
        {
            
            string remaining = Source;
            List<Concept> results = new List<Concept>();

            Parallel.ForEach(mConcepts, c =>
            {
                string tmp = Source;
                while (tmp.Contains(c.Name))
                {
                    results.Add(c);
                    tmp = Utils.ReplaceOnce(tmp, c.Name, "");
                }
            });


            foreach (Concept c in results)
            {
                //for unknown extraction
                remaining = Utils.ReplaceOnce(remaining, c.Name, " ");
            }

            foreach (string s in remaining.Split(new char[1]{' '}, StringSplitOptions.RemoveEmptyEntries))
            {
                results.Add(new Concept(s, "UNKNOWN"));
            }

            return results;
        }

    }
}
