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
    public class CampaignModeMenu
    {
        private Texture2D campaignModeTexture;
        private Texture2D blankTexture;

        private Menu campaignScreen;

        public CampaignModeMenu(ContentManager Content, Texture2D blank)
        {
            campaignModeTexture = Content.Load<Texture2D>("Images/Backgrounds/Menus/Player Selection");

            campaignScreen = new Menu("Campaign", campaignModeTexture);
            blankTexture = blank;
        }

        public void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                BoT.gameState = GameState.ModeSelect;
                BoT.touchpoints.ResetTouchpoints();
            }
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            campaignScreen.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
