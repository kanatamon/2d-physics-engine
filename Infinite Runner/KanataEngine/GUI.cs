using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;

namespace Infinite_Runner.KanataEngine
{
    static class GUI
    {
        public static bool enabled = true;
        public static SpriteBatch spriteBatch;

        public static Color color = Color.White;

        public static Skin skin;

        // The empty GUIStyle 
        static GUIStyle theStyle = new GUIStyle();

        public static SamplerState sampleState = SamplerState.PointClamp;

        #region Label 
        /// <summary>
        /// Make a texture label on screen.
        /// </summary>
        public static void Label(Rectangle rect, Texture2D image) 
        {
            // Start Drawing
            spriteBatch.Begin(SpriteSortMode.Immediate, 
                BlendState.AlphaBlend,
                sampleState, 
                null, 
                null, 
                null);
            /*
            spriteBatch.Draw(
                image,
                rect,
                color
            );*/
            
            spriteBatch.Draw(
                image,
                rect,
                null,
                color,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                1f);
            
            spriteBatch.End();
        }

        /// <summary>
        /// Make a texture label on screen.
        /// </summary>
        public static void Label(Rectangle rect, GUIStyle style) 
        {
            Label(rect, style.normal.backGround);
        }

        /// <summary>
        /// Make a text or texture label on screen.
        /// </summary>
        public static void Label(Rectangle rect, String text, GUIStyle style) 
        {
            // Draw texture 
            Label(rect, style.normal.backGround);

            // Calculate parameter for the text offset
            //Vector2 offset = style.font.MeasureString(text);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            
            // Draw the text
            spriteBatch.DrawString(
                style.font,
                text,
                new Vector2(rect.X, rect.Y),
                style.normal.fontColor,
                0f,
                Vector2.Zero,
                style.fontSize,
                SpriteEffects.None,
                0f);

            spriteBatch.End();
        }

        #endregion

        #region Box fucntion 

        /// <summary>
        /// Make a graphical box.
        /// </summary>
        public static void Box(Rectangle rect, Texture2D image) 
        {
            if (rect.Contains(Input.mousePosition))
            {
                Color preColor = color;
                color = Color.LightGray;
                Label(rect, image);
                color = preColor;
            }
            else
            {
                Label(rect, image);
            }
                
        }
        /// <summary>
        /// Make a graphical box.
        /// </summary>
        public static void Box(Rectangle rect, GUIStyle style)
        {
            // Check if mouse is on the box 
            if (rect.Contains(Input.mousePosition)) 
                Label(rect, style.hover.backGround);
            else
                Label(rect, style.normal.backGround);
            
        }

        /// <summary>
        /// Make a graphical box.
        /// </summary>
        public static void Box(Rectangle rect, String text, GUIStyle style)
        {
            // Using the empty GUIStyle 
            theStyle.font = style.font;
            theStyle.fontSize = style.fontSize;

            // Check if the mouse is on the box 
            if (rect.Contains(Input.mousePosition))
            {
                theStyle.normal.backGround = style.hover.backGround;
                theStyle.normal.fontColor = style.hover.fontColor;

            }
            else
            {
                theStyle.normal.backGround = style.normal.backGround;
                theStyle.normal.fontColor = style.normal.fontColor;
            }

            // Draw button
            Label(rect, text, theStyle);

        }
        #endregion

        #region Button function
        /// <summary>
        /// Make a single press button , The user clicks them and something happens immdediately.
        /// </summary>
        public static bool Button(Rectangle rect, Texture2D image)
        {
            bool isActive = false;
            // Save color , used to restore color back at the end of this method
            Color preColor = color;

            // Check if the mouse is on the button
            if (rect.Contains(Input.mousePosition))
            {
                // Check if mouse is released on this rectangle , set active 
                isActive = Input.isMouseRelease;
                // Check if mouse is pressing , change the color 
                color = Input.isMouse ? Color.RoyalBlue : Color.LightGray;

            }

            // Draw button
            Label(rect, image);
            // Restore color 
            color = preColor;

            return isActive;
        }

        /// <summary>
        /// Make a single press button , The user clicks them and something happens immdediately.
        /// </summary>
        public static bool Button(Rectangle rect, GUIStyle style)
        {
            bool isActive = false;
            // Set background , normal background
            Texture2D backGround = style.normal.backGround;

            if (rect.Contains(Input.mousePosition))
            {
                // Check if mouse is released on this rectangle , set active 
                isActive = Input.isMouseRelease;
                // Check if mouse is pressing then set background 
                backGround = Input.isMouse ? style.active.backGround : style.hover.backGround;

            }

            // Draw button
            Label(rect, backGround);

            return isActive;

        }

        /// <summary>
        /// Make a single press button , The user clicks them and something happens immdediately.
        /// </summary>
        public static bool Button(Rectangle rect, String text, GUIStyle style)
        {
            bool isActive = false;

            theStyle.font = style.font;
            theStyle.fontSize = style.fontSize;

            if (rect.Contains(Input.mousePosition))
            {
                isActive = Input.isMouseRelease;

                // Check if mouse is pressing set font color and background 
                if (Input.isMouse)
                {
                    theStyle.normal.backGround = style.active.backGround;
                    theStyle.normal.fontColor = style.active.fontColor;
                }
                else
                {
                    theStyle.normal.backGround = style.hover.backGround;
                    theStyle.normal.fontColor = style.hover.fontColor;
                }

            }
            else
            {
                // If mouse is not on the rectangle set it normal
                theStyle.normal.backGround = style.normal.backGround;
                theStyle.normal.fontColor = style.normal.fontColor;

            }

            // Draw button
            Label(rect, text, theStyle);

            return isActive;
        }

        #endregion

        /// <summary>
        /// Draw a texture within a rectangle.
        /// </summary>
        public static void DrawTexture() { }

        /// <summary>
        /// Make a text or texture label on screen.
        /// </summary>
        public static void Label() { }

        /// <summary>
        /// Make a multi-line text field where the user can edit a string.
        /// </summary>
        public static void TextArea() { }

        /// <summary>
        /// Make a single-line text field where the user can edit a string. 
        /// </summary>
        public static void TextField() { }

      

        
    }

}
