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
    public class MainMenu
    {
        private Texture2D titleScreenTexture;
        public Song song;

        public bool isMusicPlaying;

        private Menu titleScreen;

        private RenderText menuitem_startmenu_Play;
        private RenderText menuitem_startmenu_Options;
        private RenderText menuitem_startmenu_ExitGame;

        public Boolean CloseGame { get; private set; }

        public MainMenu(ContentManager Content)
        {
            titleScreenTexture = Content.Load<Texture2D>("Images/Backgrounds/Menus/Titlescreen");
            song = Content.Load<Song>("Music/Rise to the King");

            titleScreen = new Menu("Title Screen", titleScreenTexture, true);

            menuitem_startmenu_Play = new RenderText("Play", new Vector2(50.0f, 140.0f), Color.LimeGreen, BoT.SpaceAndAstronomy);
            menuitem_startmenu_Options = new RenderText("Options", new Vector2(50.0f, 300.0f), Color.LimeGreen, BoT.SpaceAndAstronomy); // (50.0f, 220.0f)
            menuitem_startmenu_ExitGame = new RenderText("Exit Game", new Vector2(50.0f, 220.0f), Color.LimeGreen, BoT.SpaceAndAstronomy); //(50.0f, 300.0f)

            CloseGame = false;
            isMusicPlaying = false;
        }

        public void Update(GameTime gameTime)
        {
            if (BoT.isMusicCurrentlyPlaying)
            {
                if (!isMusicPlaying)
                {
                    isMusicPlaying = true;
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(song);
                }
            }

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();
                switch (gs.GestureType)
                {
                    case GestureType.Tap:
                        if (menuitem_startmenu_Play.CollideRectangle.Contains(new Rectangle((int)gs.Position.X, (int)gs.Position.Y, 1, 1)))
                        {
                            BoT.gameState = GameState.ArcadeSelection; // Go to Mode Select Screen
                            BoT.touchpoints.ResetTouchpoints();
                        }
                        else if (menuitem_startmenu_Options.CollideRectangle.Contains(new Rectangle((int)gs.Position.X, (int)gs.Position.Y,1,1)))
                        {
                            //BoT.gameState = GameState.Options;
                            //BoT.touchpoints.ResetTouchpoints();
                        }
                        else if (menuitem_startmenu_ExitGame.CollideRectangle.Contains(new Rectangle((int)gs.Position.X, (int)gs.Position.Y, 1, 1)))
                        {
                            if (!Guide.IsVisible)
                            {
                                Guide.BeginShowMessageBox("Exit Game?", "Are you sure you want to quit the game?",
                                   new List<String> { "Yes", "No" }, 0, MessageBoxIcon.Error, asyncResult =>
                                   {
                                       int? returned = Guide.EndShowMessageBox(asyncResult);
                                       if (returned != null && returned == 0)
                                       {
                                           CloseGame = true;
                                       }
                                   }, null);
                            }
                        }
                        break;
                }
            }
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            titleScreen.Draw(spriteBatch);
            menuitem_startmenu_Play.Draw(spriteBatch);
            //menuitem_startmenu_Options.Draw(spriteBatch);
            menuitem_startmenu_ExitGame.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
