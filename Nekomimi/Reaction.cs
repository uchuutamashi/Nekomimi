using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nekomimi
{
    class Reaction
    {
        public Concept[] mReactants;
        public Concept mProduct;

        public Reaction(Concept[] reactants, Concept prod)
        {
            mReactants = reactants;
            mProduct = prod;

        }


    }
}
