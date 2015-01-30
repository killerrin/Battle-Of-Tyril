using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Advertising;
using Microsoft.Advertising.Mobile;
using Microsoft.Advertising.Mobile.Xna;
using System.Diagnostics;
using System.Device.Location;

namespace Battle_of_Tyril
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BoT : Microsoft.Xna.Framework.Game
    {
        #region Game Variables
        public static readonly string ApplicationId = "c2014049-b785-4eaf-b1a9-c11c2bacb820"; //"test_client";
        public static readonly string AdUnitId = "126039"; //"Image480_80"; //other test values: Image480_80, Image300_50, TextAd
        public static DrawableAd bannerAd;
        // We will use this to find the device location for better ad targeting.
        public static GeoCoordinateWatcher gcw = null;

        GraphicsDeviceManager graphics;
        public static GraphicsDevice graphicsDevice;

        SpriteBatch spriteBatch;

        Texture2D killerrinStudiosLogo;
        Texture2D blankTexture;
        Texture2D playerSelectionTexture;

        public static SpriteFont segoeFont;
        public static SpriteFont SpaceAndAstronomy;
        public static SpriteFont SpaceAndAstronomySmall;

        public static Touchpoints touchpoints;
        public static Random random;

        public static Rectangle screen;

        public static MainMenu mainMenu;
        public static OptionsMenu optionsMenu;
        public ModeSelectionMenu modeSelectionMenu;
        public ArcadeModeMenu arcadeModeMenu;
        public CampaignModeMenu campaignModeMenu;
        public static PlayGameMenu playGameMenu;
        public static GameOverMenu gameOverMenu;

            //-- Standard Variables --\\

        public static GameState gameState;

        public static bool developmentMode = true;
        public static string debugtext = "";

        //public static bool didMenuChange;
        public static bool fixedthumbstick;
        public static bool isMusicCurrentlyPlaying;

        float splashScreenTimer;
        const float SPLASH_SCREEN_TIMER = 1000f; // 1000 = 1 second

        //double menuChangeTimer;

        public static int screenWidth;
        public static int screenHeight;

        public static bool AdRemove;
        public static bool AdAdd;
        #endregion

        #region Initialization
        public BoT()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            ///--- Set the preferred display size ---\\\
            ////////----------Lumia 920----------\\\\\\\\
            //this.graphics.PreferredBackBufferWidth = 1280;
            //this.graphics.PreferredBackBufferHeight = 768;
            ///------------------------------------------\\\
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 480;
            this.graphics.IsFullScreen = true;

            ///--- Determine if control can be given to music. If the OS says music is playing, do not play music as per Windows Phone Application Guidelines. ---\\\
            isMusicCurrentlyPlaying = MediaPlayer.GameHasControl;

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            //-- Enable Options --\\
            TouchPanel.EnabledGestures = GestureType.Tap;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize the AdGameComponent with your ApplicationId and add it to the game.
            AdGameComponent.Initialize(this, ApplicationId);
            Components.Add(AdGameComponent.Current);
            // Now create an actual ad for display.
            CreateAd(new Rectangle((GraphicsDevice.Viewport.Bounds.Width - 480) / 2,5,480,80));
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        public void RemoveAd()
        {
            //AdComponent.Current.RemoveAll();
            AdGameComponent.Current.Visible = false;
            AdRemove = false;
        }
        public void AddAd()
        {
            AdGameComponent.Current.Visible = true;
            AdAdd = false;
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphicsDevice = GraphicsDevice;

            AdRemove = false;
            AdAdd = false;

            //-------------------------------------------------- Display The SplashScreen --------------------------------------------------------
            killerrinStudiosLogo = Content.Load<Texture2D>("Images/killerrinStudios");
            GraphicsDevice.Clear(Color.Black);
             
            spriteBatch.Begin();
            spriteBatch.Draw(killerrinStudiosLogo, new Vector2(120, 140), Color.White);
            spriteBatch.End();
            GraphicsDevice.Present();

            //-------------------------------------------------- Set the game variables ----------------------------------------------------------
            gameState = GameState.MainMenu;
            
            ///-- Display Settings --\\\
            screenWidth = TouchPanel.DisplayWidth;//GraphicsDevice.DisplayMode.Height;//GraphicsDevice.PresentationParameters.BackBufferWidth;//
            screenHeight = TouchPanel.DisplayHeight;// GraphicsDevice.DisplayMode.Width;//GraphicsDevice.PresentationParameters.BackBufferHeight;//
            screen = new Rectangle(0, 0, screenWidth, screenHeight);

            ///-- Extra --\\\
            fixedthumbstick = false;

            ///-- Random --\\\
            random = new Random();

            //-------------------------------------------------- Load the game content -----------------------------------------------------------
            //-- Extra --\\
            blankTexture = new Texture2D(GraphicsDevice, 1, 1);
            blankTexture.SetData(new Color[] { Color.White });
            touchpoints = new Touchpoints(blankTexture);

            ///-- Fonts --\\\
            segoeFont = Content.Load<SpriteFont>("Fonts/SegoeFont");
            SpaceAndAstronomy = Content.Load<SpriteFont>("Fonts/SpaceAndAstronomy");
            SpaceAndAstronomySmall = Content.Load<SpriteFont>("Fonts/SpaceAndAstronomy_Small");

            //--Menus--\\
            mainMenu = new MainMenu(Content);
            optionsMenu = new OptionsMenu(Content);
            modeSelectionMenu = new ModeSelectionMenu(Content);
            playGameMenu = new PlayGameMenu(Content, GraphicsDevice);
            arcadeModeMenu = new ArcadeModeMenu(Content, blankTexture);
            campaignModeMenu = new CampaignModeMenu(Content, blankTexture);
            gameOverMenu = new GameOverMenu(Content);

            playerSelectionTexture = Content.Load<Texture2D>("Images/Backgrounds/Menus/Player Selection");

            ///-- Sounds --\\\
            
            ///-- Music --\\\

            //-- ...Hold splashscreen an extra second..
            //System.Threading.Thread.Sleep(1000);
        }
        #endregion

        #region Game Loop
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // Update the touchpoints
            touchpoints.Update();

            switch (gameState)
            {
                case GameState.SplashScreen:
                    splashScreenTimer += gameTime.ElapsedGameTime.Milliseconds;
                    if (splashScreenTimer >= SPLASH_SCREEN_TIMER) { gameState = GameState.MainMenu; }
                    break;

                case GameState.MainMenu:
                    // Allows the game to exit
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    {
                        this.Exit();
                    }
                    if (mainMenu.CloseGame) 
                    { 
                        this.Exit(); 
                    }
                    mainMenu.Update(gameTime);

                    break;

                case GameState.Options:
                    optionsMenu.Update(gameTime);
                    break;

                case GameState.ModeSelect:
                    modeSelectionMenu.Update(gameTime);
                    break;

                case GameState.CampaignSelection:
                    campaignModeMenu.Update(gameTime);
                    break;

                case GameState.ArcadeSelection:
                    arcadeModeMenu.Update(gameTime);
                    break;

                case GameState.PlayGame:
                    if (playGameMenu.CloseGame) { this.Exit(); }
                    playGameMenu.Update(gameTime);
                    break;

                case GameState.GameOver:
                    gameOverMenu.Update(gameTime);
                    break;
            }

            if (AdRemove)
            {
                RemoveAd();
            }
            if (AdAdd)
            {
                AddAd();
            }
            //AdComponent.Current.Update(gameTime.ElapsedGameTime);

            //-- Debug code goes here --\\
            if (developmentMode == true)
            {
                
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (gameState)
            {
                case (GameState.SplashScreen):
                    DrawSplash(gameTime);
                    break;

                case GameState.MainMenu:
                    mainMenu.Draw(spriteBatch);
                    break;

                case GameState.Options:
                    optionsMenu.Draw(spriteBatch);
                    break;

                case GameState.ModeSelect:
                    modeSelectionMenu.Draw(spriteBatch);
                    break;

                case GameState.CampaignSelection:
                    campaignModeMenu.Draw(spriteBatch);
                    break;

                case GameState.ArcadeSelection:
                    arcadeModeMenu.Draw(spriteBatch);
                    //AdComponent.Current.Draw();
                    break;

                case (GameState.PlayGame):
                    playGameMenu.Draw(spriteBatch);
                    spriteBatch.Begin();
                    spriteBatch.End();
                    break;

                case (GameState.GameOver):
                    gameOverMenu.Draw(spriteBatch);
                    spriteBatch.Begin();
                    spriteBatch.End();
                    break;
            }
            
            ///-- Draw Debug Information --\\\
            spriteBatch.Begin();
            if (developmentMode == true)
            {
                RenderText.Draw(spriteBatch, (touchpoints.LastTappedX.ToString() + " | " + touchpoints.LastTappedY.ToString()), new Vector2(25.0f, 10.0f), Color.LimeGreen, segoeFont);
                RenderText.Draw(spriteBatch, (screenWidth.ToString() + " | " + screenHeight.ToString()), new Vector2(25.0f, 30.0f), Color.LimeGreen, segoeFont);

                RenderText.Draw(spriteBatch, (VirtualThumbsticks.LeftThumbstick).ToString(), new Vector2(175.0f, 10.0f), Color.LimeGreen, segoeFont);
                RenderText.Draw(spriteBatch, (VirtualThumbsticks.RightThumbstick).ToString(), new Vector2(175.0f, 30.0f), Color.LimeGreen, segoeFont);

                RenderText.Draw(spriteBatch, (playGameMenu.player.shootctr.ToString() + "/" + playGameMenu.player.MainWeapon.WeaponCounter.ToString()), new Vector2(25.0f, 50.0f), Color.LimeGreen, segoeFont);
                RenderText.Draw(spriteBatch, (debugtext), new Vector2(80.0f, 50.0f), Color.LimeGreen, segoeFont);
                //RenderText.Draw(spriteBatch, (currentLevel.Activated).ToString(), new Vector2(45.0f, 70.0f), Color.LimeGreen, segoeFont);

                touchpoints.Draw(spriteBatch);
            }
            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        private void DrawSplash(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(killerrinStudiosLogo, new Vector2((graphics.PreferredBackBufferWidth / 2f - (killerrinStudiosLogo.Width / 2.7f)), (graphics.PreferredBackBufferHeight / 2f - (killerrinStudiosLogo.Height / 2f))), null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
        #endregion

        #region Advertising Methods
        private void CreateAd(Rectangle rectangle)
        {
            // Create a banner ad for the game.
            int width = 480;
            int height = 80;
            int x = (GraphicsDevice.Viewport.Bounds.Width - width) / 2; // centered on the display
            int y = 5;
            //new Rectangle(x, y, width, height)
            bannerAd = AdGameComponent.Current.CreateAd(AdUnitId, rectangle, true);

            // Add handlers for events (optional).
            bannerAd.ErrorOccurred += new EventHandler<Microsoft.Advertising.AdErrorEventArgs>(bannerAd_ErrorOccurred);
            bannerAd.AdRefreshed += new EventHandler(bannerAd_AdRefreshed);

            // Set some visual properties (optional).
            //bannerAd.BorderEnabled = true; // default is true
            //bannerAd.BorderColor = Color.White; // default is White
            //bannerAd.DropShadowEnabled = true; // default is true

            // Provide the location to the ad for better targeting (optional).
            // This is done by starting a GeoCoordinateWatcher and waiting for the location to be available.
            // The callback will set the location into the ad. 
            // Note: The location may not be available in time for the first ad request.
            AdGameComponent.Current.Enabled = false;
            gcw = new GeoCoordinateWatcher();
            gcw.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(gcw_PositionChanged);
            gcw.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(gcw_StatusChanged);
            gcw.Start();

            AdGameComponent.Current.Enabled = true;
            AdGameComponent.Current.Visible = true;
        }

        /// <summary>
        /// This is called whenever a new ad is received by the ad client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bannerAd_AdRefreshed(object sender, EventArgs e)
        {
            Debug.WriteLine("Ad received successfully");
        }

        /// <summary>
        /// This is called when an error occurs during the retrieval of an ad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains the Error that occurred.</param>
        private void bannerAd_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            Debug.WriteLine("Ad error: " + e.Error.Message);
        }

        private void gcw_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            // Stop the GeoCoordinateWatcher now that we have the device location.
            gcw.Stop();

            bannerAd.LocationLatitude = e.Position.Location.Latitude;
            bannerAd.LocationLongitude = e.Position.Location.Longitude;

            AdGameComponent.Current.Enabled = true;

            Debug.WriteLine("Device lat/long: " + e.Position.Location.Latitude + ", " + e.Position.Location.Longitude);
        }

        private void gcw_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            if (e.Status == GeoPositionStatus.Disabled || e.Status == GeoPositionStatus.NoData)
            {
                // in the case that location services are not enabled or there is no data
                // enable ads anyway
                AdGameComponent.Current.Enabled = true;
                Debug.WriteLine("GeoCoordinateWatcher Status :" + e.Status);
            }
        }

        /// <summary>
        /// Clean up the GeoCoordinateWatcher
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (gcw != null)
                {
                    gcw.Dispose();
                    gcw = null;
                }
            }
        }
        #endregion

        #region Helper Methods

        #endregion

        #region Unload Content
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        #endregion
    }
}
