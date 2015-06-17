using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Infinite_Runner.KanataEngine
{
    // The LayerMark of the game 
    // To-Use : Edit layerMark must do here on the enum  
    enum LayerMark
    {
        Default,
        Water,
        Player,
        Enemy,
        Shootable,
        Ground,
        UI
    }

    class Layer : Component
    {
        public static int totalLayerMark = Enum.GetNames(typeof(LayerMark)).Length;

        // The layer where this gameObject existing .
        public LayerMark existingLayerMark = LayerMark.Default;

        // The data .
        bool[] ignorances;

        /**** Clss-Constructor ****/
        public Layer(GameObject gameObj)
        {
            // Attach the layer to the gameObject .
            gameObject = gameObj;
            
            // Allocate array 
            ignorances = new bool[totalLayerMark];
            
            // Init data 
            for (int i = 0; i < totalLayerMark; i++)
            {
                ignorances[i] = false;
            }

        }

        /**** Public Function ****/

        /// <summary>
        /// Set data on the specific layerMark .
        /// </summary>
        public void SetIgnore(LayerMark layerMark, bool isIgnore = true)
        {
            // Convert the layerMark to index .
            int index = (int)layerMark;

            // Set the boolean of the layer then exit .
            ignorances[index] = isIgnore;
 
        }

        /// <summary>
        /// Get data on the specific layerMark .
        /// </summary>
        public bool IsIgnore(LayerMark layerMark)
        {
            // Convert the layerMark to index .
            int index = (int)layerMark;

            return ignorances[index];
        }

        /// <summary>
        /// Display the datas .
        /// </summary>
        public static void Display(bool[] data)
        {
            for(int i=0 ;i<totalLayerMark ;i++)
            {
                LayerMark l = (LayerMark)i;
                Console.WriteLine("On the layer [" + l.ToString() + "] ignorace is [" + data[i].ToString() + "]");
            }
            
        }
  
    }
}
