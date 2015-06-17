using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infinite_Runner.KanataEngine;

namespace Infinite_Runner.InfiniteGame
{
    class Message : Behavior
    {
        public PolygonCollider poly;

        public override void Update(GameTime gameTime)
        {
            if (Input.GetKey(Keys.T))
                gameObject.GetComponent<Rigidbody2D>().torque = -10000f;

            if (Input.GetKey(Keys.R))
                gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0f;

            if (Input.GetKey(Keys.S))
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.Zero;

            if (Input.GetKeyDown(Keys.D))
                scene.Destroy(gameObject);

            if (Input.GetMousePress(Input.MouseButton.Left))
            {
                //Console.WriteLine("Poly at "+poly.worldPosition.ToString());
                Console.WriteLine("Mouse at " + Camera.main.ScreenToWorld(Input.mousePosition));
                Console.WriteLine(Physics2D.OverlapPoint(poly, Camera.main.ScreenToWorld(Input.mousePosition), 100f));
                //float v = new Vector2(3f, -5f).Length();
                //Console.WriteLine(v);
            }

        }

    }
}
