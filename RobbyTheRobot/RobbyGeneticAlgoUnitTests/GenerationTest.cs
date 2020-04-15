using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobbyGeneticAlgo;


namespace RobbyGeneticAlgoUnitTests
{
    [TestClass]
    public class GenerationTest
    {
        [TestMethod]
        public void TestNormalGenerationConstructor()
        {
            //Arrange
            int populationSize = 10;
            int numGenes = 50;

            //Act
            Generation gen = new Generation(populationSize, numGenes);

            //Assert
            Assert.IsNotNull(gen[9]);
        }

        [TestMethod]
        public void TestEvalFitness()
        {
            //Arrange
            Generation gen = new Generation(10, 50);

            //Act
            gen.EvalFitness(testFitness);

            //Assert
            Assert.AreNotEqual(gen[0], gen[1]);

        }

        //Method to give a rando double as a fitness
        public static double testFitness(Chromosome c)
        {
            return Helpers.rand.NextDouble();
        }

        [TestMethod]
        public void TestSelectParent()
        {
            //Arrange
            Generation gen = new Generation(10, 50);

            //Act
            gen.EvalFitness(testFitness);
            Chromosome c =  gen.SelectParent();
            Chromosome c2 = gen.SelectParent();

            //Assert
            Assert.AreNotEqual(c, c2);
        }
    }
}
