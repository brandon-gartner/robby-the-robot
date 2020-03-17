using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbyGeneticAlgo
{
    public class Chromosome : IComparable
    {
        private Allele[] alleles;

        public Chromosome(int length)
        {
            //TO DO
        }

        public Chromosome(Allele[] gene)
        {
            //TO DO
        }

        public Chromosome[] Reproduce(Chromosome spouse, Crossover f, double mutationRate)
        {
            //TO DO
            throw new NotImplementedException();
        }

        public void EvalFitness(Fitness f)
        {
            //TO DO
        }
        
        public Allele this[int index]
        {
            get { return alleles[index]; }
        }
        
        public double Fitness
        {
            get { throw new NotImplementedException(); }

            private set { throw new NotImplementedException(); }
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

    }
}
