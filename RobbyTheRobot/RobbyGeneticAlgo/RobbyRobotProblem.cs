using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbyGeneticAlgo
{
    public class RobbyRobotProblem
    {
        private int numGenerations;
        private int popSize;
        private int numActions;
        private int numTestGrids;
        private int numGenes;
        private double eliteRate;
        private double mutationRate;
        private double highestFitness = -1000000000;
        private int highestGen = 0;
        private int gridSize;
        Contents[][,] gridContents;
        AlleleMoveAndFitness f;
        public event GenerationEventHandler GenerationReplacedEvent;
        
        private Generation currentGeneration;


        public RobbyRobotProblem(int numGenerations, int popSize, AlleleMoveAndFitness f, int numActions = 200, int numTestGrids = 100, int numGenes = 243, double eliteRate = .05, double mutationRate = .05, int gridSize = 10)
        {
            this.numGenerations = numGenerations;
            this.popSize = popSize;
            this.numActions = numActions;
            this.numTestGrids = numTestGrids;
            this.numGenes = numGenes;
            this.eliteRate = eliteRate;
            this.mutationRate = mutationRate;
            this.gridSize = gridSize;
            this.f = f;
            gridContents = new Contents[numTestGrids][,];
            
        }

        public void Start()
        {
            Generation lastGeneration;
            currentGeneration = new Generation(popSize, numGenes);
            for (int i = 0; i < numGenerations + 1; i++)
            {
                
                //call the robotProblem's eval fitness, which evaluates the fitness of the entire currentGeneration
                EvalFitness(RobbyFitness);

                //keeps track of the highest fitness and the generation with that fitnes
                if (currentGeneration[0].Fitness >= highestFitness)
                {
                    highestFitness = currentGeneration[0].Fitness;
                    highestGen = i;
                }
                lastGeneration = currentGeneration;
                GenerationReplacedEvent?.Invoke(i, lastGeneration);
                currentGeneration = GenerateNextGeneration();
                if (highestFitness != currentGeneration[0].Fitness)
                {

                    Console.WriteLine("Highest generation was thus far was: Generation " + highestGen);
                    Console.WriteLine("Highest fitness thus far was: " + highestFitness);
                    Console.WriteLine("The current generation's fitness was less than the max by: " + Math.Round(currentGeneration[0].Fitness - highestFitness, 2));
                }
                
                Console.WriteLine("---------------------------------------------------------------------");
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            System.Diagnostics.Debug.WriteLine("Final Results:");
            System.Diagnostics.Debug.WriteLine("The generation that performed the best was: Generation " + highestGen);
            System.Diagnostics.Debug.WriteLine("Its fitness was: " + highestFitness);

        }

        public void EvalFitness(Fitness f)
        {
            //fill gridContents with square rectangular array
            for (int i = 0; i < numTestGrids; i++)
            {
                gridContents[i] = Helpers.GenerateRandomTestGrid(gridSize);
            }

            //evaluates the fitness of everything in the currentGeneration
            currentGeneration.EvalFitness(RobbyFitness);
        }


        //creates the next generation
        public Generation GenerateNextGeneration()
        {
            //figure out how many are "elite", how many to ignore when reproducing
            int eliteCount = EliteCountDecider(popSize, eliteRate);

            //creating the chromosomes array
            Chromosome[] chromosomes = new Chromosome[popSize];

            //copying all of the elite chromosomes from currentGeneration into the new chromosomes array
            for (int i = 0; i < eliteCount; i++)
            {
                chromosomes[i] = currentGeneration[i];
            }

            //figures out a number called randomMax, which is the highest number we will randomize to.
            //randomMax is equal to the population size + the eliteCount.
            for (int i = 0; i < (popSize - eliteCount) / 2; i++)
            {
                Chromosome firstParent = currentGeneration.SelectParent();
                Chromosome secondParent = currentGeneration.SelectParent();

                //reproduce, and then copy over the values into the chromosome array, to positions that make sense mathematically
                Chromosome[] tempChromoArr = firstParent.Reproduce(secondParent, firstParent.DoubleCrossover, this.mutationRate);
                chromosomes[eliteCount + 2 * i] = tempChromoArr[0];
                chromosomes[eliteCount + (2 * i) + 1] = tempChromoArr[1];
            }

            //create a generation ouot of the array of chromosomes created.
            Generation nextGen = new Generation(chromosomes);
            return nextGen;
        }

        //decides how many elite chromosomes there are.
        public int EliteCountDecider(int popSize, double eliteRate)
        {
            //tries to create the eliteCount as populationSize * eliteRate
            int eliteCount = (int)(popSize * eliteRate);

            //if it's odd, make it even
            if (eliteCount % 2 == 1)
            {
                //if the eliteCount + 1 isn't greater than the population size, increase it
                if (eliteCount + 1 < popSize)
                {
                    eliteCount++;
                }

                //otherwise, if the eliteCount - 1 is still greater than 0, subtract 1 from it
                else if (eliteCount - 1 > 0)
                {
                    eliteCount--;
                }

                //if neither, throw an exception telling people to not use a population of two
                else
                {
                    throw new ArgumentException("Please don't use a population size of 2, 1, or 0.");
                }
            }

            //return whatever was decided above.
            return eliteCount;
        }

        public double RobbyFitness(Chromosome c)
        {

            int sum = 0;
            double avg = 0;

            //run robby through the grid multiple times, finding his score each time. 
            for (int i = 0; i < this.numTestGrids; i++)
            {
                sum += Helpers.RunRobbyInGrid(gridContents[i], c, this.numActions, f);
            }

            //average out his scores, and return the average
            avg = ((double)sum / numTestGrids);
            return avg;
        }



    }
}
