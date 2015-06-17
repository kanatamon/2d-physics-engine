using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;

namespace Infinite_Runner.KanataEngine
{
    class SceneManager : DrawableGameComponent
    {
        #region Fields

        List<Scene> scenes = new List<Scene>();
        Scene currentScene;

        IGraphicsDeviceService graphicsDeviceService;

        #endregion


        #region Properties

        /// <summary>
        /// Expose access to our Game instance (this is protected in the
        /// default GameComponent, but we want to make it public).
        /// </summary>
        new public Game Game
        {
            get { return base.Game; }
        }


        /// <summary>
        /// Expose access to our graphics device (this is protected in the
        /// default DrawableGameComponent, but we want to make it public).
        /// </summary>
        new public GraphicsDevice GraphicsDevice
        {
            get { return base.GraphicsDevice; }
        }

        
        /// <summary>
        /// A content manager used to load data that is shared between multiple
        /// scenes. This is never unloaded, so if a scene requires a large amount
        /// of temporary data, it should create a local content manager instead.
        /// </summary>
        public ContentManager content { get; private set; }


        /// <summary>
        /// A default SpriteBatch shared by all the scenes. This saves
        /// each scene having to bother creating their own local instance.
        /// </summary>
        public SpriteBatch spriteBatch { get; private set; }


        /// <summary>
        /// A default font shared by all the scenes. This saves
        /// each scene having to bother loading their own local copy.
        /// </summary>
        public SpriteFont font { get; private set; }

        #endregion


        #region Initialization

        /// <summary>
        /// Constructs a new scene manager component.
        /// </summary>
        public SceneManager(Game game) : base(game)
        {
            content = new ContentManager(game.Services, "Content");

            graphicsDeviceService = (IGraphicsDeviceService)game.Services.GetService(typeof(IGraphicsDeviceService));

            if (graphicsDeviceService == null)
                throw new InvalidOperationException("No graphics device service.");

        }

        public override void Initialize()
        {
            // initialize all game's component 
            
            // init physics content
            Physics2D.LoadContent(GraphicsDevice, content);
            
            // instance the Game's SpriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // attach the main spriteBatch to the GUI
            GUI.spriteBatch = spriteBatch;
            
            // attach the main GraphicsDevice to the Animation
            Animation.graphicsDevice = GraphicsDevice;
            
            // Create the main-camera , used for very-first camera 
            Camera.main = new Camera(GraphicsDevice);

            base.Initialize();

        }

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            // inite the scene
            currentScene.Initialize();
            // set physic using to apply in the scene 
            Physics2D.scene = currentScene;

        }


        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Unload content belonging to the scene manager.
            content.Unload();

            // Tell each of the scenes to unload their content.
            foreach (Scene scene in scenes)
            {
                scene.UnloadContent();
            }
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Allows each scene to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // read new keyboard and gamepad
            Input.UpdateNewInput();

            // update the current scene
            currentScene.Update(gameTime);
            
            // update the Physics2D
            Physics2D.Update(gameTime);

            // read previous keyboard and gamepad
            Input.UpdatePreviousInput();
        }


        /// <summary>
        /// Tells each scene to draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Render all graphics
            currentScene.Render(gameTime);

            //if(isDevlopeMode)
            //Physics2D.DrawColliders(spriteBatch);
            Physics2D.DrawColliders2(spriteBatch);

        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Adds a new scene to the scene manager.
        /// </summary>
        public void AddScene(Scene scene)
        {
            scene.sceneManager = this;

            scenes.Add(scene);

            // Check if the 'currentScene' never has any scene , then use the scene .
            if (currentScene == null)
            {
                currentScene = scene;
            }

        }


        /// <summary>
        /// Removes a scene from the scene manager. You should normally
        /// use GameScene.ExitScene instead of calling this directly, so
        /// the scene can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        public void RemoveScene(Scene scene)
        {
            // If we have a graphics device, tell the scene to unload content.
            if ((graphicsDeviceService != null) &&
                (graphicsDeviceService.GraphicsDevice != null))
            {
                scene.UnloadContent();
            }

            scenes.Remove(scene);

        }


        /// <summary>
        /// Expose an array holding all the scenes. We return a copy rather
        /// than the real master list, because scenes should only ever be added
        /// or removed using the AddScene and RemoveScene methods.
        /// </summary>
        public Scene[] GetScenes()
        {
            return scenes.ToArray();
        }

        #endregion

        #region LoadScene functions

        // Load scene from number .
        public void LoadScene(int index)
        {
            // Check if the index be in the available range .
            if (index < 0 || index >= scenes.Count) return;

            // Change to the current scene 
            currentScene = scenes[index];
            LoadContent();

        }

        #endregion
    }
}
