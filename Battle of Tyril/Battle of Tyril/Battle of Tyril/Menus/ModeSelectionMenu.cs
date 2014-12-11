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
    public class ModeSelectionMenu
    {
        private Texture2D modeSelectionTexture;
        private Menu modeScreen;

        private RenderText menuitem_modeselection_Campaign;
        private RenderText menuitem_modeselection_Arcade;

        public ModeSelectionMenu(ContentManager Content)
        {
            modeSelectionTexture = Content.Load<Texture2D>("Images/Backgrounds/Menus/Player Selection");
            modeScreen = new Menu("Mode Selection", modeSelectionTexture);

            menuitem_modeselection_Campaign = new RenderText("Campaign", new Vector2(80.0f, 140.0f), Color.LimeGreen, BoT.SpaceAndAstronomy); //(80.0f, 140.0f)
            menuitem_modeselection_Arcade = new RenderText("Arcade", new Vector2(80.0f, 220.0f), Color.LimeGreen, BoT.SpaceAndAstronomy); // (80.0f, 220.0f)
        }

        public void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                BoT.gameState = GameState.MainMenu;
                BoT.touchpoints.ResetTouchpoints();
            }

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();
                switch (gs.GestureType)
                {
                    //new Rectangle((int)gs.Position.X, (int)gs.Position.Y,1,1)
                    case GestureType.Tap:
                        if (menuitem_modeselection_Campaign.CollideRectangle.Contains(new Rectangle((int)gs.Position.X, (int)gs.Position.Y, 1, 1)))
                        {
                            BoT.gameState = GameState.CampaignSelection;
                            BoT.touchpoints.ResetTouchpoints();
                        }
                        else if (menuitem_modeselection_Arcade.CollideRectangle.Contains(new Rectangle((int)gs.Position.X, (int)gs.Position.Y, 1, 1)))
                        {
                            BoT.gameState = GameState.ArcadeSelection;
                            BoT.touchpoints.ResetTouchpoints();
                        }
                        break;
                }
            }
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            modeScreen.Draw(spriteBatch);
            //menuitem_modeselection_Campaign.Draw(spriteBatch);
            menuitem_modeselection_Arcade.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
