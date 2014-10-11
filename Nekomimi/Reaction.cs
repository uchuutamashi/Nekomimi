using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nekomimi
{

    /// <summary>
    /// A data structure that is used to temporarily store the reactant and result of a rule-application(reaction).
    /// </summary>
    class Reaction
    {
        Concept[] mReactants;
        Concept mProduct;

        public Reaction(Concept[] reactants, Concept prod)
        {
            mReactants = reactants;
            mProduct = prod;
        }

        public Concept[] Reactants
        {
            get{
            return mReactants;
            }
        }

        public Concept Product
        {
            get
            {
                return mProduct;
            }
        }
    }
}
