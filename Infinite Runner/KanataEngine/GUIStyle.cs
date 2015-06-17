using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Infinite_Runner.KanataEngine
{
    class GUIStyle
    {
        public class GUIContent
        {
            public Texture2D backGround;
            public Color fontColor = Color.Black;

        }

        public String name = "New Style";
        public GUIContent normal = new GUIContent();
        public GUIContent hover = new GUIContent();
        public GUIContent active = new GUIContent();
        public SpriteFont font;
        public float fontSize = 1f;

        /// <summary>
        /// Constructor
        /// </summary>
        public GUIStyle(String name, Texture2D normalText, Texture2D hoverText, Texture2D activeText, SpriteFont spFont)
        {
            // Initialization of the new style
            this.name = name;
            normal.backGround = normalText;
            hover.backGround = hoverText;
            active.backGround = activeText;
            font = spFont;

        }

        public GUIStyle() { }

    }
}
