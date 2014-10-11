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
