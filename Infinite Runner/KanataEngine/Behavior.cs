using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Infinite_Runner.KanataEngine
{
    abstract class Behavior : Component
    {
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// Update method will be called one per frame.
        /// </summary>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Sent when an incoming collider makes contact with this object's collider (2D physics only).
        /// </summary>
        public virtual void OnCollision(GameObject other) { }

        /// <summary>
        /// Sent when another object enters a trigger collider attached to this object (2D physics only).
        /// </summary>
        public virtual void OnTrigger(GameObject other) { }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        public virtual void OnGUI() { }

    }
}
