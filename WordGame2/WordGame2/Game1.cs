using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.IO;
using System.Diagnostics;

namespace TextTest
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        public const int WIDTH = 800;
        public const int HEIGHT = 480;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<String> words = new List<string>();
        SpriteFont normalFont;
        List<WordSprite> wordSprites = new List<WordSprite>();
        Texture2D dot, space;
        Vector2 bg = new Vector2();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            StreamReader reader = new StreamReader("text.txt");
            while (reader.Peek() != -1)
            {
                String line = reader.ReadLine();
                line = line.Replace(".", "").Replace(",", "");
                String[] lineWords = line.Split(new char[] { ' ', '\t', '—', '-' });
                words.AddRange(lineWords);
            }

            normalFont = Content.Load<SpriteFont>("Normal");
            dot = Content.Load<Texture2D>("Dot");
            space = Content.Load<Texture2D>("space");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Vector2 mapVelocity = new Vector2(Mouse.GetState().X * 2 - WIDTH, Mouse.GetState().Y * 2 - HEIGHT);

            bg -= mapVelocity * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if (bg.X < -space.Width) bg.X += space.Width;
            if (bg.Y < -space.Height) bg.Y += space.Height;
            if (bg.X > 0) bg.X -= space.Width;
            if (bg.Y > 0) bg.Y -= space.Height;

            foreach (WordSprite sprite in wordSprites)
            {
                sprite.Update(gameTime, mapVelocity);
            }

            if (gameTime.TotalGameTime.Milliseconds % 10 == 0)
            {
                if (words.Count > 0 && wordSprites.Count < 300)
                {
                    string word = words[0];
                    words.RemoveAt(0);
                    wordSprites.Add(new WordSprite(word, normalFont));
                }
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    spriteBatch.Draw(space, bg + new Vector2(i * space.Width, j * space.Height), Color.White);
                }
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            
            foreach (WordSprite sprite in wordSprites)
            {
                sprite.Draw(spriteBatch);
            }

            spriteBatch.Draw(dot, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
