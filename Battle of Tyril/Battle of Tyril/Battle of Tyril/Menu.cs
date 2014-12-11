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
    public class Menu
    {
        public string Name { get; set; }
        public bool Activated { get; set; }
        public Texture2D Texture { get; set; }

        public Menu()
        {
            Name = "";
            Activated = false;
        }
        public Menu(string menuName, Texture2D texture)
        {
            Name = menuName;
            Activated = false;
            Texture = texture;
        }
        public Menu(string menuName, Texture2D texture, bool activated)
        {
            Name = menuName;
            Activated = activated;
            Texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,new Vector2(0,0),Color.White);
        }
        public void Draw(SpriteBatch spriteBatch, RenderText text)
        {
            spriteBatch.Draw(Texture, new Vector2(0, 0), Color.White);
            text.Draw(spriteBatch);
        }
    }
}
