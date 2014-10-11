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
            List<List<Concept>> CurrentLevel = Utils.PowerSet(Utils.Order(lConcept, Source),Source);

            List<Reaction> Reactions= new List<Reaction>();
            List<List<Concept>> NextLevel= new List<List<Concept>>();

            for(int i =0 ; i<50;i++)
            {
                // Get all applicable rules and store the reactions for later use
                Reactions.Clear();
                foreach (List<Concept> list in CurrentLevel)
                {
                    foreach (Rule r in RuleBase.FindApplicableRules(list))
                    {
                        Reaction re = new Reaction(list.ToArray(), r.Apply(list));
                        if (!Reactions.Contains(re))
                        {
                            Reactions.Add(re);
                        }
                    }
                }

                NextLevel.Clear();
                foreach (List<Concept> list in CurrentLevel)
                {
                    foreach (Reaction re in Reactions)
                    {
                        NextLevel.Add(Utils.Substitute(list, re.mReactants, re.mProduct));
                    }
                }

                if (NextLevel.Count() == 0)
                {
                    break;
                }
                else
                {
                    CurrentLevel = new List<List<Concept>>(NextLevel);
                }
                
            }

            //DEBUG===========================================
            Rule ru = Rule.FromString("*+*,TESTTYPE,none");
            foreach (List<Concept> list in CurrentLevel)
            {
                foreach (Concept c in list)
                {
                    Console.Write(c+" ");
                }
                Console.WriteLine(">>> " + ru.IsMatch(list));
                
                Console.WriteLine();
            }
            //================================================

            return null;
        }

    }
}
