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
    public class PlayerShip:Ship
    {
        public int shootctr {get;set;}

        public PlayerShip(Vector2 location, float acceleration, Texture2D texture, Rectangle screensize, Color colour)
            : base(location, acceleration, texture, screensize, colour)
        {
            Health = 100.0;
            shootctr = 0;
        }

        public override void Update(GameTime gameTime)
        {
            // adjust our velocity base on our virtual thumbstick
            Velocity += VirtualThumbsticks.LeftThumbstick * Acceleration;

            base.Update(gameTime);

            // decrease the velocity a little bit for some drag
            Velocity *= 0.99f;

            // update our ship's rotation based on the left thumbstick
            if (VirtualThumbsticks.LeftThumbstick.X != 0 && VirtualThumbsticks.LeftThumbstick.Y != 0)
            {
                Rotation = (float)Math.Atan2(VirtualThumbsticks.LeftThumbstick.X, -VirtualThumbsticks.LeftThumbstick.Y);
            }

            // Clamp the player to the screen
            ClampToScreen();
            CollideRectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public override Bullet Shoot()
        {
            Bullet shot;
            //shootctr = 0;
            //shot = new Bullet(MainWeapon, Position, VirtualThumbsticks.RightThumbstick, true);
            //return (shot);
            if (shootctr == MainWeapon.WeaponCounter || shootctr == -999)
            {
                shootctr = 0;
                shot = new Bullet(MainWeapon, Position, VirtualThumbsticks.RightThumbstick);//, true);
                return (shot);
            }
            else { ++shootctr; return (new Bullet()); }
        }
        
        public override void Draw (SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }


    }
}
