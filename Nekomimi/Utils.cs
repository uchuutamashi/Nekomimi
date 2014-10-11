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
                    if (!lhs.ElementAt<T>(i).Equals(rhs.ElementAt<T>(i)))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public static List<T> Order<T>(IEnumerable<T> list, string order)
        {
            T[] results = list.ToArray();
            List<int> index = new List<int>();
            string tmp = order;

            foreach (T t in list)
            {
                tmp = order;
                while (tmp.Contains(t.ToString()))
                {
                    if (!index.Contains(order.Length - tmp.Length + tmp.IndexOf(t.ToString())))
                    {
                        index.Add(order.Length - tmp.Length + tmp.IndexOf(t.ToString()));
                    }
                    else
                    {
                        tmp = Utils.RSplit(tmp, t.ToString());
                    }
                }
            }

            Array.Sort( index.ToArray(),results);

            return results.ToList();

        }

        public static List<List<T>> PowerSet<T>(List<T> list, string constraint = "")
        {

            string tmp;
            bool match = true;
            List<List<T>> results = new List<List<T>>();
            double count = Math.Pow(2, list.Count);
            for (int i = 1; i <= count - 1; i++)
            {
                string str = Convert.ToString(i, 2).PadLeft(list.Count, '0');
                List<T> combination = new List<T>();
                for (int j = 0; j < str.Length; j++)
                {
                    if (str[j] == '1')
                    {
                        combination.Add(list[j]);
                    }
                }

                //constraint
                tmp = constraint;
                match = true;
                foreach (T t in combination)
                {                    
                    if (!tmp.Contains(t.ToString()))
                    {
                        match = false;
                        break;
                    }
                    tmp = Utils.RSplit(tmp, t.ToString());
                }
                if (constraint == "" || (constraint != "" && match))
                {
                    results.Add(combination);
                }
            }

            //Remove repeated elements
            foreach (List<T> ls in results.ToArray())
            {
                results.RemoveAll(x => Utils.Stringify(x) == Utils.Stringify(ls));
                results.Add(ls);
            }


            return results;
        }


        public static List<string> ExtractFromBracket(string Source, char lbrac = '{', char rbrac = '}')
        {
            bool inBrac = false;
            int level = 0;
            string content = "";
            List<string> result = new List<string>();
            foreach (char c in Source)
            {
                if (c == lbrac)
                {
                    inBrac = true;
                    level++;
                }
                if (c == rbrac)
                {
                    level--;
                    if (level == 0 && inBrac)
                    {
                        inBrac = false;
                        result.Add(content);
                        content = "";
                    }
                }
                if (inBrac)
                {
                    content += c;
                }
            }

            return result;
        }

        public static string Stringify<T>(IEnumerable<T> list, char sep )
        {
            string result = "";

            foreach (T t in list)
            {
                result += t.ToString()+sep;
            }

            return result.TrimEnd(sep);

        }

        public static string Stringify<T>(IEnumerable<T> list)
        {
            string result = "";

            foreach (T t in list)
            {
                result += t.ToString();
            }

            return result;

        }

        /// <summary>
        /// Replace all appearances of the series represented by reactant with the element product
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list</param>
        /// <param name="reactant">Series that has to be replaced</param>
        /// <param name="product">Element to replace with</param>
        /// <returns>Replaced list</returns>
        public static List<T> Substitute<T>(IEnumerable<T> list, IEnumerable<T> reactant, T product)
        {
            int currRectant = 0;
            bool matching = false;
            List<T> result= new List<T>();

            //There are two pointers here, 
            // i being the pointer into the list
            // currReactant being the pointer into the reactant series

            //Run through the list
            for (int i=0;i< list.Count();i++)
            {
                //If the two pointers match
                //Set the flag to be true
                //And move the pointer in the reactant
                if (list.ElementAt(i).Equals(reactant.ElementAt(currRectant)))
                {
                    matching = true;
                    currRectant++;
                    if (currRectant == reactant.Count())
                    {
                        result.Add(product);
                        currRectant = 0;
                        matching = false;
                    }
                }
                //If the two pointers do not match
                else
                {
                    //The first situation will be that we the two pointers were matching
                    //The current unmatch breaks the match
                    //We add back previously skipped elements
                    //And keep the pointer at the same place (by i-- and i++ in the for loop)
                    //  because the current list pointer can be the begining of a new match
                    if (matching)
                    {
                        matching = false;
                        // add back the skipped elements
                        if (currRectant > 0)
                        {
                            for (; currRectant > 0; currRectant--)
                            {
                                result.Add(list.ElementAt(i - currRectant));
                            }
                        }
                        currRectant = 0;
                        i--;
                    }
                    else
                    {
                        result.Add(list.ElementAt(i));
                    }
                }
            }

            // add back the skipped elements
            if (currRectant > 0)
            {
                for (; currRectant > 0; currRectant--)
                {
                    result.Add(list.ElementAt(list.Count() - currRectant));
                }
            }

            return result;
        }


    }
}
