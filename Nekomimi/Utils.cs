using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nekomimi
{
    /// <summary>
    /// Various useful string and array processing & generating functions.
    /// </summary>
    static class Utils
    {
        /// <summary>
        /// Replace the first encountered instance of old string in source with new string.
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="oldstr">Original content</param>
        /// <param name="newstr">New content</param>
        /// <returns>String with first instance of oldstr replaced</returns>
        public static string ReplaceOnce(string source, string oldstr, string newstr)
        {
            int index = source.IndexOf(oldstr);
            if (index == -1) { return source; }
            return source.Insert(index, newstr).Remove(index + newstr.Length, oldstr.Length);
        }

        /// <summary>
        /// Return the string to the right of the first encountered separator.
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="sep">Separator</param>
        /// <returns>Content to the right of separator</returns>
        public static string RSplit(string source, string sep)
        {
            int index = source.ToLower().IndexOf(sep.ToLower());
            if (index == -1) return "";
            return source.Substring(index + sep.Length).Trim();
        }

        /// <summary>
        /// Return the string to the left of the first encountered separator.
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="sep">Separator</param>
        /// <returns>Content to the right of separator</returns>
        public static string LSplit(string source, string sep)
        {
            int index = source.ToLower().IndexOf(sep.ToLower());
            if (index == -1) return "";
            return source.Substring(0, index).Trim();
        }

        /// <summary>
        /// Default operator== for List compares the addresses rather than content. This function provides content comparison.
        /// </summary>
        /// <param name="lhs">List 1</param>
        /// <param name="rhs">List 2</param>
        /// <returns></returns>
        public static bool Identical<T>(IEnumerable<T> lhs, IEnumerable<T> rhs)
        {
            if (lhs.Count<T>() == rhs.Count<T>())
            {
                for (int i = 0; i < lhs.Count<T>(); i++)
                {
                    if (!lhs.ElementAt<T>(i).Equals( rhs.ElementAt<T>(i) ))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
