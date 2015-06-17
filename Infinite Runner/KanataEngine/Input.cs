using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Infinite_Runner.KanataEngine
{
    static class Input
    {
        // The current mouse position in pixel coordinates. (Read Only)
        public static Point mousePosition
        {
            get
            {
                return currentMouseState.Position;
            }
        }

        public static bool isMouse = false;
        public static bool isMousePress = false;
        public static bool isMouseRelease = false;

        public enum MouseButton { Left, Right, Middle };
        #region Keyboard & Mouse state properties
        private static KeyboardState currentKeyboardState;
        private static MouseState currentMouseState;

        private static KeyboardState previousKeyboardState;
        private static MouseState previousMouseState;

        #endregion

        #region Update
        /// <summary>
        /// Read the new input , Use the method on every first line of game-loop update.
        /// </summary>
        public static void UpdateNewInput()
        {
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            
            // Define fields
            isMouseRelease = GetMouseRelease(0);
            isMousePress = GetMousePress(0);
            isMouse = GetMouse(0);
        }

        /// <summary>
        /// Read the previous input , Use the method on every last line of game-loop update.
        /// </summary>
        public static void UpdatePreviousInput()
        {
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;
           
        }
        #endregion

        #region Keyboard's method

        /// <summary>
        /// Returns true while the user holds down the key identified by name. Think auto fire.
        /// </summary>
        public static bool GetKey(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns true during the frame  the use starts pressing down the key identified by name.
        /// </summary>
        public static bool GetKeyDown(Keys key)
        {
            if (previousKeyboardState.IsKeyUp(key) && currentKeyboardState.IsKeyDown(key))
                return true;

            return false;
        }

        /// <summary>
        /// Returns true during the frame the user releases the key identified by name.
        /// </summary>
        public static bool GetKeyUp(Keys key)
        {
            if (previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key))
                return true;
       
            return false;
        }

        #endregion

        #region Mouse's method
        /// <summary>
        /// Returns whether the given mouse button is held down.
        /// </summary>
        public static bool GetMouse(MouseButton button)
        {
           
            switch (button)
            {
                case MouseButton.Left:
                    if (currentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        return true;
                    }
                    break;

                case MouseButton.Right:
                    if (currentMouseState.RightButton == ButtonState.Pressed)
                    {
                        return true;
                    }
                    break;

                case MouseButton.Middle:
                    if (currentMouseState.MiddleButton == ButtonState.Pressed)
                    {
                        return true;
                    }
                    break;

                default:
                    Console.WriteLine("Use number 0,1,2 only");
                    break;

            }

            return false;

        }

        /// <summary>
        /// Returns true during the frame the user pressed the given mouse button.
        /// </summary>
        public static bool GetMousePress(MouseButton button)
        {

            switch (button)
            {
                case MouseButton.Left:
                    if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                        return true;
                    break;

                case MouseButton.Right:
                    if (currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released)
                        return true;
                    break;

                case MouseButton.Middle:
                    if (currentMouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Released)
                        return true;
                    break;

                default:
                    Console.WriteLine("Use number 0,1,2 only");
                    break;

            }

            return false;

        }

        /// <summary>
        /// Return true during the frame the user pressed the given mouse button.
        /// </summary>
        public static bool GetMouseRelease(MouseButton button)
        {

            switch (button)
            {
                case MouseButton.Left:
                    if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                        return true;
                    break;

                case MouseButton.Right:
                    if (currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed)
                        return true;
                    break;

                case MouseButton.Middle:
                    if (currentMouseState.MiddleButton == ButtonState.Released && previousMouseState.MiddleButton == ButtonState.Pressed)
                        return true;
                    break;

                default:
                    Console.WriteLine("Use number 0,1,2 only");
                    break;

            }

            return false;

        }

        #endregion

    }

}
