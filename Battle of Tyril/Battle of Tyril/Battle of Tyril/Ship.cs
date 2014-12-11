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
    public class Ship
    {
        public Rectangle ScreenSize { get; set; }
        public Rectangle CollideRectangle { get; set; }

        public Texture2D Texture { get; set; }

        public Color Colour { get; set; }

        public const double MaxSpeed = 10.0;

        public float Rotation;
        public float Acceleration { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }

        public Vector2 SpriteOrigin { get; set; }

        public Weapon MainWeapon { get; set; }
        public double Health { get; set; }

        public Random random;

        public Ship(Texture2D texture)
        {
            Texture = texture;
            ScreenSize = new Rectangle(0, 0, TouchPanel.DisplayWidth, TouchPanel.DisplayHeight);
            Rotation = 0.0f;

            SpriteOrigin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }
        public Ship(Vector2 location, Texture2D texture)
        {
            Position = location;
            Texture = texture;
            Acceleration = 0.75f;
            ScreenSize = new Rectangle(0, 0, TouchPanel.DisplayWidth, TouchPanel.DisplayHeight);
            Colour = Color.White;

            SpriteOrigin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Rotation = 0.0f;
            CollideRectangle = new Rectangle((int)location.X, (int)location.Y, texture.Width, texture.Height);
        }
        public Ship(Vector2 location, float acceletation, Texture2D texture) 
        {
            Position = location;
            Texture = texture;
            Acceleration = acceletation;
            ScreenSize = new Rectangle(0, 0, TouchPanel.DisplayWidth, TouchPanel.DisplayHeight);
            Colour = Color.White;

            SpriteOrigin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Rotation = 0.0f;
            CollideRectangle = new Rectangle((int)location.X, (int)location.Y, texture.Width, texture.Height);
        }
        public Ship(Vector2 location, float acceletation, Texture2D texture, Rectangle screensize)
        {
            Position = location;
            Texture = texture;
            Acceleration = acceletation;
            ScreenSize = screensize;
            Colour = Color.White;

            SpriteOrigin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Rotation = 0.0f;
            CollideRectangle = new Rectangle((int)location.X, (int)location.Y, texture.Width, texture.Height);
        }
        public Ship(Vector2 location, float acceletation, Texture2D texture, Rectangle screensize, Color colour)
        {
            Position = location;
            Texture = texture;
            Acceleration = acceletation;
            ScreenSize = screensize;
            Colour = colour;

            SpriteOrigin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Rotation = 0.0f;
            CollideRectangle = new Rectangle((int)location.X, (int)location.Y, texture.Width, texture.Height);
        }

        public bool CheckCollision(Rectangle rect)
        {
            if (CollideRectangle.Intersects(rect))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IntersectPixels(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }

        public static bool IntersectPixels(
                        Matrix transformA, int widthA, int heightA, Color[] dataA,
                        Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }

        //is there a collision with another sprite?
        public virtual bool CheckPixelPerfectColision(Bullet bullet)
        {
            //if (CollisionRectangle.Intersects(sprite.CollisionRectangle))
            //{
            //    Color[] personTextureData;
            //    Color[] blockTextureData;

            //    blockTextureData = new Color[TextureImage.Width * TextureImage.Height];
            //    sprite.TextureImage.GetData(blockTextureData);
            //    personTextureData = new Color[TextureImage.Width * TextureImage.Height];
            //    TextureImage.GetData(personTextureData);

            //    return (IntersectPixels(CollisionRectangle, personTextureData, sprite.CollisionRectangle, blockTextureData));
            //}
            bool hit = false;

            // Update the passed object's transform
            // SEQUENCE MATTERS HERE - DO NOT REARRANGE THE ORDER OF THE TRANSFORMATIONS BELOW
            Matrix spriteTransform =
                Matrix.CreateTranslation(new Vector3(-bullet.MainWeapon.SpriteOrigin, 0.0f)) *
                Matrix.CreateScale(1f) *  //would go here
                Matrix.CreateRotationZ(0f) *
                Matrix.CreateTranslation(new Vector3(bullet.Position, 0.0f));

            // Build the calling object's transform
            // SEQUENCE MATTERS HERE - DO NOT REARRANGE THE ORDER OF THE TRANSFORMATIONS BELOW
            Matrix thisTransform =
                Matrix.CreateTranslation(new Vector3(-SpriteOrigin, 0.0f)) *
                Matrix.CreateScale(1f) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateTranslation(new Vector3(Position, 0.0f));

            // Calculate the bounding rectangle of the passed object in world space
            //For the bounding rectangle, can't use CollisionRectangle property because
            //it adjusts for origin and scale, transform does both of those things for us, so 
            //we just need a simple bounding rectangle here

            //With transformations, don't use position here for X and Y, the transformation does that for you
            //also don't scale it or use origin, transformation does those things too          
            Rectangle spriteRectangle = CalculateBoundingRectangle(
                     new Rectangle(0, 0, Convert.ToInt32(bullet.MainWeapon.Texture.Width), Convert.ToInt32(bullet.MainWeapon.Texture.Height)),
                     spriteTransform);

            // Calculate the bounding rectangle of the calling object in world space
            Rectangle thisRectangle = CalculateBoundingRectangle(
                     new Rectangle(0, 0, Convert.ToInt32(Texture.Width), Convert.ToInt32(Texture.Height)),
                     thisTransform);

            // The per-pixel check is expensive, so check the bounding rectangles
            // first to prevent testing pixels when collisions are impossible.
            if (thisRectangle.Intersects(spriteRectangle))
            {
                // The color data for the images; used for per-pixel collision
                Color[] thisTextureData;        //calling object
                Color[] spriteTextureData;		//passed object

                // Extract collision data from calling object
                thisTextureData =
                    new Color[Texture.Width * Texture.Height];
                Texture.GetData(thisTextureData);

                // Extract collision data from passed object
                spriteTextureData =
                    new Color[bullet.MainWeapon.Texture.Width * bullet.MainWeapon.Texture.Height];
                bullet.MainWeapon.Texture.GetData(spriteTextureData);

                // Check collision 
                if (IntersectPixels(spriteTransform, bullet.MainWeapon.Texture.Width,
                        bullet.MainWeapon.Texture.Height, spriteTextureData,
                        thisTransform, Texture.Width,
                        Texture.Height, thisTextureData
                        ))
                {
                    //if per pixel is true, return true from the method
                    hit = true;
                }
            }
            //this will be false if there was no rectangle collision or if
            //there was a rectangle collision, but no per pixel collision 
            return hit;
        }

        public virtual bool CheckPixelPerfectColision(Ship ship)
        {
            //if (CollisionRectangle.Intersects(sprite.CollisionRectangle))
            //{
            //    Color[] personTextureData;
            //    Color[] blockTextureData;

            //    blockTextureData = new Color[TextureImage.Width * TextureImage.Height];
            //    sprite.TextureImage.GetData(blockTextureData);
            //    personTextureData = new Color[TextureImage.Width * TextureImage.Height];
            //    TextureImage.GetData(personTextureData);

            //    return (IntersectPixels(CollisionRectangle, personTextureData, sprite.CollisionRectangle, blockTextureData));
            //}
            bool hit = false;

            // Update the passed object's transform
            // SEQUENCE MATTERS HERE - DO NOT REARRANGE THE ORDER OF THE TRANSFORMATIONS BELOW
            Matrix spriteTransform =
                Matrix.CreateTranslation(new Vector3(-ship.SpriteOrigin, 0.0f)) *
                Matrix.CreateScale(1f) *  //would go here
                Matrix.CreateRotationZ(0f) *
                Matrix.CreateTranslation(new Vector3(ship.Position, 0.0f));

            // Build the calling object's transform
            // SEQUENCE MATTERS HERE - DO NOT REARRANGE THE ORDER OF THE TRANSFORMATIONS BELOW
            Matrix thisTransform =
                Matrix.CreateTranslation(new Vector3(-SpriteOrigin, 0.0f)) *
                Matrix.CreateScale(1f) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateTranslation(new Vector3(Position, 0.0f));

            // Calculate the bounding rectangle of the passed object in world space
            //For the bounding rectangle, can't use CollisionRectangle property because
            //it adjusts for origin and scale, transform does both of those things for us, so 
            //we just need a simple bounding rectangle here

            //With transformations, don't use position here for X and Y, the transformation does that for you
            //also don't scale it or use origin, transformation does those things too          
            Rectangle spriteRectangle = CalculateBoundingRectangle(
                     new Rectangle(0, 0, Convert.ToInt32(ship.Texture.Width), Convert.ToInt32(ship.Texture.Height)),
                     spriteTransform);

            // Calculate the bounding rectangle of the calling object in world space
            Rectangle thisRectangle = CalculateBoundingRectangle(
                     new Rectangle(0, 0, Convert.ToInt32(Texture.Width), Convert.ToInt32(Texture.Height)),
                     thisTransform);

            // The per-pixel check is expensive, so check the bounding rectangles
            // first to prevent testing pixels when collisions are impossible.
            if (thisRectangle.Intersects(spriteRectangle))
            {
                // The color data for the images; used for per-pixel collision
                Color[] thisTextureData;        //calling object
                Color[] spriteTextureData;		//passed object

                // Extract collision data from calling object
                thisTextureData =
                    new Color[Texture.Width * Texture.Height];
                Texture.GetData(thisTextureData);

                // Extract collision data from passed object
                spriteTextureData =
                    new Color[ship.Texture.Width * ship.Texture.Height];
                ship.Texture.GetData(spriteTextureData);

                // Check collision 
                if (IntersectPixels(spriteTransform, ship.Texture.Width,
                        ship.Texture.Height, spriteTextureData,
                        thisTransform, Texture.Width,
                        Texture.Height, thisTextureData
                        ))
                {
                    //if per pixel is true, return true from the method
                    hit = true;
                }
            }
            //this will be false if there was no rectangle collision or if
            //there was a rectangle collision, but no per pixel collision 
            return hit;
        }

        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle, Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                        (int)(max.X - min.X), (int)(max.Y - min.Y));
        }


        public void ClampToScreen()
        {
            if (Position.X < 0)
            {
                //Position.X = -ScreenSize.Width / 2f;
                Position = new Vector2(0, Position.Y);
                if (Velocity.X < 0f)
                {
                    //Velocity.X = 0f;
                    Velocity = new Vector2(0f, Velocity.Y);
                }
            }

            if (Position.X > ScreenSize.Width)
            {
                //Position.X = ScreenSize.Width / 2f;
                Position = new Vector2(ScreenSize.Width, Position.Y);
                if (Velocity.X > 0f)
                {
                    //Velocity.X = 0f;
                    Velocity = new Vector2(0f, Velocity.Y);
                }
            }

            if (Position.Y < 0)
            {
                //Position.Y = -ScreenSize.Height / 2f;
                Position = new Vector2(Position.X, 0);
                if (Velocity.Y < 0f)
                {
                    //Velocity.Y = 0f;
                    Velocity = new Vector2(Velocity.X, 0f);
                }
            }

            if (Position.Y > ScreenSize.Height)
            {
                //Position.Y = ScreenSize.Height / 2f;
                Position = new Vector2(Position.X, ScreenSize.Height);
                if (Velocity.Y > 0f)
                {
                    //Velocity.Y = 0f;
                    Velocity = new Vector2(Velocity.X, 0f);
                }
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            // add our velocity to our position to move the ship
            Position += Velocity * gameTime.ElapsedGameTime.Milliseconds;
        }

        public virtual Bullet Shoot()
        {
            return (new Bullet());
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Colour, Rotation, new Vector2(Texture.Width / 2f, Texture.Height / 2f), 1f, SpriteEffects.None, 0f);
            //texture,
            //Position,
            //null,
            //Color.White,
            //Rotation,
            //new Vector2(texture.Width / 2f, texture.Height / 2f),
            //1f,
            //SpriteEffects.None,
            //0f);
        }
        public virtual void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(Texture, Position, color);
        }

    }
}
