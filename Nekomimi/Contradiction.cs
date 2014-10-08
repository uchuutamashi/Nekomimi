using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nekomimi
{
    static class Contradiction
    {
        /// <summary>
        /// Throw a Contradiction.
        /// </summary>
        /// <param name="obj"></param>
        static public void Throw(object obj)
        {
            // Conceptual Contradiction
            //---------------------------------------- 
            Concept[] pairs = obj as Concept[];

            if ((object)pairs != null)
            {
                // TODO
            }
            //----------------------------------------

        }

    }
}
