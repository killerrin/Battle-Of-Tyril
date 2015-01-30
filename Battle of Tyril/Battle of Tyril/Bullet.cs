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
    public class Bullet
    {
        public Weapon MainWeapon { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public Rectangle CollideRectangle { get; set; }
        //public bool AllyOrFoe { get; set; }

        public int DeathCounter { get; set; }

        public Bullet()
        {
            DeathCounter = 1;
        }

        public Bullet(Weapon weapon, Vector2 position, Vector2 direction)//, bool allyOrFoe)
        {
            MainWeapon = weapon;
            Position = position;
            Direction = direction;
            //AllyOrFoe = allyOrFoe;

            DeathCounter = 0;
            CollideRectangle = new Rectangle((int)position.X, (int)position.Y, weapon.Texture.Width, weapon.Texture.Height);
        }

        public bool CheckCollision(Rectangle rect)
        {
            if (CollideRectangle.Intersects(rect))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Update()
        {
            if (DeathCounter == 0)
            {
                Position = new Vector2((Position.X + Direction.X * (float)MainWeapon.Speed),
                    (Position.Y + Direction.Y * (float)MainWeapon.Speed));

                CollideRectangle = new Rectangle((int)Position.X, (int)Position.Y,
                    (int)MainWeapon.Texture.Width, (int)MainWeapon.Texture.Height);

                //if enemycollide:
                //    for i in enemylist:
                //        if pygame.sprite.collide_mask(self, i):
                //            i.set_health(i.get_health()-25)
                //            self.deathctr +=1
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MainWeapon.Texture, Position, Color.Green);
        }
    }
}
