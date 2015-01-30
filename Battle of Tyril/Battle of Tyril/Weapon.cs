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
    public class Weapon
    {
        public string Name { get; set; }
        public double Speed { get; set; }
        public double Damage { get; set; }
        public int WeaponCounter { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 SpriteOrigin { get; set; }

        public Weapon(string name, double speed, double damage, int counter, Texture2D texture)
        {
            Name = name;
            Speed = speed;
            Damage = damage;
            WeaponCounter = counter;
            Texture = texture;
            SpriteOrigin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }
    }
}
