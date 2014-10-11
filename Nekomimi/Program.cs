using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nekomimi
{
    class Program
    {
        static void Main(string[] args)
        {
            ConceptBase.Load("concepts");
            RuleBase.Load("rules");
            

            CParser.Parse("xxxyxxxy1");

            Console.Read();

        }
    }
}
