using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nekomimi
{
    class Hypothesis<T>
    {
        T mClaim;
        double mCertainty;

        public Hypothesis(T claim, double certainty)
        {
            mClaim = claim;
            mCertainty = certainty;
        }       

        public T Claim
        {
            get
            {
                return mClaim;
            }
        }

        public double Certainty
        {
            get
            {
                return mCertainty;
            }
        }

        static public List<Hypothesis<T>> Hypothesize(List<T> list, Func<T,double> FCertainty)
        {
            List<Hypothesis<T>> results= new List<Hypothesis<T>>();
            foreach (T t in list)
            {
                results.Add(new Hypothesis<T>(t, FCertainty(t)));
            }

            return results;

        }
    }
}
