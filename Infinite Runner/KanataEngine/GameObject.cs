using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Infinite_Runner.KanataEngine
{
    class GameObject
    {
        // The scene that this attached to
        public Scene scene { get; private set; }

        // The name of the gameObject
        public string name = "GameObject";

        public Vector2 scale = new Vector2(1, 1);
        public Vector2 position = new Vector2(0, 0);
        public float rotation = 0f;

        // The collection of component that attached to the gameObject.
        private List<Component> components = new List<Component>();

        // The collection of update object that attached to this gameObject.
        private List<Behavior> behaviors = new List<Behavior>();
        
        // The collection of collider attached to the gameObject.
        private List<Collider> _colliders = new List<Collider>();
        public List<Collider> colliders 
        {
            get { return _colliders; }
            private set { _colliders = value; } 
        }

        /*
        // The rigidbody attached to the gameObject.
        public Rigidbody2D rigidbody { get; private set; }

        // The layer data , used for the drawing order and collision ignorance.  
        public Layer layer { get; private set; }

        // The Renderer attached to this GameObject.
        public Renderer2D renderer2D { get; private set; }

        public Animator animator { get; private set; }
        */

        public GameObject(Scene theScene) 
        {
            scene = theScene;
        }

        /*
        /// <summary>
        /// Initialize the gameObject.
        /// </summary>
        public virtual void Initialize(Scene theScene) 
        {
            scene = theScene;
        }*/

        #region Initialization in the game world
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        public virtual void Awake() { }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
        /// </summary>
        public virtual void Start() 
        {
            foreach (Behavior behavior in behaviors) behavior.Start();
        }

        #endregion

        #region Update's Method
        /// <summary>
        ///  Update the gameObject , this called one per frame .
        /// </summary>
        public void Update(GameTime gameTime)
        { 
            foreach(Behavior behavior in behaviors)
            {
                behavior.Update(gameTime);

            }

            // Update the animator.
            Animator animator = GetComponent<Animator>();
            if (animator != null) animator.Animate(gameTime);

        }

        /// <summary>
        /// วาด GUI บนหน้าจอของกล้องหลัก
        /// </summary>
        public void OnGUI()
        {
            foreach (Behavior behavior in behaviors)
            {
                behavior.OnGUI();
            }
        }

        /// <summary>
        /// Sent when an incoming collider makes contact with this object's collider (2D physics only).
        /// </summary>
        public void OnCollision(GameObject other) 
        {
            foreach (Behavior behavior in behaviors)
            {
                behavior.OnCollision(other);
            }
        }

        /// <summary>
        /// Sent when another object enters a trigger collider attached to this object (2D physics only).
        /// </summary>
        public void OnTrigger(GameObject other) 
        {
            foreach (Behavior behavior in behaviors)
            {
                behavior.OnTrigger(other);
            }
        }
      

        #endregion

        #region Conponent's Method
        /// <summary>
        /// Returns the component of Type type if the game object has one attached, null if it doesn't.
        /// </summary>
        public T GetComponent<T>() where T : Component 
        {
            // Create the component container  
            //Component theComponent = null;
            T theComponent = (T)components.Find(component => component.GetType() == typeof(T));
            /*
            // Find the component that attached to this gameobject 
            foreach(Component component in components)
            {
                // Check the comming component
                if (component.GetType() == typeof(T))
                {
                   theComponent = component;
                   break;
                }

            }*/

            //return (T)theComponent;
            return theComponent;
        
        } 

        /// <summary>
        /// Returns all components of Type type in the GameObject.
        /// </summary>
        public IEnumerable<T> GetComponents<T>() where T : Component
        {
            List<T> list = new List<T>();

            // Find all component type T in the GameObject
            foreach (Component component in components)
            {
                // Check if coming component is sub-class of T
                if (component.GetType().IsSubclassOf(typeof(T)))
                    list.Add((T)component);
            }

            return list;
        }

        /// <summary>
        /// Add the instance of component , attached to this gameObjec t with specific type.
        /// </summary>
        public T AddComponent<T>() where T : Component, new()
        {
            // Instance new object type T 
            Component component = new T();
            Type typeT = typeof(T);

            // Attach this gameObject to the component
            PropertyInfo gameObjProp = typeT.GetProperty("gameObject");
            gameObjProp.SetValue(component, this, null);

            // Attach the component to this gameObject 
            components.Add(component);

            // 
            if (typeT.IsSubclassOf(typeof(Behavior)))
            {
                // Subclass of Behavior , has update method
                behaviors.Add((Behavior)component);
            }
            else
            {
                if(typeT.IsSubclassOf(typeof(Collider)))
                {
                    // Check if the new collider is the first one 
                    // Init the colliders
                    if (colliders == null) colliders = new List<Collider>();
                    
                    // Add the new collider to the collection
                    if(typeT == typeof(PolygonCollider))
                        colliders.Add((PolygonCollider)component);
                    else
                        colliders.Add((CircleCollider)component);
                    
                }
    
            }
            
            return (T)component;
        }

        #endregion

        #region Static Function
        /// <summary>
        /// Get gameObject with specific name in the current scene .
        /// </summary>
        public static GameObject Find(string name)
        {
            GameObject findingGameObject = null;

            // Find gameObject in the current scene .
            foreach(GameObject gameObject in Physics2D.scene.GameObjects)
            {
                // Check if gameObject is the specific gameObject , using name 
                if (gameObject.name.Equals(name))
                {
                    // Alias to the returned instance then exit the loop 
                    findingGameObject = gameObject;
                    break;
                } 
                
            }

            return findingGameObject;
        }

        public static void Destroy(GameObject gameObject) 
        {
          
        }

        #endregion
    }
}
