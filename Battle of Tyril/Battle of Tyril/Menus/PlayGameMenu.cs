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
    public class PlayGameMenu
    {
        enum PlayMode
        {
            Playing,
            Paused,
            HelpAndOptions,
            Options
        }
        private PlayMode playMode;

        private Menu pauseScreen;

        private Texture2D playership01;
        private Texture2D playership02;
        private Texture2D enemyship01;
        private Texture2D thumbstick;
        private Texture2D plasmaBoltTexture;
        private Song gameSong;
        private SoundEffect pauseSound;
        private SoundEffectInstance pauseSoundInstance;
        //private Song pausedSong;

        private bool isMusicPlaying;

        private RenderText menuitem_pause_Resume;
        private RenderText menuitem_pause_EndGame;
        private RenderText menuitem_pause_ExitGame;
        private RenderText menuitem_pause_optionsandhelp;
        private RenderText menuitem_pause_options;

        public Level currentLevel;
        private List<Bullet> playerBullets;
        private List<AIShip> aiShips;
        private Weapon plasmaBolt;
        public PlayerShip player;

        const double CRASH_HEALTH_CONST = 10;

        float aiSpawnTimer;
        public int wave = 0;
        public float totalTimeAlive = 0f;
        const float MAX_AI_SPAWN_TIMER = 5000;
        public Boolean CloseGame { get; private set; }

        public PlayGameMenu(ContentManager Content, GraphicsDevice graphicDevice)
        {
            PlayMode playMode = PlayMode.Playing;

            CloseGame = false;
            isMusicPlaying = false;

            wave = 0;
            aiSpawnTimer = 5000f;
            totalTimeAlive = 0f;

            ///-- Audio --\\\
            pauseSound = Content.Load<SoundEffect>("Music/Pause Ambience");
            pauseSoundInstance = pauseSound.CreateInstance();
            pauseSoundInstance.IsLooped = true;
            gameSong = Content.Load<Song>("Music/Flight of the Crow");


            ///-- Ships ---\\\
            playership01 = Content.Load<Texture2D>("Images/Ships/Players/001");
            playership02 = Content.Load<Texture2D>("Images/Ships/Players/002");
            enemyship01 = Content.Load<Texture2D>("Images/Ships/Enemies/001");

            ///-- Weapons ---\\\
            plasmaBoltTexture = Content.Load<Texture2D>("Images/Weapons/PlasmaBolt/Plasma");

            ///-- Images --\\\
            thumbstick = Content.Load<Texture2D>("Images/ect/thumbstick");

            ///-- Menus --\\\
            pauseScreen = new Menu("Pause", new Texture2D(graphicDevice, BoT.screenWidth, BoT.screenHeight));

            ///-- Text --\\\
            menuitem_pause_Resume = new RenderText("Resume", new Vector2(200, 220), Color.LimeGreen, BoT.SpaceAndAstronomySmall);
            menuitem_pause_EndGame = new RenderText("End Level", new Vector2(50, 300), Color.LimeGreen, BoT.SpaceAndAstronomySmall);
            menuitem_pause_ExitGame = new RenderText("Exit Game", new Vector2(170, 380), Color.LimeGreen, BoT.SpaceAndAstronomySmall);
            menuitem_pause_optionsandhelp = new RenderText("How to Play", new Vector2(300, 300), Color.LimeGreen, BoT.SpaceAndAstronomySmall); //Help & Options
            menuitem_pause_options = new RenderText("Options", new Vector2(550, 400), Color.LimeGreen, BoT.SpaceAndAstronomySmall);

            ///-- Weapons --\\\
            ///                         NAME       SPEED DAM CTR       TEXTURE    
            plasmaBolt = new Weapon("Plasma Bolt", 30.0, 1.0, 3, plasmaBoltTexture);

            ///-- Player --\\\
            player = new PlayerShip(new Vector2(BoT.screenWidth / 2, BoT.screenHeight / 2), 0.03f, playership01, BoT.screen, Color.White);
            player.MainWeapon = plasmaBolt;

            ///-- Other --\\\
            playerBullets = new List<Bullet>();
            aiShips = new List<AIShip>();
            //foreach (AIShip i in AIShip.SpawnX(enemyship01, 10))
            //{
            //    aiShips.Add(i);
            //}
        }

        public void Update(GameTime gameTime)
        {
            switch (playMode)
            {
                case PlayMode.Playing:
                    PlayingUpdate(gameTime);
                    break;

                case PlayMode.Paused:
                    PausedUpdate(gameTime);
                    break;

                case PlayMode.HelpAndOptions:
                    HelpAndOptionsUpdate(gameTime);
                    break;

                case PlayMode.Options:
                    OptionsUpdate(gameTime);
                    break;
            }

        }

        private void HelpAndOptionsUpdate(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                playMode = PlayMode.Paused;
                BoT.AdAdd = true;
                BoT.touchpoints.ResetTouchpoints();
            }

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();
                switch (gs.GestureType)
                {
                    //new Rectangle((int)gs.Position.X, (int)gs.Position.Y,1,1)
                    case GestureType.Tap:
                        if (menuitem_pause_options.CollideRectangle.Contains(new Rectangle((int)gs.Position.X, (int)gs.Position.Y, 1, 1)))
                        {
                            //playMode = PlayMode.Options;
                            //BoT.touchpoints.ResetTouchpoints();
                        }
                        break;
                }
            }
        }

        private void OptionsUpdate(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                playMode = PlayMode.HelpAndOptions;
                BoT.touchpoints.ResetTouchpoints();
            }
        }

        private void PausedUpdate(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                playMode = PlayMode.Playing;
                BoT.AdRemove = true;
                BoT.touchpoints.ResetTouchpoints();
                if (BoT.isMusicCurrentlyPlaying)
                {
                    MediaPlayer.Resume();
                    pauseSoundInstance.Stop();
                }
            }

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();
                switch (gs.GestureType)
                {
                    //new Rectangle((int)gs.Position.X, (int)gs.Position.Y,1,1)
                    case GestureType.Tap:
                        if (menuitem_pause_Resume.CollideRectangle.Contains(new Rectangle((int)gs.Position.X, (int)gs.Position.Y, 1, 1)))
                        {
                            BoT.touchpoints.ResetTouchpoints();
                            playMode = PlayMode.Playing;
                            BoT.AdRemove = true;
                            if (BoT.isMusicCurrentlyPlaying)
                            {
                                MediaPlayer.Resume();
                                pauseSoundInstance.Stop();
                            }
                        }
                        else if (menuitem_pause_EndGame.CollideRectangle.Contains(new Rectangle((int)gs.Position.X, (int)gs.Position.Y, 1, 1)))
                        {
                            if (!Guide.IsVisible)
                            {
                                Guide.BeginShowMessageBox("End Game?", "Are you sure you want to go back to the main menu?",
                                   new List<String> { "Yes", "No" }, 0, MessageBoxIcon.Error, asyncResult =>
                                   {
                                       int? returned = Guide.EndShowMessageBox(asyncResult);
                                       if (returned != null && returned == 0)
                                       {
                                           playMode = PlayMode.Playing;
                                           BoT.gameState = GameState.MainMenu;
                                           isMusicPlaying = false;
                                           BoT.AdAdd = true;
                                           if (BoT.isMusicCurrentlyPlaying) { MediaPlayer.Stop(); }
                                       }
                                   }, null);
                            }
                            BoT.touchpoints.ResetTouchpoints();
                        }
                        else if (menuitem_pause_optionsandhelp.CollideRectangle.Contains(new Rectangle((int)gs.Position.X, (int)gs.Position.Y, 1, 1)))
                        {
                            playMode = PlayMode.HelpAndOptions;
                            BoT.touchpoints.ResetTouchpoints();
                        }
                        else if (menuitem_pause_ExitGame.CollideRectangle.Contains(new Rectangle((int)gs.Position.X, (int)gs.Position.Y, 1, 1)))
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
                            BoT.touchpoints.ResetTouchpoints();
                        }
                        break;
                }
            }
        }

        private void PlayingUpdate(GameTime gameTime)
        {
            if (BoT.isMusicCurrentlyPlaying)
            {
                if (!isMusicPlaying)
                {
                    isMusicPlaying = true;
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(gameSong);
                }
            }

            totalTimeAlive += gameTime.ElapsedGameTime.Milliseconds;
            aiSpawnTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (aiSpawnTimer <= 0)
            {
                aiSpawnTimer = MAX_AI_SPAWN_TIMER;
                aiShips.AddRange(AIShip.SpawnX(enemyship01, 10));
                ++wave;
            }


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                BoT.AdAdd = true;
                playMode = PlayMode.Paused;
                BoT.touchpoints.ResetTouchpoints();
                if (BoT.isMusicCurrentlyPlaying)
                {
                    MediaPlayer.Pause();

                    pauseSoundInstance.Play();
                }
            }

            //-- Update the Thumbsticks
            VirtualThumbsticks.Update();

            player.Update(gameTime);

            foreach (AIShip i in aiShips)
            {
                i.Update(gameTime);
            }

            if (VirtualThumbsticks.RightThumbstick != Vector2.Zero)
            {
                Bullet bullet = player.Shoot();
                BoT.debugtext = "Shooting Bullet";

                if (bullet.DeathCounter == 0)
                {
                    BoT.debugtext = "Bullet Added to List";
                    playerBullets.Add(bullet);
                }
            }
            foreach (Bullet i in playerBullets)
            {
                i.Update();
            }

            
            while (true)
            {
                bool exit = true;
                for (int i = 0; i < aiShips.Count; i++)
                {
                    if (aiShips[i].CheckPixelPerfectColision(player))
                    {
                        exit = false;
                        player.Health -= CRASH_HEALTH_CONST;
                        aiShips.RemoveAt(i);
                        break;
                    }
                    if (exit == false) { break; }
                    for (int x = 0; x < playerBullets.Count; x++)
                    {
                        if (aiShips[i].CheckPixelPerfectColision(playerBullets[x]))
                        {

                            exit = false;
                            playerBullets.RemoveAt(x);
                            aiShips.RemoveAt(i);
                            break;
                        }
                        if (exit == false) { break; }
                    }
                }

                if (exit == true) { break;}
            }

            if (player.Health <= 0)
            {
                playMode = PlayMode.Playing;
                BoT.gameState = GameState.GameOver;
                isMusicPlaying = false;
                if (BoT.isMusicCurrentlyPlaying) { MediaPlayer.Stop(); }
            }
        }

        private void PlayingDraw(SpriteBatch spriteBatch)
        {
            currentLevel.Draw(spriteBatch);

            DrawThumbsticks(spriteBatch);
            player.Draw(spriteBatch);

            foreach (AIShip i in aiShips)
            {
                i.Draw(spriteBatch);
            }

            foreach (Bullet i in playerBullets)
            {
                i.Draw(spriteBatch);
            }

            if (BoT.developmentMode)
            {
                RenderText.Draw(spriteBatch, "Health \n    " + player.Health.ToString(), new Vector2(BoT.screenWidth / 2.5f, 25f), Color.Green, BoT.SpaceAndAstronomySmall);
            }
            else
            {
                RenderText.Draw(spriteBatch, "Health \n    " + player.Health.ToString(), new Vector2(35, 25f), Color.Green, BoT.SpaceAndAstronomySmall);
            }
            RenderText.Draw(spriteBatch, "Next Wave \n    " + (aiSpawnTimer/1000).ToString(), new Vector2(BoT.screenWidth / 1.5f, 25f), Color.Green, BoT.SpaceAndAstronomySmall);
        
        
        }

        private void PausedDraw(SpriteBatch spriteBatch)
        {
            RenderText.Draw(spriteBatch, "GAME PAUSED", new Vector2(150, 100), Color.Silver, BoT.SpaceAndAstronomy);
            menuitem_pause_Resume.Draw(spriteBatch);
            menuitem_pause_EndGame.Draw(spriteBatch);
            menuitem_pause_ExitGame.Draw(spriteBatch);
            menuitem_pause_optionsandhelp.Draw(spriteBatch);
        }

        private void HelpAndOptionsDraw(SpriteBatch spriteBatch)
        {
            //menuitem_pause_options.Draw(spriteBatch);

            if (BoT.fixedthumbstick == true)
            {
                RenderText.Draw(spriteBatch, "Press, hold and rotate the left thumbstick to move", new Vector2(50, 100), Color.White, BoT.segoeFont);
                RenderText.Draw(spriteBatch, "Press, hold and rotate the right thumbstick to shoot", new Vector2(50, 120), Color.White, BoT.segoeFont);
            }
            else
            {
                RenderText.Draw(spriteBatch, "Press and hold on the left/right sides of the screen to bring up", new Vector2(50, 100), Color.White, BoT.segoeFont);
                RenderText.Draw(spriteBatch, "the thumbsticks. LeftThumbstick to move. RightThumbstick to shoot", new Vector2(50, 120), Color.White, BoT.segoeFont);
            }
            //RenderText.Draw(spriteBatch, "Pick up powerups for temperary upgrades by flying into them", new Vector2(50, 150), Color.White, BoT.segoeFont);
            //RenderText.Draw(spriteBatch, "Kill enemies for Credits", new Vector2(50, 180), Color.White, BoT.segoeFont);
            //RenderText.Draw(spriteBatch, "Use Credits to upgrade your ship in the level selection menu.", new Vector2(50, 210), Color.White, BoT.segoeFont);
            //RenderText.Draw(spriteBatch, "Above all else... DONT DIE!", new Vector2(200, 240), Color.White, BoT.segoeFont);
        }

        private void OptionsDraw(SpriteBatch spriteBatch)
        {
            BoT.optionsMenu.optionScreen.Draw(spriteBatch);
        }

        private void DrawThumbsticks(SpriteBatch spriteBatch)
        {
            if (VirtualThumbsticks.LeftThumbstickCenter.HasValue)
            {
                spriteBatch.Draw(
                    thumbstick,
                    VirtualThumbsticks.LeftThumbstickCenter.Value - new Vector2(thumbstick.Width / 2f, thumbstick.Height / 2f),
                    Color.White);
            }

            if (VirtualThumbsticks.RightThumbstickCenter.HasValue)
            {
                spriteBatch.Draw(
                    thumbstick,
                    VirtualThumbsticks.RightThumbstickCenter.Value - new Vector2(thumbstick.Width / 2f, thumbstick.Height / 2f),
                    Color.White);
            }
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            switch (playMode)
            {
                case PlayMode.Playing:
                    PlayingDraw(spriteBatch);
                    break;

                case PlayMode.Paused:
                    PausedDraw(spriteBatch);
                    break;

                case PlayMode.HelpAndOptions:
                    HelpAndOptionsDraw(spriteBatch);
                    break;

                case PlayMode.Options:
                    OptionsDraw(spriteBatch);
                    break;
            }
            spriteBatch.End();
        }
    }
}
