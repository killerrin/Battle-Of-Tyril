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


/// 
/// At this time the class does not work. Use the VirtualThumbstick.cs class instead.
///
namespace Battle_of_Tyril
{
    class Thumbstick
    {
        #region Fields
        private Texture2D texture;
        
        private const float maxThumbstickDistance = 60f;
        private Vector2 thumbPosition;
        private Vector2 newTouch;
        private int thumbId = -1;

        public Vector2 ThumbstickCenter { get; set; }
        public int ThumbID { get { return thumbId; } set { thumbId = value; } }
        public Vector2 ThumbPosition { get { return thumbPosition; } set { thumbPosition = value; } }
        public Vector2 NewTouch { get { return newTouch; } }
        #endregion

        public Thumbstick(Vector2 location, Texture2D tex)
        {
            texture = tex;
            ThumbPosition = location;
            ThumbstickCenter = new Vector2((location.X+tex.Width / 2), (location.Y+tex.Height/2));
        }

        public Vector2 ThumbDistance()
        {
            //if (!NewTouch.HasValue)
            //{
            //    return Vector2.Zero;
            //}
            //else
            //{
                //Vector2 temp = new Vector2(NewTouch.X, NewTouch.Value.Y);
                Vector2 l = (NewTouch - ThumbstickCenter) / maxThumbstickDistance;

                if (l.LengthSquared() > 1f)
                {
                    l.Normalize();
                }
                return l;
            //}
        }

        public void Update()
        {
            TouchLocation? ttouch = null;//, rtouch = null;
            TouchCollection touches = TouchPanel.GetState();

            foreach (var touch in touches)
            {
                if (touch.Id == ThumbID)
                {
                    ttouch = touch;
                    continue;
                }

                TouchLocation earliestTouch;
                if (!touch.TryGetPreviousLocation(out earliestTouch))
                {
                    earliestTouch = touch;
                }

                if (ThumbID == -1)
                {
                    if (earliestTouch.Position.X > TouchPanel.DisplayWidth / 2)// && earliestTouch.Position.Y > TouchPanel.DisplayHeight / 2)
                    {
                        // If Bottom Right
                        ttouch = earliestTouch;
                        //continue;
                    }
                    else if (earliestTouch.Position.X < TouchPanel.DisplayWidth / 2)// && earliestTouch.Position.Y > TouchPanel.DisplayHeight / 2)
                    {
                        // If Bottom Left
                        ttouch = earliestTouch;
                        //continue;
                    }
                    //else if (earliestTouch.Position.X > TouchPanel.DisplayWidth / 2 && earliestTouch.Position.Y < TouchPanel.DisplayHeight / 2)
                    //{
                    //    // If Top Right
                    //    ttouch = earliestTouch;
                    //    //continue;
                    //}
                    //else if (earliestTouch.Position.X < TouchPanel.DisplayWidth / 2 && earliestTouch.Position.Y < TouchPanel.DisplayHeight / 2)
                    //{
                    //    // If Top Left
                    //    ttouch = earliestTouch;
                    //    //continue;
                    //}
                }
            }

            if (ttouch.HasValue)
            {
                newTouch = ttouch.Value.Position;

                ThumbID = ttouch.Value.Id;
            }
            else
            {
                newTouch = Vector2.Zero;//new Vector2 (0.00f,0.00f);
                ThumbID = -1;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, ThumbPosition, Color.White);
        }

    }
}