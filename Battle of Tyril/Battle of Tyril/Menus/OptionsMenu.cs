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
    public class OptionsMenu
    {
        private Texture2D optionsTexture;

        public Menu optionScreen;

        public OptionsMenu(ContentManager Content)
        {
            optionsTexture = Content.Load<Texture2D>("Images/Backgrounds/Menus/Options");
            optionScreen = new Menu("Options", optionsTexture);
        }

        public void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                BoT.gameState = GameState.MainMenu;
                BoT.touchpoints.ResetTouchpoints();
            }
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            optionScreen.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
