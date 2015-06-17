using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infinite_Runner.KanataEngine;

namespace Infinite_Runner.InfiniteGame
{
    class BoxObj : GameObject
    {
        public PolygonCollider poly;
        public CircleCollider circle;

        public BoxObj(Scene theScene) : base(theScene)
        {
            //base.Initialize(theScene);

            name = "Box";
            //rotation = 10f;
            Rigidbody2D rigid = AddComponent<Rigidbody2D>();
            poly = AddComponent<PolygonCollider>();
            poly.SetOrient();
            //poly. = new Vector2(0, 200f);
            poly.SetBox(1000f, 20.0f);
            
            //circle = AddComponent<CircleCollider>();
            //circle.radius = 100f;
            //circle.center = new Vector2(-200f, 0f);

            //circle = AddComponent<CircleCollider>();
            //circle.radius = 200f;
            //circle.center = new Vector2(200f, 0f);

            rigid.SetStatic();
            rigid.restitution = 10f;

            position = new Vector2(0.0f, 600f);
            //rigidbody.angularVelocity = 0.1f;
            
        }

    }
}
