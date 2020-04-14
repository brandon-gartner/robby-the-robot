using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using RobbyGeneticAlgo;
using System.Net;

namespace RobbyMonogame
{
    class SimulationSprite : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Game1 game;
        private Texture2D imageBall;
        private Texture2D imageLogo;
        private Texture2D imageTile;
        private SpriteFont font;
        String[][] data;
        private int countNumMoves;
        private Contents[,] grid;
        private int robbyX;
        private int robbyY;
        private int score = 0;
        private double time;
        private int i = 0;
        private Chromosome chromosome;

        public SimulationSprite(Game1 game) : base(game)
        {
            // TODO: Construct any child components here
            this.game = game;
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.countNumMoves = 0;
            robbyX = Helpers.rand.Next(0, 10);
            robbyY = Helpers.rand.Next(0, 10);
        }

        public override void Initialize()
        {
            //Initialize the outer array to be 8
            data = new string[8][];
            String[] line;
            //String[] to hold all the paths for the files
            String[] paths = { @"..\..\..\..\..\RobbyGeneticAlgo\gen1.txt", @"..\..\..\..\..\RobbyGeneticAlgo\gen20.txt", @"..\..\..\..\..\RobbyGeneticAlgo\gen100.txt", @"..\..\..\..\..\RobbyGeneticAlgo\gen200.txt", @"..\..\..\..\..\RobbyGeneticAlgo\gen500.txt", @"..\..\..\..\..\RobbyGeneticAlgo\gen1000.txt", @"..\..\..\..\..\RobbyGeneticAlgo\gen5000.txt", @"..\..\..\..\..\RobbyGeneticAlgo\gen10000.txt" };

            //Loop to go through each file and get the information, slip it and store inside the data array
            for (int i = 0; i < paths.Length; i++)
            {
                //Get the only line from the file
                line = File.ReadAllLines(paths[i]);

                //Slipt that line on each ; and store as a String[] inside the string[][]
                data[i] = line[0].Split(';');
            }

            grid = Helpers.GenerateRandomTestGrid(10);
            chromosome = new Chromosome(AlleleArrayMaker());

            base.Initialize();
        }

        protected override void LoadContent()
        {
            imageBall = game.Content.Load<Texture2D>("ball");
            imageLogo = game.Content.Load<Texture2D>("logo");
            imageTile = game.Content.Load<Texture2D>("tile");

            font = game.Content.Load<SpriteFont>("scoreFont");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if(countNumMoves < 200)
            {
                //If statement to slow down the game
                if (time > 0.4)
                {
                    this.countNumMoves++;
                    score += Helpers.ScoreForAllele(chromosome, grid, ref robbyX, ref robbyY);
                    time = 0.0;
                }
                time += gameTime.ElapsedGameTime.TotalSeconds;

                base.Update(gameTime);
            }
            else
            {
                this.countNumMoves = 0;
                if(i+1 < data.Length)
                {
                    i++;             
                    chromosome = new Chromosome(AlleleArrayMaker());
                    grid = Helpers.GenerateRandomTestGrid(10);
                    score = 0;
                }
                else
                {
                    game.Exit();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            //Loop to draw all the tiles
            for (int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    spriteBatch.Draw(imageTile, new Rectangle(i * 32, j * 32, 32, 32), Color.White);
                }
            }

            //Loop to draw all the cans on the grid
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if(grid[i,j] == Contents.Can)
                    {
                        spriteBatch.Draw(imageBall, new Rectangle(i * 32, j * 32, 32, 32), Color.White);
                    }
                }
            }

            spriteBatch.Draw(imageLogo, new Rectangle(robbyX * 32, robbyY * 32, 32, 32), Color.White);

            //Vectors for the display boxes
            Vector2 vector2 = new Vector2(25, 350);
            Vector2 vector21 = new Vector2(25, 375);
            Vector2 vector22 = new Vector2(25, 400);

            //Display info
            spriteBatch.DrawString(font, "Current Generation: " + data[i][0] + "", vector2, Color.White);
            spriteBatch.DrawString(font, "Score: " + score + "", vector21, Color.White);
            spriteBatch.DrawString(font, "Number Of Moves: " + countNumMoves + "", vector22, Color.White);

            spriteBatch.End();


            base.Draw(gameTime);
        }

        /// <summary>
        /// Method to create the current array of chromosome
        /// </summary>
        /// <returns> An Allele[] </returns>
        public Allele[] AlleleArrayMaker()
        {
            string arr = data[i][2];
            Allele[] alleles = new Allele[243];

            String[] allelesString = arr.Split(',');

            for (int j = 0; j < 243; j++)
            {
                alleles[j] = (Allele)Enum.Parse(typeof(Allele), allelesString[j]);
            }

            return alleles;
        }
    }
}
 