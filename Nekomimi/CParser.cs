using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nekomimi
{
    static class CParser
    {
        public static List<Hypothesis> Parse(string Source)
        {
            List<Concept> lConcept = ConceptBase.Extract(Source);

            

            List<List<Concept>> BigTable = Utils.PowerSet(Utils.Order(lConcept, Source),Source);


            foreach (List<Concept> list in BigTable)
            {
                foreach (Concept c in list)
                {
                    Console.Write(c+" ");
                }
                Console.WriteLine();
            }

            return null;
        }

    }
}
