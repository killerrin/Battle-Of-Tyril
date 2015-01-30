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
    public class Touchpoints
    {
        public double LastTappedX { get; set; }
        public double LastTappedY { get; set; }

        public Rectangle LastTapped { get; set; }
        public Rectangle[] RectCoords { get; set; }

        public Texture2D Texture;

        public Touchpoints(Texture2D texture)
        {
            Texture = texture;
            Texture.SetData(new Color[] { Color.White });
        }

        public void ResetTouchpoints()
        {
            LastTappedX = 0;
            LastTappedY = 0;
            LastTapped = new Rectangle(0, 0, 1, 1);
            //for (int ctr = 0; ctr < 1000000; ctr++) { }
        }

        public void Update()
        {
            // Grab Touchpoints to allow for easier debugging.
            int ctr = 0;
            RectCoords = new Rectangle[6];
            foreach (TouchLocation location in TouchPanel.GetState())
            {
                LastTappedX = location.Position.X;
                LastTappedY = location.Position.Y;

                try
                {
                    RectCoords[ctr] = new Rectangle((int)location.Position.X - 25, (int)location.Position.Y - 25, 50, 50);
                    ++ctr;
                }
                catch (IndexOutOfRangeException)
                {
                    break;
                }

            }
            LastTapped = new Rectangle((int)LastTappedX, (int)LastTappedY, 1, 1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < RectCoords.Length; ++i)
            {
                spriteBatch.Draw(Texture, RectCoords[i], Color.GreenYellow);
            }
        }
    }
}
