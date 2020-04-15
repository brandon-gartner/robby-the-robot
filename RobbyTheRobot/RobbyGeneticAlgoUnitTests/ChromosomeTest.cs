using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobbyGeneticAlgo;

namespace RobbyGeneticAlgoUnitTests
{
    [TestClass]
    public class ChromosomeTest
    {
        
        //I decided not to test the length constructor because if I can't trust C# to make an array of the size I tell it to, the language would be in shambles
        //and entirely non-functional

        [TestMethod]
        public void TestAlleleArrayChromosomeConstructor()
        {
            //I made this much longer than i should have, but overall just tests the accuracy of the allele constructor
            Allele[] testAlleles = new Allele[14];
            testAlleles[0] = Allele.North;
            testAlleles[1] = Allele.South;
            testAlleles[2] = Allele.East;
            testAlleles[3] = Allele.West;
            testAlleles[4] = Allele.Nothing;
            testAlleles[5] = Allele.PickUp;
            testAlleles[6] = Allele.Random;
            testAlleles[7] = Allele.Random;
            testAlleles[8] = Allele.PickUp;
            testAlleles[9] = Allele.Nothing;
            testAlleles[10] = Allele.West;
            testAlleles[11] = Allele.East;
            testAlleles[12] = Allele.South;
            testAlleles[13] = Allele.North;

            Chromosome testChromo = new Chromosome(testAlleles);
            for (int i = 0; i < 14; i++)
            {
                switch (i)
                {
                    case 0:
                    case 13:
                        Assert.AreEqual(testChromo[i], Allele.North);
                        break;

                    case 1:
                    case 12:
                        Assert.AreEqual(testChromo[i], Allele.South);
                        break;

                    case 2:
                    case 11:
                        Assert.AreEqual(testChromo[i], Allele.East);
                        break;

                    case 3:
                    case 10:
                        Assert.AreEqual(testChromo[i], Allele.West);
                        break;

                    case 4:
                    case 9:
                        Assert.AreEqual(testChromo[i], Allele.Nothing);
                        break;

                    case 5:
                    case 8:
                        Assert.AreEqual(testChromo[i], Allele.PickUp);
                        break;

                    case 6:
                    case 7:
                        Assert.AreEqual(testChromo[i], Allele.Random);
                        break;

                }

            }
        }

