﻿using System;
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
        Contents[][,] gridContents;
        AlleleMoveAndFitness f;
        public event GenerationEventHandler GenerationReplacedEvent;

        //CONCERN: is the below allowed?
        Generation currentGeneration;


        public RobbyRobotProblem(int numGenerations, int popSize, AlleleMoveAndFitness f, int numActions = 200, int numTestGrids = 100, int numGenes = 243, double eliteRate = .05, double mutationRate = .05)
        {
            this.numGenerations = numGenerations;
            this.popSize = popSize;
            this.numActions = numActions;
            this.numTestGrids = numTestGrids;
            this.numGenes = numGenes;
            this.eliteRate = eliteRate;
            this.mutationRate = mutationRate;
            this.f = f;
            gridContents = new Contents[numTestGrids][,];
            
        }

        public void Start()
        {
            Generation lastGeneration;
            currentGeneration = new Generation(popSize, numGenes);
            for (int i = 0; i < numGenerations; i++)
            {
                
                //call the robotProblem's eval fitness, which evaluates the fitness of the entire currentGeneration
                EvalFitness(RobbyFitness);

                //saves your lastGeneration in case it is needed later
                //WARNING: before submitting, decide if this is needed
                if (currentGeneration[0].Fitness >= highestFitness)
                {
                    highestFitness = currentGeneration[0].Fitness;
                    highestGen = i;
                }
                lastGeneration = currentGeneration;
                //CONCERN: should the event be invoked before or after generating the new generation?
                GenerationReplacedEvent?.Invoke(i, lastGeneration);
                currentGeneration = GenerateNextGeneration();
                Console.WriteLine("Highest generation was thus far was: Generation " + highestGen);
                Console.WriteLine("Highest fitness thus far was: " + highestFitness);
                if (highestFitness != currentGeneration[0].Fitness)
                {
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
                gridContents[i] = Helpers.GenerateRandomTestGrid(10);
            }

            //evaluates the fitness of everything in the currentGeneration
            currentGeneration.EvalFitness(RobbyFitness);
        }


        //written late at night with little sleep.  EXPECT BUGS
        public Generation GenerateNextGeneration()
        {
            //figure out how many are "elite", how many to ignore when reproducing
            int eliteCount = EliteCountDecider(popSize, eliteRate);
            int ignoreCount = (int)(popSize * .1);

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
                //WARNING: the chromosome placement in the array may or may not work
                Chromosome[] tempChromoArr = firstParent.Reproduce(secondParent, firstParent.DoubleCrossover, this.mutationRate);
                chromosomes[eliteCount + 2 * i] = tempChromoArr[0];
                chromosomes[eliteCount + (2 * i) + 1] = tempChromoArr[1];
            }

            //create a generation ouot of the array of chromosomes created.
            Generation nextGen = new Generation(chromosomes);
            return nextGen;
        }

      
        public int WeightedChromosomeSelector(int popSize, int randomMax, int eliteCount, int ignoreCount) 
        {
            
            //declare numToAccess, initialize randomGen as an int less than randomMax
            int numToAccess = 0;
            int randomGen = Helpers.rand.Next(randomMax);
            bool ignored = true;
            //if it is part of the section we ignore (greater than popSize + elites - ignore)
            while (ignored)
            {
                if (randomGen > (popSize + eliteCount - ignoreCount))
                {
                    randomGen = Helpers.rand.Next(randomMax);
                }
                //if randomGen is less than double the eliteCount, divide the number by 2 and use that as the numToAccess
                else if (randomGen < (2 * eliteCount))
                {
                    numToAccess = randomGen / 2;
                    ignored = false;
                }
                //if its not part of the section to ignore, and its not within the extended elite count, subtract the eliteCount
                //to figure out what the number would have been originally
                else
                {
                    numToAccess = (randomGen - eliteCount);
                    ignored = false;
                }
            }
            
            return numToAccess;
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
