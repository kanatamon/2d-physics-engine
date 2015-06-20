using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Infinite_Runner.KanataEngine
{
    struct Constraint
    {
        public bool FreezePositionX;
        public bool FreezePositionY;
        public bool FreezePositionZ;
        public bool FreezeRotation;
    }   

    class Rigidbody2D : Component
    {
        #region The GameObject reference
        // the position attached to the rigidbody
        public Vector2 position 
        { 
            get { return gameObject.position; }

            set
            {
                gameObject.position = value;
            }
        }
        
        #endregion
        
        #region The properties of the rigidbody
        public Constraint constraint;// = default(Constraint);
        // The velocity of the rigidbody
        public Vector2 velocity = Vector2.Zero;

        // The direction along the gravity in vector perspective , used for physic2D only ,
        // The direction will be normalized every change of the value .

        private Vector2 _direction = new Vector2(0 , 1);
        public Vector2 direction
        {
            get { return _direction; }

            set 
            {
                value.Normalize();
                _direction = value;
                
                //gravity = direction * gravityScale * Physics2D.gravityForce;
            }

        }

        public Vector2 gravity { get; private set; }
        private float _gravityScale = 1.0f;
        public float gravityScale
        {
            get { return _gravityScale; }
            set
            {
                _gravityScale = value;

                gravity = gravityScale * Physics2D.gravity;
            }
        }

        #region Mass data
        private float _mass = 1.0f;
        public float mass  {
            get { return _mass; }
            set
            {
                _mass = value;
                invMass = value == 0 ? 0.0f : 1.0f / value;
            }

        }
        public float invMass { get; private set; }
        #endregion

        #region Inertia data
        private float _inertia = 10000f;
        public float inertia
        {
            get { return _inertia; }
            set
            {
                _inertia = value;
                invInertia = value == 0 ? 0.0f : 1.0f / value;
            }
        }
        public float invInertia { get; private set; }
        #endregion

        // Coeffecient of drag 
        public float drag = 1f;
        public float area = 1f;
    
        public float angularVelocity = 0f;
        public float torque = 0f;
        //public float orient;

        public float restitution = 0.2f;
        public float staticFriction = 0.5f;
        public float dynamicFriction = 0.3f;

        public Vector2 force;
        #endregion

        public void SetStatic() 
        {
            gravityScale = 0f;
            mass = 0.0f;
            inertia = 0.0f;
        }

        public void AddForce(Vector2 f)
        {
            force += f;
        }

        public void AddImpulse(Vector2 impulse, Vector2 contactVector)
        {
            velocity += invMass * impulse;
            angularVelocity += invInertia * Mathf.Cross(contactVector, impulse);
        }

    }
}
