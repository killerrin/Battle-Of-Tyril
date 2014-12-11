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
    public class GameOverMenu
    {
        private Texture2D modeSelectionTexture;
        private Menu modeScreen;

        private RenderText menuitem_gameOver_GoHome;

        public GameOverMenu(ContentManager Content)
        {
            modeSelectionTexture = Content.Load<Texture2D>("Images/Backgrounds/Menus/Player Selection");
            modeScreen = new Menu("Mode Selection", modeSelectionTexture);

            menuitem_gameOver_GoHome = new RenderText("Main Menu", new Vector2(BoT.screenWidth / 3f, 120f), Color.LimeGreen, BoT.SpaceAndAstronomySmall);
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
                        if (menuitem_gameOver_GoHome.CollideRectangle.Contains(new Rectangle((int)gs.Position.X, (int)gs.Position.Y, 1, 1)))
                        {
                            BoT.gameState = GameState.MainMenu;
                            BoT.touchpoints.ResetTouchpoints();
                        }
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            modeScreen.Draw(spriteBatch);
            RenderText.Draw(spriteBatch, "Game Over", new Vector2(BoT.screenWidth / 4f, 20f), Color.DarkRed, BoT.SpaceAndAstronomy);
            
            menuitem_gameOver_GoHome.Draw(spriteBatch);
            RenderText.Draw(spriteBatch, "You Survived " + (BoT.playGameMenu.totalTimeAlive/1000f) + " Seconds\nfor a total of " + BoT.playGameMenu.wave + " Waves.", new Vector2(BoT.screenWidth/6f, 200f), Color.Green, BoT.SpaceAndAstronomySmall);
            spriteBatch.End();
        }
    }
}
