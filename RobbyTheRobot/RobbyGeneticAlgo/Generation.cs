using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobbyGeneticAlgo;

namespace RobbyGeneticAlgo
{
    public class Generation
    {
        Chromosome[] chromosomes;

        public Generation(int populationSize, int numGenes)
        {
            //TO DO
        }

        public Generation(Chromosome[] members)
        {
            chromosomes = new Chromosome[members.Length];

            for (int i = 0; i < members.Length; i++)
            {
                chromosomes[i] = members[i];
            }
        }

        public void EvalFitness(Fitness f)
        {
            foreach (var c in chromosomes)
            {
                f(c);
            }

            Array.Sort(chromosomes);
            Array.Reverse(chromosomes);
        }

        public Chromosome this[int index] 
        {
            get { return chromosomes[index]; } 
        }

        public Chromosome SelectParent()
        {
            int[] nums = new int[10];

            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = Helpers.rand.Next(0, chromosomes.Length - 1);
            }

            int min = Helpers.FindMin(nums);

            return chromosomes[min];
        }
    }
}

