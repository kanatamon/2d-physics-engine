using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Infinite_Runner.KanataEngine;
using Infinite_Runner.KanataEditor;

namespace Infinite_Runner.InfiniteGame
{
    class Demo : Scene
    {
        GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;
        ContentManager content;

        Skin mySkin;
        Texture2D testTex;

        float time = 0f;

        public override void Initialize()
        {
            // Alias the game's field
            spriteBatch = sceneManager.spriteBatch;
            content = sceneManager.content;
            graphicsDevice = sceneManager.GraphicsDevice; 

            base.Initialize();

        }

        /// <summary>
        /// Load every gameObject used in the Scene
        /// all of your content
        /// </summary>
        public override void LoadGameObject()
        {
            Camera.main.zoom = 0.3f;
            //Camera.main.position = new Vector2(200f, 0f);
            //Camera.main.rotatixc   on = 10f;

            BoxObj boxObj = new BoxObj(this);
            //boxObj.Initialize(this);
            CircleObj cirObj = new CircleObj(this);
            //cirObj.Initialize(this);
            PolygonObj polyObj = new PolygonObj(this);
            //polyObj.Initialize(this);

            AddGameObjectOnLoad(cirObj);
            //GameObjects.Add(polyObj);
            AddGameObjectOnLoad(boxObj);
            
            /*
            basicContoller = new BasicContorller();
            box = new Box();

            // init the scene's GameObject
            GameObjects.Add(box);
            GameObjects.Add(basicContoller);
            
            
            // init gameObject 
            basicContoller.Initialize(this, content);
            box.Initialize(this, content);

            // Load original texture 
            Texture2D origin = content.Load<Texture2D>("flatastic2D");

            Rectangle source;
            Texture2D normalTex;
            Texture2D hoverTex;

            Texture2D ori = content.Load<Texture2D>("flatastic");
            testTex = ori;
       
            SpriteFont font = content.Load<SpriteFont>("Score");

            source = new Rectangle(21, 74, 75, 37);
            normalTex = KanataTool.CropTexture2D(graphicsDevice, origin, source);
            source = new Rectangle(21, 168, 75, 37);
            hoverTex = KanataTool.CropTexture2D(graphicsDevice, origin, source);

            mySkin = new Skin();
            GUI.skin = mySkin;
            mySkin.AddStyle(new GUIStyle("button", normalTex, hoverTex, normalTex, font));
            
            // Add the_light 
            Texture2D light = content.Load<Texture2D>("the_light");
            mySkin.AddStyle(new GUIStyle("light", light, light, light, font));
            */
        }

        /*
        public override void Update(GameTime gameTime)
        {
            Console.Clear();
            
            base.Update(gameTime);
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Console.WriteLine("eslaped time : " + time.ToString());
        }
        */
    }
}
