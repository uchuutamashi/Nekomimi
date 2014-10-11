using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nekomimi
{
    class Rule
    {
        string[] mPattern;
        string mProdType;
        string mIForm;

        public Rule(string pattern, string prod, string iform)
        {
            mProdType = prod;
            mIForm = iform;
            mPattern = pattern.Split('+');
        }

        static public Rule FromString(string line){
            var pattern = Utils.LSplit(line, ",");
            var tmp = Utils.RSplit(line, ",");
            var prod = Utils.LSplit(tmp, ",");
            var iform = Utils.RSplit(tmp, ",");

            return new Rule(pattern, prod, iform);            
        }

        public string GetProdType()
        {
            return Utils.Stringify(mPattern, '+');
        }

        public bool IsMatch(List<Concept> list)
        {
            if (list.Count() != mPattern.Count()) { return false; }

            for (int i = 0; i < mPattern.Count(); i++)
            {
                // 4 Syntax:
                // 1. TYPE  (* for free match)
                // 2. {NAME}
                // 3. {PROP=VAL}
                // 4. {NAME, TYPE}

                if (mPattern[i].StartsWith("{") && mPattern[i].EndsWith("}"))
                {
                    if (mPattern[i].Contains(','))
                    {
                        //SYNTAX 4
                        string name = Utils.LSplit(mPattern[i].Trim('{', '}'), ",");
                        string type = Utils.RSplit(mPattern[i].Trim('{', '}'), ",");

                        if (list[i].Name != name || list[i].Type != type)
                        {
                            return false;
                        }
                    }
                    else if (mPattern[i].Contains('='))
                    {
                        //SYNTAX 3
                        string prop = Utils.LSplit(mPattern[i].Trim('{', '}'), "=");
                        string val = Utils.RSplit(mPattern[i].Trim('{', '}'), "=");

                        if (list[i].GetProperty(prop) != val)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //SYNTAX 2
                        if (list[i].Name != mPattern[i].Trim('{', '}'))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //SYNTAX1
                    if (list[i].Type != mPattern[i] && mPattern[i]!="*")
                    {
                        return false;
                    }
                }

            }

            return true;
        }

        public Concept Apply(List<Concept> list)
        {
            List<Concept> args = new List<Concept>();
            for (int i = 0; i < mPattern.Count(); i++)
            {
                if (mPattern[i].StartsWith("{") && mPattern[i].EndsWith("}"))
                {
                    //SYNTAX 3
                    if (mPattern[i].Contains("="))
                    {
                        args.Add(list[i]);
                    }
                }
                else
                {
                    //SYNTAX 1
                    args.Add(list[i]);
                }
            }
            return new Concept(RParser.Parse(mIForm, args), mProdType);
        }


        // Equalities
        //---------------------------------------------------------------------------------------
        public static bool operator ==(Rule a, Rule b)
        {
            if ((object)a == null || (object)b == null) return false;
            return a.mPattern == b.mPattern && a.mProdType==b.mProdType && a.mIForm == b.mIForm;
        }
        public static bool operator !=(Rule a, Rule b)
        {
            if ((object)a == null ^ (object)b == null) return true;
            if ((object)a == null && (object)b == null) return false;
            return a.mPattern != b.mPattern || a.mProdType != b.mProdType || a.mIForm != b.mIForm;
        }
        public override bool Equals(object obj)
        {
            Rule a = obj as Rule;
            if ((object)a == null)
            {
                return false;
            }

            return a.mPattern == mPattern && a.mProdType == mProdType && a.mIForm == mIForm;
        }
        public bool Equals(Rule a)
        {
            if ((object)a == null) return false;
            return a.mPattern == mPattern && a.mProdType == mProdType && a.mIForm == mIForm;
        }
        //---------------------------------------------------------------------------------------

    }
}
