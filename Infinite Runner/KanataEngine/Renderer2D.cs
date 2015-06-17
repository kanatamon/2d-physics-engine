using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Infinite_Runner.KanataEngine
{
    class Renderer2D : Component
    {   const float deg2rad = -0.01745329f;
        // The center point of the sprite
        public Vector2 origin = Vector2.Zero;
        // Alpha value for Draw method 
        public float alpha = 1.0f;
        // The sprite texture 
        private Texture2D _sprite;
        public Texture2D sprite
        {
            get { return _sprite; }

            set 
            {
                _sprite = value;

                // Recalculate origin
                origin = new Vector2(_sprite.Width / 2f, _sprite.Height / 2f);
            }
        }

        public SamplerState sampleState = SamplerState.PointClamp;
        public SpriteEffects spriteEffects = SpriteEffects.None;
        //Vector2 r = new Vector2(1.2f, 1.2f);
        
        /// <summary>
        /// Draw the sprite
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                        BlendState.AlphaBlend,
                        sampleState,
                        null,
                        null,
                        null,
                        Camera.main.transformation);

            spriteBatch.Draw(
                sprite,                             // Texture
                gameObject.position,                // Position
                null,                               // Source rectangle 
                Color.White * alpha,                // Color
                MathHelper.ToRadians(gameObject.rotation),      // Rotation , covert : degree to radian
                origin,                             // Origin
                gameObject.scale,                   // Scale 
                spriteEffects,                      // Mirroring effect 
                0);                                 // Depth
       
            spriteBatch.End(); // Call Sprite Batch End
        }

    }
}
