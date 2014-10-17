using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nekomimi
{
    /// <summary>
    /// Internal Form Parser
    /// Parser for internal forms that add/modify rules/concepts 
    /// Perform contradiction checks!
    /// 
    /// e.g.
    /// [TYPE]A.COLOR=RED  (TYPE:KNOWLEDGE, update A with the specific type, if A not exist, create A)
    /// NOUN+NOUN,NOUN,{0}{1} (TYPE:RULE, if rule exists it is updated)
    /// 
    /// </summary>
    static class IParser
    {
        static public void Parse(Concept IForm)
        {
            if (IForm.Type == "KNOWLEDGE")
            {
                string lhs = Utils.LSplit(IForm.Name, "=");
                string val = Utils.RSplit(IForm.Name, "=");

                string target = Utils.LSplit(lhs, ".");
                string prop = Utils.RSplit(lhs, ".");

                var tmp = Utils.ExtractFromBracket(target, '[', ']');
                string type = null;
                if (tmp != null)
                {
                    type = tmp[0];
                    target.Replace("[" + type + "]", "");

                }

                if (type == null)
                {
                    Concept c = ConceptBase.Find(target);
                    if (c == null)
                    {
                        c = new Concept(target);
                        c.SetProperty(prop, val);
                        ConceptBase.Add(c);
                    }
                    else
                    {
                        c.SetProperty(prop, val);
                    }
                }
                else
                {
                    Concept c = ConceptBase.Find(target, type);
                    if (c == null)
                    {
                        c = new Concept(target, type);
                        c.SetProperty(prop, val);
                        ConceptBase.Add(c);
                    }
                    else
                    {
                        c.SetProperty(prop, val);
                    }
                }

            }
            else if (IForm.Type == "RULE")
            {
                Rule r = Rule.FromString(IForm.Name);
                RuleBase.Remove(r.GetPattern());
                RuleBase.Add(r);

            }
        }

    }
}
