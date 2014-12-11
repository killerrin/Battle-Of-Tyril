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
    public class AIShip:Ship
    {
        enum AIState
        {
            Spawning,
            RandomMovement,
            Chasing
        }

        enum RandomMovementState
        {
            GetRandomPosition,
            MoveToRandomPosition
        }

        private AIState state;
        private RandomMovementState movementState;
        
        private int shootctr { get; set; }

        private float range;
        private Rectangle sightRange;

        private Vector2 randMovement;
        private bool pathFindX;
        private bool pathFindY;

        public int EnemyType { get; private set; }
               // Position = new Vector2(0, 0);
               // Acceleration = 0.75f;
               // Colour = Color.White;
               // CollideRectangle = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);

        public AIShip(Texture2D texture, Random r)
            :base(texture)
        {
            random = r;

            //int temp = random.Next(0, 3);
            switch (random.Next(0,3))
            {
                case 0:
                    //Colour = new Color(100f, 100f, 90f, 100f);
                    Colour = Color.Silver;
                    Acceleration = 0.40f;// 0.70f
                    Health = 80.0f;
                    range = 100f;
                    EnemyType = 0;
                    break;
                case 1:
                    //Colour = new Color(56f, 120f, 204f, 100f);
                    Colour = Color.Sienna;
                    Acceleration = 0.35f;//0.65f
                    Health = 70.0f;
                    range = 50f;
                    EnemyType = 1;
                    break;
                case 2:
                    //Colour = new Color(169f, 19f, 19f, 100f);
                    Colour = Color.BlanchedAlmond;
                    Acceleration = 0.20f;//0.60f
                    Health = 50.0f;
                    range = 40f;
                    EnemyType = 2;
                    break;
                default:
                    Colour = new Color(255f, 242f, 67f, 100f);
                    Acceleration = 0.15f;//0.55f
                    Health = 50.0f;
                    range = 40f;
                    EnemyType = 3;
                    break;
            }

            //Acceleration = 25f;

            //temp = random.Next(0, 4);
            switch (random.Next(0,4))
            {
                case 0:
                    Position = new Vector2(random.Next(0, 226), random.Next(0, (480 - texture.Height)));
                    break;
                case 1:
                    Position = new Vector2(random.Next(574, (800-texture.Width)), random.Next(0, (480-texture.Height)));
                    break;
                case 2:
                    Position = new Vector2(random.Next(0, (800 - texture.Width)), random.Next(0, 140));
                    break;
                case 3:
                    Position = new Vector2(random.Next(0, (800 - texture.Width)), random.Next(340, (480-texture.Height)));
                    break;
                default:
                    Position = new Vector2(0, 0);
                    break;
            }

            sightRange = new Rectangle ((int)(Position.X - range), (int)(Position.Y - range), (int)(range + Texture.Width + range), (int)(range + Texture.Height + range));
            CollideRectangle = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            
            shootctr = 0;
            pathFindX = true;
            pathFindY = true;

            randMovement = Vector2.Zero;

            state = AIState.RandomMovement;
            movementState = RandomMovementState.GetRandomPosition;
        }

        public override void Update(GameTime gameTime)
        {
            sightRange.X = (int)(Position.X - range);
            sightRange.Y = (int)(Position.Y - range);

            switch (state)
            {
                case AIState.Spawning:
                    SpawningUpdate(gameTime);
                    break;
                case AIState.RandomMovement:
                    RandomMovementUpdate(gameTime, BoT.playGameMenu.player);
                    break;
                case AIState.Chasing:
                    ChasePlayerUpdate(gameTime, BoT.playGameMenu.player);
                    break;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Colour);
        }

        private void SpawningUpdate(GameTime gameTime)
        {
            /// To be added at a later date.
        }

        private void ChasePlayerUpdate(GameTime gameTime, PlayerShip player)
        {

            sightRange = new Rectangle((int)(Position.X - range), (int)(Position.Y - range), (int)(range + Texture.Width + range), (int)(range + Texture.Height + range));
            if (!sightRange.Contains(player.CollideRectangle)) { state = AIState.RandomMovement; RandomMovementUpdate(gameTime, player); return; }

            Vector2 temp = Vector2.Zero;

            float x = 0f;
            if (Position.X <= player.Position.X) { x = +Acceleration; }
            else if (Position.X >= player.Position.X) { x = -Acceleration; }
            temp.X = x;

            float y = 0f;
            if (Position.Y <= player.Position.Y) { y = +Acceleration; }
            else if (Position.Y >= randMovement.Y) { y = -Acceleration; }
            temp.Y = y;

            Velocity = temp;
        }

        private void RandomMovementUpdate(GameTime gameTime, PlayerShip player)
        {
            float movementPointRadiusCheck = 25f;

            sightRange = new Rectangle((int)(Position.X - range), (int)(Position.Y - range), (int)(range + Texture.Width + range), (int)(range + Texture.Height + range));
            if (sightRange.Contains(player.CollideRectangle))
            {
                state = AIState.Chasing;
                movementState = RandomMovementState.GetRandomPosition;
                ChasePlayerUpdate(gameTime, player);
                return;
            }

            switch (movementState)
            {
                case RandomMovementState.GetRandomPosition:
                    randMovement = new Vector2(BoT.random.Next(0, ScreenSize.Width), BoT.random.Next(0, ScreenSize.Height));
                    pathFindX = true;
                    pathFindY = true;
                    movementState = RandomMovementState.MoveToRandomPosition;
                    break; 

                case RandomMovementState.MoveToRandomPosition:
                    if (sightRange.Contains(player.CollideRectangle)) { state = AIState.Chasing; ChasePlayerUpdate(gameTime, player); return; }

                    Vector2 temp= Vector2.Zero;
                    if (pathFindX)
                    {
                        float x = 0f;
                        if (Position.X <= randMovement.X) { x = +Acceleration; }
                        else if (Position.X >= randMovement.X) { x = -Acceleration; }

                        temp.X = x;

                        if (Position.X <= (randMovement.X + movementPointRadiusCheck) && Position.X >= randMovement.X - movementPointRadiusCheck) { pathFindX = false; }
                    } else { temp.X = 0f; }
                    if (pathFindY)
                    {
                        float y = 0f;
                        if (Position.Y <= randMovement.Y) { y = +Acceleration; }
                        else if (Position.Y >= randMovement.Y) { y = -Acceleration; }

                        temp.Y = y;

                        if (Position.Y <= (randMovement.Y + movementPointRadiusCheck) && Position.Y >= randMovement.Y - movementPointRadiusCheck) { pathFindY = false; }
                    } else { temp.Y = 0f; }

                    Velocity = temp;
                    if (!pathFindX && !pathFindY) { movementState = RandomMovementState.GetRandomPosition; }
                    break;
            }
        }

        //------------------- Draws -------------------\\
        private void SpawningDraw(SpriteBatch spriteBatch)
        {

        }

        private void ChasePlayerDraw(SpriteBatch spriteBatch)
        {

        }

        private void RandomMovementDraw(SpriteBatch spriteBatch)
        {

        }

        public static List<AIShip> SpawnX(Texture2D texture, int numberOfShips)
        {
            List<AIShip> temp = new List<AIShip> { };

            for (int i = 0; i < numberOfShips; i++)
            {
                temp.Add(new AIShip(texture, BoT.random));
            }

            return (temp);
        }
    }
}
