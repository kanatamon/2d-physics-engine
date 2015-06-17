using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infinite_Runner.KanataEngine
{
    class KanataObject
    {

        public static void Destroy(KanataObject obj) 
        {
            if(obj.GetType().IsSubclassOf(typeof(Component)))
            {
                // rigidbody delete from gameObj
                // collider delete from gameObj
                // layer delete from gameObj
                // any class else delete from gameObj.components
            }
            else if (obj.GetType().IsSubclassOf(typeof(GameObject)))
            {
                // delete from scene
            }

        }
    }
}
