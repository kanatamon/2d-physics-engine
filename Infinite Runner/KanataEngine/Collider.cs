using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Infinite_Runner.KanataEngine
{
    class Collider : Component
    {
        #region Collider's properties, used for Physics

        public bool isTrigger = false; 
        /*
        // The size of collider ,
        // When the collider was created the size will be sized as the gamObject.size .
        // When the size change , this will recalculate the halfSize .
        private Vector2 _size;
        public Vector2 size
        {
            get { return _size; }
            
            set
            {
                _size = value;
                // Recalculate the 'halfSize'
                halfSize = size / 2;
            }
        }

        // The half size of the size , 
        // Used for optimization of collision detection on Physics2D .
        // Adjustment of the halfSize is avialable only on the 'size' .
        public Vector2 halfSize { get; private set; }
        */

        // The center of the collider start the zero point of the GameObject's position .
        protected Vector2 _center = Vector2.Zero;
        public virtual Vector2 center
        {
            get { return _center; }
            set { _center = value; }
        }

        // Get the position in the real world.
        public Vector2 positionOnWorld { get { return Vector2.Transform(center, affMat) + position; } }

        // The affine matrix , including rotation using radians
        public Matrix affMat = new Matrix();

        #endregion

        public virtual void SetOrient() 
        {
            // transformation vertices , rotate -> translate
            affMat = Matrix.CreateRotationZ(MathHelper.ToRadians(gameObject.rotation));
        } 


    }

}
