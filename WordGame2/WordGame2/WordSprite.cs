using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TextTest
{
    public class WordSprite
    {
        private String word;
        private SpriteFont font;
        private Vector2 size;
        private Random rand = new Random();
        private Color color;
        private float layerDepth;

        public Vector2 Position;
        public Vector2 Velocity;

        private Color startColor;


        public WordSprite(string word, SpriteFont font)
        {
            this.word = word;
            this.font = font;
            size = font.MeasureString(word);
            if (rand.Next(2) == 0)
            {
                Velocity = new Vector2(250 + rand.Next(1000), 0);
                Position = new Vector2(-size.X, rand.Next(Game1.HEIGHT));


                if (rand.Next(2) == 0)
                {
                    Velocity *= -1;
                    Position.X += Game1.WIDTH + size.X * 2;
                }
            }
            else
            {
                Velocity = new Vector2(0, 250 + rand.Next(1000));
                Position = new Vector2(rand.Next(Game1.WIDTH), -size.Y);

                if (rand.Next(2) == 0)
                {
                    Velocity *= -1;
                    Position.Y += Game1.HEIGHT + size.Y * 2;
                }
            }

            int trunk = 200;
            Velocity.X = (int)(Velocity.X / trunk) * trunk;
            Velocity.Y = (int)(Velocity.Y / trunk) * trunk;

            //Velocity += new Vector2(rand.Next(800) - 400, rand.Next(800) - 400);

            startColor = new Color(rand.Next(255), rand.Next(255), rand.Next(255));

        }

        public void Update(GameTime gameTime, Vector2 mapVelocity)
        {
            Vector2 relativeVelocity = Velocity - mapVelocity;
            Position += relativeVelocity * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if (Position.X > Game1.WIDTH + size.X && relativeVelocity.X > 0) Position.X = -size.X;
            if (Position.Y > Game1.HEIGHT + size.Y && relativeVelocity.Y > 0) Position.Y = -size.Y;
            if (Position.X < -size.X && relativeVelocity.X < 0) Position.X = Game1.WIDTH + size.X;
            if (Position.Y < -size.Y && relativeVelocity.Y < 0) Position.Y = Game1.HEIGHT + size.Y;
            color = speedColor(relativeVelocity);
            layerDepth = Math.Max(255 - Math.Abs(relativeVelocity.Length()), 0) / 255f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, word, Position, color, 0, size / 2, layerDepth + 1, SpriteEffects.None, layerDepth);
            //spriteBatch.DrawString(font, word, Position - size / 2, color);
        }

        private Color speedColor(Vector2 relativeVelocity)
        {
            //int r = (int)Math.Min(Math.Max(startColor.R - 123 + Math.Abs(relativeVelocity.Length()), 0), 255);
            //int b = (int)Math.Min(Math.Max(startColor.B - 123 + Math.Abs(relativeVelocity.Length()), 0), 255);
            return new Color(startColor.R, startColor.G, startColor.B);
        }
    }
}
