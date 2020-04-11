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


namespace RobbyMonogame
{
    class SimulationSprite : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Game1 game;
        private Texture2D imageBall;
        private Texture2D imageLogo;
        String[][] data; 

        public SimulationSprite(Game1 game) : base(game)
        {
            // TODO: Construct any child components here
        }

        public override void Initialize()
        {
            base.Initialize();

            //Initialize the outer array to be 7
            data = new string[7][];
            String[] line;
            //String[] to hold all the paths for the files
            String[] paths = { @"..\..\gen1.txt", @"..\..\gen20.txt", @"..\..\gen100.txt", @"..\..\gen200.txt", @"..\..\gen500.txt", @"..\..\gen1000.txt", @"..\..\gen5000.txt", @"..\..\gen10000.txt" };

            //Loop to go through each file and get the information, slip it and store inside the data array
            for (int i = 0; i < paths.Length; i++)
            {
                //Get the only line from the file
                line = File.ReadAllLines(paths[i]);

                //Slipt that line on each ; and store as a String[] inside the string[][]
                data[i] = line[0].Split(';');
            }


        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
 