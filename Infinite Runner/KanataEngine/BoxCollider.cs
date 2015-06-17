using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Infinite_Runner.KanataEngine
{
    class BoxCollider : Collider
    {
        const int totalBoxDot = 4;
       
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

                // assign new dots 
                baseDots[0].X = -halfSize.X + center.X;
                baseDots[0].Y = -halfSize.Y + center.Y;

                baseDots[1].X = halfSize.X + center.X;
                baseDots[1].Y = -halfSize.Y + center.Y;

                baseDots[2].X = halfSize.X + center.X;
                baseDots[2].Y = halfSize.Y + center.Y;

                baseDots[3].X = -halfSize.X + center.X;
                baseDots[3].Y = halfSize.Y + center.Y;
               
            }
        }

        public override Vector2 center
        {
            get { return _center; }
            set 
            {
                _center = value;

                // assign new dots 
                baseDots[0].X = -halfSize.X + center.X;
                baseDots[0].Y = -halfSize.Y + center.Y;

                baseDots[1].X = halfSize.X + center.X;
                baseDots[1].Y = -halfSize.Y + center.Y;

                baseDots[2].X = halfSize.X + center.X;
                baseDots[2].Y = halfSize.Y + center.Y;

                baseDots[3].X = -halfSize.X + center.X;
                baseDots[3].Y = halfSize.Y + center.Y;
            }
        } 

        // The half size of the size , 
        // Used for optimization of collision detection on Physics2D .
        // Adjustment of the halfSize is avialable only on the 'size' .
        public Vector2 halfSize { get; private set; }

        readonly Vector2[] baseDots = new Vector2[totalBoxDot];
        public Vector2[] boxDots;

       
        /// <summary>
        /// Get left-normal vectors of the box collider.
        /// </summary>
        public Vector2[] GetNormal()
        {
            boxDots = new Vector2[totalBoxDot];
            
            // transform dots
            /*
            for (int i = 0; i < totalBoxDot; i++)
            {
                boxDots[i] = Vector2.Transform(
                    baseDots[i], 
                    Matrix.CreateRotationZ(MathHelper.ToRadians(gameObject.rotation))
                    );
                boxDots[i] += gameObject.position;
            }*/

            // affine matrix = Rotate -> Translate
            Matrix affMat = Matrix.CreateRotationZ(MathHelper.ToRadians(gameObject.rotation))
                * Matrix.CreateTranslation(gameObject.position.X, gameObject.position.Y, 1);
            Vector2.Transform(baseDots, ref affMat, boxDots);

            // normalize left-side each dot's vector
            // normalized-left's vector = (-y , x)
            Vector2[] normals = new Vector2[totalBoxDot];

            for (int i = 0; i < totalBoxDot; i++)
            {
                normals[i] = new Vector2(
                        boxDots[(i + 1) % totalBoxDot].X - boxDots[i].X,
                        boxDots[(i + 1) % totalBoxDot].Y - boxDots[i].Y
                    );
                
                // left normalize
                normals[i] = new Vector2(
                        -1 * normals[i].Y,
                        normals[i].X
                    );
                //normals[i].Normalize();   // direction of normal be used , nomalize or not is ok   
            }
            return normals;

        }
 

    }
}