        //tests whether the reproduce method works properly when the mutationRate is equal to 0
        [TestMethod]
        public void TestReproduce()
        {
            Allele[] a = { Allele.North, Allele.South, Allele.Random, Allele.North, Allele.South };
            Allele[] b = { Allele.East, Allele.West, Allele.Nothing, Allele.East, Allele.West };
            Allele[] c = { Allele.East, Allele.West, Allele.Nothing, Allele.North, Allele.South };
            Allele[] d = { Allele.North, Allele.South, Allele.Random, Allele.East, Allele.West };

            Chromosome first = new Chromosome(a);
            Chromosome second = new Chromosome(b);
            Chromosome[] children = first.Reproduce(second, first.DoubleCrossover, 0);
            Chromosome[] testChildren = { new Chromosome(c), new Chromosome(d) };
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Assert.AreEqual(children[i][j], testChildren[i][j]);
                }
            }
        }

        //tests whether the resulting children are different than what they would be, if we set mutationrate to .5
        [TestMethod]
        public void TestReproduceFail()
        {
            Allele[] a = { Allele.North, Allele.South, Allele.Random, Allele.North, Allele.South };
            Allele[] b = { Allele.East, Allele.West, Allele.Nothing, Allele.East, Allele.West };
            Allele[] c = { Allele.East, Allele.West, Allele.Nothing, Allele.North, Allele.South };
            Allele[] d = { Allele.North, Allele.South, Allele.Random, Allele.East, Allele.West };

            Chromosome first = new Chromosome(a);
            Chromosome second = new Chromosome(b);
            Chromosome[] children = first.Reproduce(second, first.DoubleCrossover, .5);
            Chromosome[] testChildren = { new Chromosome(c), new Chromosome(d) };
            for (int i = 0; i < 2; i++)
            {
                Assert.AreNotEqual(children[i], testChildren[i]);
            }
        }

        //tests whether or not the EvalFitness works, using a custom Fitness method
        //found at the bottom of this class
        [TestMethod]
        public void TestEvalFitness()
        {
            Allele[] testAlleles = { Allele.North };
            Chromosome testChromosome = new Chromosome(testAlleles);
            testChromosome.EvalFitness(FitnessBasedOnAlleleLiterals);
            Assert.AreEqual(testChromosome.Fitness, 0);
        }


        //tests the indexer.  this test is probably unnecessary
        [TestMethod]
        public void TestIndexer()
        {
            Allele[] testAllele = new Allele[4];
            testAllele[1] = (Allele)1;
            testAllele[3] = (Allele)3;

            Chromosome testChromo = new Chromosome(testAllele);
            Assert.AreEqual(testChromo[1], testAllele[1]);
            Assert.AreEqual(testChromo[3], testChromo[3]);
        }

        //tests whether converting a custom chromosome to a string creates the string that we expect
        [TestMethod]
        public void TestToString()
        {
            Allele[] testAllele = new Allele[2];
            testAllele[0] = Allele.North;
            testAllele[1] = Allele.Random;

            Chromosome testChromo = new Chromosome(testAllele);
            string asdf = testChromo.ToString();
            System.Diagnostics.Debug.WriteLine(asdf);
            Assert.AreEqual(asdf, "North,Random,");
        }

        //since my compareTo is simple I didn't write a test case for if both are chromosomes


        //this will fail if you try to compare two objects, at least one of which is not a chromosome
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCompareToNotChromosome()
        {
            Chromosome a = new Chromosome(4);
            string b = "asdasdasdasd";
            a.CompareTo(b);
        }


        //this has been tested by hand, fails if singleCrossover doesn't generate the appropriate
        //random numbers.  currently, it does, and so this then is able to test whether or not singleCrossover works or not
        [TestMethod]
        public void TestSingleCrossover()
        {
                Allele[] a = { Allele.North, Allele.South, Allele.Random };
                Allele[] b = { Allele.East, Allele.West, Allele.Nothing };

                Chromosome first = new Chromosome(a);
                Chromosome second = new Chromosome(b);
                Chromosome[] children = first.SingleCrossover(first, second);
                Allele[] c = { Allele.North, Allele.South, Allele.Nothing };
                Allele[] d = { Allele.East, Allele.West, Allele.Random };
;               Chromosome[] testChildren = { new Chromosome(c), new Chromosome(d) };
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Assert.AreEqual(children[i][j], testChildren[i][j]);
                    }
                }
        }


        //this has been tested by hand, fails if doubleCrossover doesn't generate the appropriate
        //random numbers.  currently, it does, and so this then is able to test whether or not doubleCrossover works or not
        [TestMethod]
        public void TestDoubleCrossover()
        {
            Allele[] a = { Allele.North, Allele.South, Allele.Random, Allele.North, Allele.South };
            Allele[] b = { Allele.East, Allele.West, Allele.Nothing, Allele.East, Allele.West };

            Helpers.rand.Next(2);

            Chromosome first = new Chromosome(a);
            Chromosome second = new Chromosome(b);
            Chromosome[] children = first.DoubleCrossover(first, second);
            Allele[] c = { Allele.East, Allele.West, Allele.Nothing, Allele.North, Allele.South };
            Allele[] d = { Allele.North, Allele.South, Allele.Random, Allele.East, Allele.West };
            Chromosome[] testChildren = { new Chromosome(c), new Chromosome(d) };
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Assert.AreEqual(children[i][j], testChildren[i][j]);
                }
            }
        }

        //helper method to help test the evalFitness.  returns a certain fitness, based on the first allele of any chromosome
        public double FitnessBasedOnAlleleLiterals(Chromosome c)
        {
            switch (c[0])
            {
                case Allele.North:
                    return 0;
                case Allele.South:
                    return 1;
                case Allele.East:
                    return 2;
                case Allele.West:
                    return 3;
                case Allele.Nothing:
                    return 4;
                case Allele.PickUp:
                    return 5;
                case Allele.Random:
                    return 6;
                default:
                    return -1;
            }
        }
    }
}

