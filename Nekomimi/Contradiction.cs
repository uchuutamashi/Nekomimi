using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nekomimi
{
    static class Contradiction
    {
        public enum ERRCODE { DIRECT, INHERITANCE };


        /// <summary>
        /// Throw a Contradiction.
        /// </summary>
        /// <param name="obj"></param>
        static public void Throw(ERRCODE err, object obj)
        {
            switch (err)
            {
                case ERRCODE.DIRECT:
                    // Direct Contradiction
                    // e.g. Apple is red VS Apple is blue
                    //---------------------------------------- 
                    Concept[] pairs = obj as Concept[];

                    if ((object)pairs != null)
                    {
                        // TODO
                    }
                    //----------------------------------------
                    break;
                case ERRCODE.INHERITANCE:
                    // Inheritance Contradiction
                    // e.g. Foo is an apple VS Foo is blue
                    //---------------------------------------- 
                    pairs = obj as Concept[];

                    if ((object)pairs != null)
                    {
                        // TODO
                    }
                    //----------------------------------------
                    break;
                default:
                    break;

            }
        }

    }
}
