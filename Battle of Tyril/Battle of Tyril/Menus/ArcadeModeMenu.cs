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
using System.Diagnostics;
using Microsoft.Advertising;
using Microsoft.Advertising.Mobile;
using Microsoft.Advertising.Mobile.Xna;
using System.Device.Location;

namespace Battle_of_Tyril
{
    public class ArcadeModeMenu
    {
        private ContentManager content;

        private Texture2D arcadeModeTexture;
        private Texture2D blankTexture;

        private Texture2D level001Background;
        private Texture2D level001Gameover;
        private Texture2D level001Win;
        private Texture2D level002Background;
        private Texture2D level002Gameover;
        private Texture2D level002Win;

        private Level arcadeLevel001;
        private Level arcadeLevel002;

        private Menu arcadeScreen;
        private RenderText menuitem_arcadeselection_StartGame;

        public ArcadeModeMenu(ContentManager Content, Texture2D blank)
        {
            content = Content;

            arcadeModeTexture = Content.Load<Texture2D>("Images/Backgrounds/Menus/Player Selection");

            arcadeScreen = new Menu("Arcade", arcadeModeTexture);
            menuitem_arcadeselection_StartGame = new RenderText("Start Game", new Vector2(350.0f, 400.0f), Color.LimeGreen, BoT.SpaceAndAstronomy);

            blankTexture = blank;

            level001Background = Content.Load<Texture2D>("Images/Backgrounds/Levels/Arcade/001/Game1");
            level001Gameover = Content.Load<Texture2D>("Images/Backgrounds/Levels/Arcade/001/Gameover Death");
            level001Win = Content.Load<Texture2D>("Images/Backgrounds/Levels/Arcade/001/Gameover Time Up");

            level002Background = Content.Load<Texture2D>("Images/Backgrounds/Levels/Arcade/002/Game");
            level002Gameover = Content.Load<Texture2D>("Images/Backgrounds/Levels/Arcade/002/Gameover Death");
            level002Win = Content.Load<Texture2D>("Images/Backgrounds/Levels/Arcade/002/Gameover Time Up");

            arcadeLevel001 = new Level("Tyril", level001Background, new Vector2(0, 0), Color.White);
            arcadeLevel001.GameOverBackground = level001Gameover;
            arcadeLevel001.GameWinBackground = level001Win;
            arcadeLevel001.ThumbOutline = blankTexture;

            arcadeLevel002 = new Level("Sal'Dero", level002Background, new Vector2(0, 0), Color.White);
            arcadeLevel002.GameOverBackground = level002Gameover;
            arcadeLevel002.GameWinBackground = level002Win;
            arcadeLevel002.ThumbOutline = blankTexture;

            BoT.playGameMenu.currentLevel = arcadeLevel001;// new Level("Tyril", level001Background, new Vector2(0, 0), Color.White, false);
        }

        public void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                BoT.gameState = GameState.MainMenu; // Put mode back to ModeSelect
                BoT.touchpoints.ResetTouchpoints();
            }

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();
                switch (gs.GestureType)
                {
                    //new Rectangle((int)gs.Position.X, (int)gs.Position.Y,1,1)
                    case GestureType.Tap:
                        if (menuitem_arcadeselection_StartGame.CollideRectangle.Contains(new Rectangle((int)gs.Position.X, (int)gs.Position.Y, 1, 1)))
                        {
                            BoT.gameState = GameState.PlayGame;
                            BoT.touchpoints.ResetTouchpoints();

                            BoT.playGameMenu = new PlayGameMenu(content, BoT.graphicsDevice);

                            if (arcadeLevel001.Activated)
                            {
                                BoT.playGameMenu.currentLevel = arcadeLevel001;
                            }
                            else if (arcadeLevel002.Activated)
                            {
                                BoT.playGameMenu.currentLevel = arcadeLevel002;
                            }
                            else// if (BoT.playGameMenu.currentLevel == null)
                            {
                                Debug.WriteLine("No Level Selected... Defaulting.");
                                BoT.playGameMenu.currentLevel = arcadeLevel001;
                            }

                            arcadeLevel001.Activated = false;
                            arcadeLevel002.Activated = false;

                            Debug.WriteLine("Loading Level: " + BoT.playGameMenu.currentLevel.Name.ToString());
                            BoT.mainMenu.isMusicPlaying = false;
                            if (BoT.isMusicCurrentlyPlaying) { MediaPlayer.Stop(); }
                            BoT.AdRemove = true;
                        }
                        else if (arcadeLevel001.ThumbRectangle.Contains(new Rectangle((int)gs.Position.X, (int)gs.Position.Y, 1, 1)))
                        {
                            BoT.playGameMenu.currentLevel = arcadeLevel001;
                            arcadeLevel001.Activated = true;
                            arcadeLevel002.Activated = false;
                            Debug.WriteLine(BoT.playGameMenu.currentLevel.Name.ToString() + " Selected");
                        }
                        else if (arcadeLevel002.ThumbRectangle.Contains(new Rectangle((int)gs.Position.X, (int)gs.Position.Y, 1, 1)))
                        {
                            BoT.playGameMenu.currentLevel = arcadeLevel002;
                            arcadeLevel001.Activated = false;
                            arcadeLevel002.Activated = true;
                            Debug.WriteLine(BoT.playGameMenu.currentLevel.Name.ToString() + " Selected");
                        }
                        break;
                }
            }
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            arcadeScreen.Draw(spriteBatch);
            arcadeLevel001.DrawThumbnail(spriteBatch, new Vector2(200, 200), 0.15f);
            arcadeLevel002.DrawThumbnail(spriteBatch, new Vector2(400, 200), 0.15f);
            menuitem_arcadeselection_StartGame.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
