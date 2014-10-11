using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nekomimi
{
    class Hypothesis
    {
        string mClaim;
        double mCertainty;

        public Hypothesis(string claim, double certainty)
        {
            mClaim = claim;
            mCertainty = certainty;
        }

        public string Claim()
        {
            return mClaim;
        }

        public double Certainty()
        {
            return mCertainty;
        }
    }
}
