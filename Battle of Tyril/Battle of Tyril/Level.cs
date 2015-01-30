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
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Battle_of_Tyril
{
    public class Level
    {
        public string Name { get; set; }
        public bool Activated { get; set; }
        public Texture2D Texture { get; set; }
        public Texture2D GameBackground { get; set; }
        public Texture2D GameOverBackground { get; set; }
        public Texture2D GameWinBackground { get; set; }
        public Texture2D ThumbOutline { get; set; }
        public Vector2 Position { get; set; }
        public Color Colour { get; set; }
        public Rectangle ThumbRectangle { get; private set; }

        public Level(string name, Texture2D texture, Vector2 position, Color colour)
        {
            Name = name;
            Texture = texture;
            GameBackground = texture;
            Position = position;
            Colour = colour;
            Activated = false;

            ThumbRectangle = new Rectangle(0,0,0,0);
        }
        public Level(string name, Texture2D texture, Vector2 position, Color colour, bool activated)
        {
            Name = name;
            Texture = texture;
            Position = position;
            Colour = colour;
            Activated = activated;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Texture, Position, Colour);
            spriteBatch.Draw(Texture, Position, null, Colour, 0f, new Vector2(0,0), 0.63f, SpriteEffects.None, 0f);
        }
        public void DrawThumbnail(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            ThumbRectangle = new Rectangle((int)position.X-96, (int)position.Y-58, (int)(Texture.Width * scale), (int)(Texture.Height * scale));

            if (Activated == true)
            {
                spriteBatch.Draw(ThumbOutline, new Rectangle((int)(ThumbRectangle.X - 2), (int)(ThumbRectangle.Y - 2), (int)ThumbRectangle.Width + 4, (int)ThumbRectangle.Height + 4), Color.LimeGreen);
            }
            else
            {
                spriteBatch.Draw(ThumbOutline, new Rectangle((int)(ThumbRectangle.X - 2), (int)(ThumbRectangle.Y - 2), (int)ThumbRectangle.Width + 4, (int)ThumbRectangle.Height + 4), Color.Green);
            }

            spriteBatch.Draw(Texture, position, null, Colour, 0f, new Vector2(Texture.Width/2, Texture.Height/2), scale, SpriteEffects.None, 0f);
        }


    }
}
