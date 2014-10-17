using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nekomimi
{
    static class CParser
    {
        public static List<Hypothesis<List<Concept>>> Parse(string Source)
        {
            List<Concept> lConcept = ConceptBase.Extract(Source);
            //DEBUG
            lConcept.Add(new Concept("xy","NODETYPE"));
            List<Hypothesis<List<Concept>>> CurrentLevel = Hypothesis<List<Concept>>.Hypothesize(Utils.PowerSet(USieve.Filter(Utils.Order(lConcept, Source)), Source), list =>
            {
                return (double)Utils.Stringify(list).Length / (double)Source.Length;   
            });

            List<Reaction> Reactions= new List<Reaction>();
            List<Hypothesis<List<Concept>>> NextLevel= new List<Hypothesis<List<Concept>>>();

            for(int i =0 ; i<50;i++)
            {
                // Get all applicable rules and store the reactions for later use
                Reactions.Clear();
                foreach (Hypothesis<List<Concept>> hyp in CurrentLevel)
                {
                    foreach (Rule r in RuleBase.FindApplicableRules(hyp.Claim))
                    {
                        Reaction re = new Reaction(hyp.Claim.ToArray(), r.Apply(hyp.Claim));
                        if (!Reactions.Contains(re))
                        {
                            Reactions.Add(re);
                        }
                    }
                }

                NextLevel.Clear();
                foreach (Hypothesis<List<Concept>> hyp in CurrentLevel)
                {
                    foreach (Reaction re in Reactions)
                    {
                        if (!Utils.Identical(Utils.Substitute(hyp.Claim, re.Reactants, re.Product), hyp.Claim))
                        {
                            NextLevel.Add(new Hypothesis<List<Concept>>(Utils.Substitute(hyp.Claim, re.Reactants, re.Product), hyp.Certainty));
                        }
                    }
                }

                if (NextLevel.Count() == 0)
                {
                    break;
                }
                else
                {
                    CurrentLevel = new List<Hypothesis<List<Concept>>>(NextLevel);
                }
                
            }

            //DEBUG===========================================
            //Rule ru = Rule.FromString("*+*,TESTTYPE,none");
            foreach (Hypothesis<List<Concept>> hyp in CurrentLevel)
            {
                foreach (Concept c in hyp.Claim)
                {
                    Console.Write(c + " ");
                }
                Console.WriteLine(">>> " + hyp.Certainty);

                Console.WriteLine();
            }
            //================================================

            return CurrentLevel;
        }

    }
}
