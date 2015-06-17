using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Infinite_Runner.KanataEngine
{
    class Skin
    {
        // The styles store uable skin for global. 
        List<GUIStyle> styles = new List<GUIStyle>();

        /// <summary>
        /// Add the style to the global. 
        /// </summary>
        public void AddStyle(GUIStyle style)
        {
            styles.Add(style);
        }

        /// <summary>
        /// Find the style by name ,return.
        /// </summary>
        public GUIStyle GetStlye(String styleName)
        {
            // Loop 
            foreach (GUIStyle style in styles)
            {
                if (style.name.Equals(styleName))
                {
                    return style;
                }
            }

            Console.WriteLine("Not found");

            return null; ;
        }

    }
}
