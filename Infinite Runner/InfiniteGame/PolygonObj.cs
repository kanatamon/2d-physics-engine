using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infinite_Runner.KanataEngine;

namespace Infinite_Runner.InfiniteGame
{
    class PolygonObj : GameObject
    {
        public PolygonCollider poly;

        public PolygonObj(Scene theScene) : base(theScene)
        {
            //base.Initialize(theScene);

            name = "Polygon";

            Rigidbody2D rigid = AddComponent<Rigidbody2D>();
            poly = AddComponent<PolygonCollider>();
            Message m = AddComponent<Message>();
            m.poly = poly;

            rigid.restitution = 0.5f;
            //rigidbody.dynamicFriction = 0.2f;
            rigid.dynamicFriction = 0f;
            rigid.staticFriction = 0.4f;

            //poly.SetBox(100f, 100f);
            Vector2[] vertise = { 
                                   new Vector2(100f,100f),
                                   new Vector2(-100f,100f),
                                   new Vector2(-100f,-100f),
                                   new Vector2(100f, -100f)
                                };
            
            //Console.WriteLine(vertise == null);
            poly.SetVertices(vertise);
            
            rotation = 44.9f;
            poly.SetOrient();
            poly.center = new Vector2(20f, 100f);

            position = new Vector2(0f, -480f);
            //position.X -= 0f;
            //position.Y -= 0f;

            rigid.mass = 10f;
            rigid.inertia = 100000f;
            rigid.gravityScale = 10f;
            rigid.angularVelocity = -0.5f;
        }

    }
}
