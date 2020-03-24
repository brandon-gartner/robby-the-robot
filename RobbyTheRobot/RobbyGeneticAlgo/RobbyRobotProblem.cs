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
        Contents[,] gridContents;

        public RobbyRobotProblem(int numGenerations, int popSize, AlleleMoveAndFitness f, int numActions, int numTestGrids, int numGenes, double eliteRate, double mutationRate)
        {
            this.numGenerations = numGenerations;
            this.popSize = popSize;
            this.numActions = numActions;
            this.numTestGrids = numTestGrids;
            this.numGenes = numGenes;
            this.eliteRate = eliteRate;
            this.mutationRate = mutationRate;
        }

        public void Start()
        {
            Generation lastGeneration;
            Generation currentGeneration = new Generation(popSize, numGenes);
            for (int i = 0; i < numGenerations; i++)
            {
                //TODO
            }

        }

        public void EvalFitness(Fitness f)
        {
            //TO DO
        }

        public void GenerateNextGeneration()
        {
            //TO DO
        }

        public double RobbyFitness(Chromosome c)
        {
            //TO DO
            throw new NotImplementedException();
        }

        //Missing the GenerationReplaced event
    }
}
