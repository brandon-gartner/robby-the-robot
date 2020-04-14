using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobbyGeneticAlgo;

namespace RobbyGeneticAlgoUnitTests
{
    [TestClass]
    public class ChromosomeTest
    {
        [TestMethod]
        public void TestAlleleArrayChromosomeConstructor()
        {
            //using some probably way too long code, assigns 2x of every allele, creates a chromosome out of them, and then checks all of them
            //to make sure that it all worked properly
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
        [TestMethod]
        public void TestReproduce()
        {
            //TODO, still need to figure out how to test random values.
        }

        [TestMethod]
        public void TestEvalFitness()
        {
            //TODO
        }

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

        [TestMethod]
        public void TestToString()
        {
            Allele[] testAllele = new Allele[2];
            testAllele[0] = Allele.North;
            testAllele[1] = Allele.Random;

            Chromosome testChromo = new Chromosome(testAllele);
            string asdf = testChromo.ToString();

            Assert.AreEqual(asdf, "Allele.North,Allele.Random");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCompareToNotChromosome()
        {
            Chromosome a = new Chromosome(4);
            string b = "asdasdasdasd";
            a.CompareTo(b);
        }

        [TestMethod]
        public void TestSingleCrossover()
        {
            //TODO
        }

        [TestMethod]
        public void TestDoubleCrossover()
        {
            //TODO
        }
    }
}
