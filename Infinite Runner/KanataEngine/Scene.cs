using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Infinite_Runner.InfiniteGame;

namespace Infinite_Runner.KanataEngine
{
    abstract class Scene
    {
        public bool isActive = true;
        
        // The collection of gameobject in the scene .
        private List<GameObject> _gameObjects = new List<GameObject>();
        public IList<GameObject> GameObjects { get { return _gameObjects.AsReadOnly(); } }

        // The manager that this screen belongs to.
        public SceneManager sceneManager;

        // The gameObjects those will be added in next game-loop.
        private List<GameObject> _addList = new List<GameObject>();
        // The gameObjects those will be romoved in next game-loop.
        private List<GameObject> _destroyList = new List<GameObject>();

        //List<Animator> animators = new List<Animator>();

        public virtual void Initialize()
        {
            LoadGameObject();

            // Flag : Now your script can use GameObject.Find() etc...

            // start runing this scene for very first time before the first Update() 
            foreach (GameObject gameObject in GameObjects) gameObject.Start();

        }

        /// <summary>
        /// Load every gameObject , instance gameObject then add them to World here.
        /// </summary>
        public virtual void LoadGameObject() { }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public virtual void UnloadContent() { }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        public virtual void Update(GameTime gameTime) 
        {
            // clear 
            //animators.Clear();

            // Update every gameObject in the world
            foreach (GameObject gameObject in GameObjects)
            {
                // Update a Game Object.
                gameObject.Update(gameTime);

                /*
                if (gameObject.animator != null)
                    animators.Add(gameObject.animator);
                */
            }

            /*
            // update animation
            foreach (Animator animator in animator)
            {
                animator.Animate(gameTime);
            }
            */

            // Destroying gameObject 
            foreach(GameObject destroyee in _destroyList) _gameObjects.Remove(destroyee);
        
            // Adding new gameObject to the scene
            foreach (GameObject newGameObj in _addList) _gameObjects.Add(newGameObj);

            // Clear executed temporary gameObject-list 
            _destroyList.Clear();
            _addList.Clear();
        }

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public virtual void Render(GameTime gameTime)
        {
            foreach (GameObject gameObj in _gameObjects)
            {
                // Draw renderer 
                Renderer2D renderer = gameObj.GetComponent<Renderer2D>();
                if (renderer != null) renderer.Draw(sceneManager.spriteBatch);

                // Draw GUI
                gameObj.OnGUI();

            }
        }

        /// <summary>
        /// Add gameObject to the world, use in LoadGameObject phase only.
        /// </summary>
        protected GameObject AddGameObjectOnLoad(GameObject gameObject) 
        {
            _gameObjects.Add(gameObject);
            return gameObject;
        }

        /// <summary>
        /// Instantiate the gameObject to the scene(world).
        /// </summary>
        public GameObject Instantiate(GameObject gameObject)
        {
            //gameObject.Initialize(this);
            gameObject.Start();

            // Add the new gameObject to the scene(wolrd)
            _addList.Add(gameObject);
            
            return gameObject;
        }

        /// <summary>
        /// Remove the gameObject.
        /// </summary>
        public void Destroy(GameObject gameObject)
        {
            _destroyList.Add(gameObject);   
        }
      
    }
}
