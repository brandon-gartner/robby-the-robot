using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbyGeneticAlgo
{
    [Serializable]
    public class Chromosome : IComparable
    {
        private Allele[] alleles;
        private double fitnessScore;

        public Chromosome(int length)
        {
            //creating the array and finding the enum quantity
            alleles = new Allele[length];
            int enumQuantity = Enum.GetNames(typeof(Allele)).Length;

            //filling the array with random enums
            for (int i = 0; i < alleles.Length; i++)
            {
                alleles[i] = (Allele)Helpers.rand.Next(enumQuantity);
            }
        }

        public Chromosome(Allele[] gene)
        {
            //instantiates the allele array, and copies each value over individually
            alleles = new Allele[gene.Length];
            for (int i = 0; i < alleles.Length; i++)
            {
                alleles[i] = gene[i];
            }
        }

        public Chromosome[] Reproduce(Chromosome spouse, Crossover f, double mutationRate)
        {
            //creates a child array of chromosomes, based on the Crossover member method
            Chromosome[] children = f(this, spouse);

            //goes through each allele of each child, generating a random number to decide if the allele should be randomized (based on mutation rate)
            //if it is decided that it should be randomized, that allele become a random allele
            for (int i = 0; i < children.Length; i++)
            {
                for (int j = 0; j < children[i].alleles.Length; j++)
                {
                    double randomComparison = Helpers.rand.NextDouble();
                    if (randomComparison < mutationRate)
                    {
                        children[i].alleles[j] = (Allele)Helpers.rand.Next(Enum.GetNames(typeof(Allele)).Length);
                    }
                }
            }
            return children;
        }

        public void EvalFitness(Fitness f)
        {
            //sets this object's fitness to be the result of the Fitness delegte method
            this.Fitness = f(this);
        }

        public Allele this[int index]
        {
            //allows us to easily check the allele at any index of this chromosome
            get { return alleles[index]; }
        }

        public double Fitness
        {
            //is a property that uses the fitnessScore private property as a backing field
            get { return this.fitnessScore; }

            private set { this.fitnessScore = value; }
        }

        public override string ToString()
        {
            //loops through all the alleles of a chromosome, adding them and a comma to the string every time.
            //returns the string at the end.
            string toConcatenateTo = "";
            for (int i = 0; i < this.alleles.Length; i++)
            {
                toConcatenateTo += this[i] + ",";
            }

            return toConcatenateTo;
        }

        public int CompareTo(object obj)
        {
            if (obj is Chromosome)
            {
                //if the given object evaluates to a chromosome, compare it to this object
                Chromosome tempChromo = obj as Chromosome;
                return (int)(this.Fitness - tempChromo.Fitness);
            }
            else
            {
                //if the object is not a chromosome, throw an exception
                throw new ArgumentException("Attempted to compare a Chromosome to a non-Chromosome");
            }
        }

        public Chromosome[] SingleCrossover(Chromosome a, Chromosome b)
        {
            //invokes singlehelper to do most of the heavy lifting for singleCrossover
            Chromosome[] children = new Chromosome[2];
            int splitLocation = Helpers.rand.Next(alleles.Length);

            children[0] = SingleHelper(a, b, splitLocation);
            children[1] = SingleHelper(b, a, splitLocation);

            return children;
        }

        public Chromosome SingleHelper(Chromosome partThatGoesFirst, Chromosome partThatGoesSecond, int splitLocation)
        {
            Allele[] tempAlleles = new Allele[alleles.Length];

            //for each number, check if it is before or after the split.  if it's before, copy from partThatGoesFirst
            //if it's at or after, copy from partThatGoesSecond
            for (int i = 0; i < alleles.Length; i++)
            {
                if (i < splitLocation)
                {
                    tempAlleles[i] = partThatGoesFirst[i];
                }
                else
                {
                    tempAlleles[i] = partThatGoesSecond[i];
                }
            }

            //create and return a chromosome made out of the alleles copied before
            Chromosome tempChromo = new Chromosome(tempAlleles);
            return tempChromo;
        }

        public Chromosome[] DoubleCrossover(Chromosome a, Chromosome b)
        {
            //figures out two points where it should split, the invokes doubleHelper to do the heavy lifting
            int midPoint = ((alleles.Length) / 2);
            int firstSplit = Helpers.rand.Next(midPoint);
            int secondSplit = Helpers.rand.Next(midPoint + 1, alleles.Length);
            Chromosome[] children = new Chromosome[2];
            children[0] = DoubleHelper(a, b, firstSplit, secondSplit);
            children[1] = DoubleHelper(b, a, firstSplit, secondSplit);

            return children;
        }

        public Chromosome DoubleHelper(Chromosome original, Chromosome newSectionOrigin, int firstSplit, int secondSplit)
        {
            Allele[] tempAlleles = new Allele[alleles.Length];

            //if the number we're at is less than firstSplit, or equal/more than secondSplit, copy from the "original" chromosome
            //if the number we're at is less than secondSplit but more than firstSplit, copy from "newSectionOrigin" chromosome
            for (int i = 0; i < alleles.Length; i++)
            {
                if (i < firstSplit || i >= secondSplit)
                {
                    tempAlleles[i] = original[i];
                }
                else
                {
                    tempAlleles[i] = newSectionOrigin[i];
                }
            }

            //create a new chromosome out of the allele generated above, and return it
            Chromosome tempChromo = new Chromosome(tempAlleles);
            return tempChromo;
        }
    }
}
