using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Infinite_Runner.KanataEditor;

namespace Infinite_Runner.KanataEngine
{
    class Animation 
    {
        public static GraphicsDevice graphicsDevice;

        public string name = "Animation";
        // The set of texture2ds that attached to the Animation
        public Texture2D[] sprites { get; private set; }
        // The current texture2D used by the Animation.
        public Texture2D sprite { get; private set; }
        // The speed rate to animate ex. sampling = 12 --> 12 time animating per second 
        private int _sampling = 12;
        public int sampling
        {
            get { return _sampling; }
            set
            {
                // Check if new value is negative 
               if (value < 0) return;
               
                _sampling = value;
                // Recalculate max time that renderer will update its sprite 
                maxTime = 1f / sampling;
            }
        }

        float maxTime = 1f; 
        int currentFrame = 0;
        float accumulator = 0f;

        public static Animation CreateAnimation(Texture2D atlase, Rectangle normal, Rectangle size, string name="Anim")
        {
            Animation animation = new Animation();

            Texture2D[] animationSprite = new Texture2D[size.X * size.Y];

            // sampling each sprite from the atlase texture 
            for (int colunm = 0; colunm < size.X; colunm++)
            {
                for (int row = 0; row < size.Y; row++)
                {
                    Rectangle cropRect = 
                        new Rectangle(
                            normal.X + (colunm * (normal.Width + size.Width)),
                            normal.Y + (row * (normal.Height + size.Height)),
                            size.Width,
                            size.Height);

                    animationSprite[row * size.X + colunm] =
                        KanataTool.CropTexture2D(
                            graphicsDevice,
                            atlase,
                            cropRect);
                }
            }

            // setting the new animation 
            animation.name = name;
            animation.sprites = animationSprite;
            animation.sprite = animationSprite[0];

            return animation;

        }

        public void Reset()
        {
            accumulator = 0f;
            currentFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            accumulator += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (accumulator >= maxTime)
            {
                // reset accumulator
                accumulator = 0f;
                
                // update next sprite 
                if (++currentFrame == sprites.Length)
                    currentFrame = 0;
                sprite = sprites[currentFrame];

            }

        }

    }
}
