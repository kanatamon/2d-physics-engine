using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infinite_Runner.KanataEngine;

namespace Infinite_Runner.InfiniteGame
{
    class CircleObj : GameObject
    {
        public float radius = 1.0f;
        public CircleCollider circle;

        public CircleObj(Scene theScene) : base(theScene)
        {
            //base.Initialize(theScene);

            name = "Circle";
           
            position = new Vector2(0f, 0f);

            /*
            circle = AddComponent<CircleCollider>();
            circle.radius = 100f;
            circle.center = new Vector2(0f, 100f);
            //AddComponent<Message>();
            */
            Rigidbody2D rigid = AddComponent<Rigidbody2D>();
            rigid.mass = 100f;
            rigid.inertia = 100000f;
            rigid.gravityScale = 10f;
            rigid.angularVelocity = -1f;
            
            //rigidbody.velocity.X = 5f;
            //rotation = 10f;
           

            // Add more circle-collider
            CircleCollider c = AddComponent<CircleCollider>();
            c.radius = 100f;
            //c.center = new Vector2(0f, -150);
            /*
            c = AddComponent<CircleCollider>();
            c.radius = 100f;
            c.center = new Vector2(150f, 150f);
            */
        }

    }
}
