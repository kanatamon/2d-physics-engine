using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Infinite_Runner.KanataEngine
{
    class Camera
    {
        // The current camera , displaying now! to user
        public static Camera main;

        // Device that the game is running
        public readonly GraphicsDevice graphicsDevice;
        
        // Transformation 
        public Matrix transformation { get; private set; }
       
        // The scale of camera 
        // ,zoom > 1 : zoom-in
        // ,zoom < 0 : zoom-out
        protected float _zoom; 
        public float zoom
        {
            get { return _zoom; }
            set 
            { 
                _zoom = value;

                if (_zoom < 0.1f) // Negative zoom will flip image 
                    _zoom = 0.1f;

                // Recalculate the tranformation
                CalculateTransformation();
            } 
        }

        // The position of the camera , represent on the game's wolrd
        private Vector2 _postion = Vector2.Zero;
        public Vector2 position
        {
            get { return _postion; }
            set
            {
                _postion = value;

                // Recalculate the tranformation
                CalculateTransformation();
            }
        }

        // The roation of the camera(degree's)
        private float _rotation = 0f;
        public float rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;

                // Recalculate the tranformation
                CalculateTransformation();
            }
        }

        
        /**** Constructor ****/
        /*
        public Camera()
        {
            zoom = 1.0f;
            rotation = 0f;
            position = Vector2.Zero;

        }*/

        public Camera(GraphicsDevice device)
        {
            graphicsDevice = device;

            zoom = 1.0f;
            rotation = 0f;
            position = Vector2.Zero;

        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            position += amount;
        }

        // ***Optimization here & Renderer to reduce if no any change on camera  
        private void CalculateTransformation()
        {
            // Alias the value
            float viewportWidth = graphicsDevice.Viewport.Width;
            float viewportHeight = graphicsDevice.Viewport.Height;

            transformation = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) 
                * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation)) 
                * Matrix.CreateScale(new Vector3(zoom, zoom, 1)) 
                * Matrix.CreateTranslation(new Vector3(viewportWidth * 0.5f, viewportHeight * 0.5f, 0));
        }

        /// <summary>
        /// Transforms position from world space into screen space.  
        /// </summary>
        public Point WorldToScreen(Vector2 worldPos)
        {
            //  (-world_W,-world_H)------------->
            //  |                        X      |   X ,position in world space
            //  |                               |
            //  |           WORLD               |
            //  |                               |
            //  |                               |
            //  v---------------(world_W,world_H)                  
            //                  ||
            //                  \/
            //  ---------------------------------
            //  |    worldToScreen's Matrix     |
            //  ---------------------------------  
            //                  ||
            //                  \/
            // (0,0)---------------------------->
            //  |                        X      |   *** X ,Now! resulted = position in screen space
            //  |                               |
            //  |           SCREEN              |
            //  |                               |
            //  |                               |
            //  v-------------(screen_W,screen_H)
            
            // Calculate matrix which transform world to screen  
            Matrix worldToScreen_t = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) 
                * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation)) 
                * Matrix.CreateScale(new Vector3(zoom, zoom, 1)) 
                * Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));

            // Calulate position on screen-point 
            Vector2 screenPoint = Vector2.Transform(worldPos, worldToScreen_t);

            return new Point((int)screenPoint.X, (int)screenPoint.Y);   
        }

        /// <summary>
        /// Transforms position from screen space into world space. 
        /// </summary>
        public Vector2 ScreenToWorld(Point screenPoint)
        {
            // (0,0)---------------------------->
            //  |                        X      |   X ,is position in screen space
            //  |                               |
            //  |           SCREEN              |
            //  |                               |
            //  |                               |
            //  v-------------(screen_W,screen_H)
            //                  ||
            //                  \/
            //  ---------------------------------
            //  |     screenToWorld's Matrix    |  
            //  ---------------------------------                                             
            //                  ||
            //                  \/
            //  (-world_W,-world_H)------------->
            //  |                        X      |  *** X ,Now! resulted = position in world space
            //  |                               |
            //  |           WORLD               |
            //  |                               |
            //  |                               |
            //  v---------------(world_W,world_H)                  
           
            // Calculate matrix which transform screen to world  
            Matrix screenToWord_t =
                Matrix.CreateTranslation(new Vector3(-graphicsDevice.Viewport.Width * 0.5f, -graphicsDevice.Viewport.Height * 0.5f, 0))
                * Matrix.CreateScale(new Vector3(1f / zoom, 1f / zoom, 1f))
                * Matrix.CreateRotationZ(-MathHelper.ToRadians(rotation))
                * Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0f));

            return Vector2.Transform(new Vector2(screenPoint.X, screenPoint.Y), screenToWord_t);
        }

    }
}
