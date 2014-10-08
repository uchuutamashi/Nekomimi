using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
                foreach (string file in Directory.EnumerateFiles(DirectoryPath, "*.nc", SearchOption.AllDirectories))
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
                var tmp=Find(Object.Name(),Object.Type());
                if (tmp != null)
                {
                    // Object has the same name-type combination with an existing Concept
                    // Hence there is a contradiction
                    if (tmp != Object)
                    {
                        Contradiction.Throw(new Concept[2] { Object, tmp });
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
        /// Find Concept in ConceptBase by Name
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
                    if (c.Name() == Name)
                    {
                        return c;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Find Concept in ConceptBase by Name-Type combination
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
                    if (c.Name() == Name && c.GetProperty("TYPE")==Type)
                    {
                        return c;
                    }
                }

                return null;
            }
        }


        /// <summary>
        /// Find Concept(s) in ConceptBase that has the specified value for the specified property.
        /// e.g. Find("Color","Red")
        /// </summary>
        /// <param name="Property">Property</param>
        /// <param name="Value">Value</param>
        /// <returns>A List containing all the matching Concept(s).</returns>
        static public List<Concept> FindProp(string Property,string Value)
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

    }
}
