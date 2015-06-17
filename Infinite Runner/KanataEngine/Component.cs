using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Infinite_Runner.KanataEngine
{
    abstract class Component
    {
        // The gameObject that this compoent attached to.
        public GameObject gameObject { get; protected set; }

        public Scene scene { get { return gameObject.scene; } }

        public Vector2 position 
        { 
            get { return gameObject.position; }
            set { gameObject.position = value; }
        }

        public float rotation
        {
            get { return gameObject.rotation; }
            set { gameObject.rotation = value; }
        }

        public Rigidbody2D rigidbody { get { return gameObject.GetComponent<Rigidbody2D>(); } }
    
    }
}
