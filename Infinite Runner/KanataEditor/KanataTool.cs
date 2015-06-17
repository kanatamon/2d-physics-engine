using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Infinite_Runner.KanataEngine;

namespace Infinite_Runner.KanataEditor
{
    static class KanataTool
    {
        /// <summary>
        /// Create the texture2d from cropping another texture2D with specific source, return. 
        /// </summary>
        public static Texture2D CropTexture2D(GraphicsDevice graphicsDevice, Texture2D origin, Rectangle source) 
        {
            // Create empty texture
            Texture2D cropTexture = new Texture2D(graphicsDevice, source.Width, source.Height);
            // Create empty color
            Color[] data = new Color[source.Width * source.Height];
            // Get data from origin with source
            origin.GetData(0, source, data, 0, data.Length);
            // Apply data 
            cropTexture.SetData(data);

            return cropTexture;
        }

    }
}
