using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infinite_Runner.KanataEngine;
using Infinite_Runner.KanataEditor;

namespace Infinite_Runner.InfiniteGame
{
    class MakeCollider : Behavior
    {
        public PolygonObj polyObj;
        public CircleObj circleObj;

        public override void Update(GameTime gameTime)
        {
            //Console.WriteLine("Mouse-position : "+Input.mousePosition.ToString());
            // if right-click , instance polygon 
            if (Input.GetMousePress(Input.MouseButton.Right))
            {
                // Get mouse position in world space 
                Vector2 X = Camera.main.ScreenToWorld(Input.mousePosition);

                // Instantiate 
                PolygonObj newPoly = (PolygonObj)scene.Instantiate(new PolygonObj(gameObject.scene));
  
                newPoly.position = X;
                newPoly.poly.SetBox(100f, 100f);
                newPoly.rotation = 44.9f;
                newPoly.poly.SetOrient();
            
            }
            // if left-click , instance circle
            if (Input.GetMousePress(Input.MouseButton.Left))
            {
                //Console.WriteLine("Left click");
                //Vector2 pos = new Vector2(Input.mousePosition.X, Input.mousePosition.Y);
                //CircleObj newCir = GameObject.Instance(polyObj, pos);// create new obj -> add to the world
                //newCir.radius
            }
        }
    }

}
