using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nekomimi
{
    /// <summary>
    /// The idea of a "Concept" is similar to that of a class in programming languages. 
    /// A Concept has various properties with different values that can be read/written.
    /// It can be thought of as a meta-representation of a class.
    /// It is a reference type object.
    /// </summary>
    class Concept
    {

        readonly string mName = "";
        Dictionary<string, string> mdictProperties = new Dictionary<string, string>();
        Concept mParent = null;

        /// <summary>
        /// Creates a new concept.
        /// </summary>
        /// <param name="Name">The external name of the concept. External name is the term that is used to refer to this concept in the input.</param>
        public Concept(string Name)
        {
            mName = Name;
        }

        public Concept(string Name, string Type)
        {
            mName = Name;
            SetProperty("TYPE", Type);
        }

        public Concept Parent
        {
            get
            {
                return mParent;
            }
            set
            {
                mParent = Parent;
            }
        }

        public string Name
        {
            get
            {
                return mName;
            }
        }

        public string Type
        {
            get
            {
                string type = GetProperty("TYPE");
                if (type != null)
                {
                    return type;
                }
                return "NONE";
            }
        }

        public override string ToString()
        {
            return mName;
        }

        // Equalities
        //---------------------------------------------------------------------------------------
        public static bool operator ==(Concept a, Concept b)
        {
            if ((object)a == null || (object)b == null) return false;
            return a.mName == b.mName && Utils.Identical(a.mdictProperties, b.mdictProperties);
        }
        public static bool operator !=(Concept a, Concept b)
        {
            if ((object)a == null ^ (object)b == null) return true;
            if ((object)a == null && (object)b == null) return false;
            return a.mName != b.mName || !Utils.Identical(a.mdictProperties, b.mdictProperties);
        }
        public override bool Equals(object obj)
        {
            Concept c = obj as Concept;
            if ((object)c == null)
            {
                return false;
            }

            return c.mName == mName && Utils.Identical(c.mdictProperties, mdictProperties);
        }
        public bool Equals(Concept c)
        {
            if ((object)c == null) return false;
            return c.mName == mName && Utils.Identical(c.mdictProperties,mdictProperties);
        }
        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Return a Concept from a UTF-8 text file.
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <returns></returns>
        static public Concept FromFile(string FilePath)
        {
            Concept c = new Concept(Path.GetFileNameWithoutExtension(FilePath));

            foreach (string line in File.ReadAllLines(FilePath, Encoding.UTF8))
            {
                c.SetProperty(Utils.LSplit(line, "="), Utils.RSplit(line, "="));
            }

            if (c.GetProperty("PARENT") != null)
            {
                c.Parent = ConceptBase.Find(c.GetProperty("PARENT"));
            }

            return c;
        }

        /// <summary>
        /// Return the specified property. If property does not exist, a null object (not an empty string) is returned.
        /// </summary>
        /// <param name="Field">Name of the property</param>
        /// <returns></returns>
        public string GetProperty(string Field)
        {
            if (mdictProperties.ContainsKey(Field))
            {
                return mdictProperties[Field];
            }            
            else if (mParent != null) //if the property can't be found, look for it at the parent's concept file
            {
                return mParent.GetProperty(Field);
            }
            return null;
        }
        
        /// <summary>
        /// Set the specified property to the specified value. If property does not exist, it is created and initialized to the specified value.
        /// </summary>
        /// <param name="Field">Name of the property</param>
        /// <param name="Value">Value</param>
        public void SetProperty(string Field,string Value)
        {
            if (mParent != null && mParent.GetProperty(Field) != Value)  //check for inheritance conflict
            {
                Contradiction.Throw(Contradiction.ERRCODE.INHERITANCE, new Concept[2] { mParent, this });
                return;
            }

            if (mdictProperties.ContainsKey(Field)) 
            {
                mdictProperties[Field] = Value;
            }
            else
            {
                mdictProperties.Add(Field, Value);
            }
        }
    }
}
