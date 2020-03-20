using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobbyGeneticAlgo;

namespace RobbyGeneticAlgo
{
    /// <summary>
    /// Generation Class
    /// An array of Chromosomes at a giver generation
    /// </summary>
    public class Generation
    {
        Chromosome[] chromosomes;

        /// <summary>
        /// Constructor to unitialize a generation with a random set of chromosomes
        /// </summary>
        /// <param name="populationSize"> the size of the population (Size of the array </param>
        /// <param name="numGenes"> Number of genes for each chromosome </param>
        public Generation(int populationSize, int numGenes)
        {
            chromosomes = new Chromosome[populationSize];
            for (int i = 0; i < populationSize; i++)
            {
                chromosomes[i] = new Chromosome(numGenes);
            }
        }

        /// <summary>
        /// Constructor that takes an array of memebers and initializes a generation based of it
        /// </summary>
        /// <param name="members"> The members to form the new Generation </param>
        public Generation(Chromosome[] members)
        {
            chromosomes = new Chromosome[members.Length];

            for (int i = 0; i < members.Length; i++)
            {
                chromosomes[i] = members[i];
            }
        }

        /// <summary>
        /// Method that takes the Fitness of all chromosomes in the generation and sort the array from biggest to smallest
        /// </summary>
        /// <param name="f"> the fucntion Fitness that we want to apply </param>
        public void EvalFitness(Fitness f)
        {
            foreach (var c in chromosomes)
            {
                f(c);
            }

            //Sort the arry ascending
            Array.Sort(chromosomes);
            //Reverse the array so its from biggest to smallest
            Array.Reverse(chromosomes);
        }

        /// <summary>
        /// Index to be able to access the chromosome array outside the class
        /// </summary>
        /// <param name="index"> an Int for the index/position </param>
        /// <returns></returns>
        public Chromosome this[int index] 
        {
            get { return chromosomes[index%chromosomes.Length]; } 
        }

        /// <summary>
        /// Method to select the parent of the next generation. 
        /// It returns the top fittest within 10 random chromosomes
        /// </summary>
        /// <returns> Top fittest within 10 random chromosomes </returns>
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

