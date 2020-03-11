using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbyGeneticAlgo
{
    public class Generation
    {
        Chromosome[] chromosomes;

        public Generation(int populationSize, int numGenes)
        {
            //TO DO
        }

        public Generation( Chromosome[] members)
        {
            chromosomes = new Chromosome[members.Length];

            for (int i = 0; i < members.Length; i++)
            {
                chromosomes[i] = members[i];
            }
        }

        public void EvalFitness(Fitness f)
        {
            //TO DO
        }

        public Chromosome this[int index] 
        {
            get { return chromosomes[index]; } 
        }

        public Chromosome SelectParents()
        {
            //TO DO
            throw new NotImplementedException();
        }
    }
}
