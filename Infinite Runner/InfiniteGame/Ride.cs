using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using Infinite_Runner.KanataEngine;
using System.Threading.Tasks;   //using Tasks.Delay()

namespace Infinite_Runner.InfiniteGame
{
    class Ride : Behavior
    {
        public string myName = "Ride";
        public Texture2D texture;

        public BoxCollider boxCollider;

        public SoundEffect effect;
        SoundEffectInstance soundEffectInstance;

        Vector2 position;
        Rigidbody2D rigidbody;
        Animator animator;

        const int WIZARD_SPEED = 400;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;

        //Sound effect
        float volume = 1.0f;
        float pitch = 0.0f;
        float pan = 0.0f;

        // Enum of the State
        enum State
        {
            idle,
            Walking,
            Jumping
        }

        State currentState = State.Walking;

        public override void Start()
        {
            // alias
            animator = gameObject.GetComponent<Animator>();
            rigidbody = gameObject.GetComponent<Rigidbody2D>();

            // create soundEffect controller
            soundEffectInstance = effect.CreateInstance();
            soundEffectInstance.IsLooped = true;
            soundEffectInstance.Volume = 0.5f;
            soundEffectInstance.Pan = -0.5f;
            soundEffectInstance.Pitch = 0.5f;

            if (typeof(PolygonCollider).IsSubclassOf(typeof(Collider)))
                Console.WriteLine("Polygon is collider ");
            if (typeof(PolygonCollider) == typeof(Component))
                Console.WriteLine("Polygon is component ");

        }

        public override void Update(GameTime gameTime)
        {
            Camera.main.position = gameObject.position;

            Move(gameTime);
            
            // jump
            if (Input.GetKeyDown(Keys.Space))
            {
                //effect.Play(volume, pitch, pan);
                gameObject.GetComponent<Rigidbody2D>().velocity.Y = -400f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            
            if (Input.GetKey(Keys.R))
            {
                //WaitToRun(0f);
                gameObject.rotation += 1;
            }

            if (Input.GetKey(Keys.T))
            {
                //WaitToRun(0f);
                boxCollider.center += new Vector2(5f, 5f);
                Console.WriteLine(gameObject.position.ToString());
            }

            if (Input.GetKeyDown(Keys.P))
                soundEffectInstance.Play();
            if (Input.GetKeyDown(Keys.O))
                soundEffectInstance.Stop();

            //if(rigidbody.velocity.Y > 10)
            //Console.WriteLine(gameObject.position.Y);

        }

        public override void OnTrigger(GameObject other)
        {
            Console.WriteLine("Ride was trigger");
        }

        public override void OnGUI()
        {
            Rectangle position = new Rectangle(100, 0, 75*2, 37*2);
            Rectangle position2 = new Rectangle(0, 0, 75, 37);
            //GUI.color = new Color(GUI.color.R, GUI.color.G, GUI.color.B, 0.5f);
            
            /*
            // GUI.Box(r, Physics2D.colliderTexture);
            if (GUI.Button(r, Physics2D.colliderTexture))
                Console.WriteLine("Contain");
            */
            //GUI.Label(source, texture);
            //GUI.Label(source, GUI.skin.GetStlye("button"));
            GUI.Box(position, GUI.skin.GetStlye("button"));
            if (GUI.Button(position2, texture))
            {
                Console.WriteLine("True");
            }
            

        }

        // Move the position of this object 
        void Move(GameTime gameTime)
        {
            Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
            if (currentState == State.Walking)
            {
                rigidbody.velocity.X = 0f;
                rigidbody.velocity.Y = 0f;

                if (Input.GetKey(Keys.Left))
                {
                    rigidbody.velocity.X = WIZARD_SPEED * MOVE_LEFT;
                    
                }
                else if (Input.GetKey(Keys.Right))
                {
                    rigidbody.velocity.X = WIZARD_SPEED * MOVE_RIGHT;
                    
                }

                if (Input.GetKey(Keys.Down))
                {
                    rigidbody.velocity.Y = WIZARD_SPEED * MOVE_DOWN;

                }
                else if (Input.GetKey(Keys.Up))
                {
                    rigidbody.velocity.Y = WIZARD_SPEED * MOVE_UP;

                }

                // apply velocity
                gameObject.position += rigidbody.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                // Flip
                if (rigidbody.velocity.X < 0 )
                {
                    gameObject.GetComponent<Renderer2D>().spriteEffects = SpriteEffects.FlipHorizontally;
                }
                else if (rigidbody.velocity.X > 0)
                {
                    gameObject.GetComponent<Renderer2D>().spriteEffects = SpriteEffects.None;
                }
                

            }
        }

        public async void WaitToRun(float t)
        {
            Console.WriteLine("1'st");
            await Task.Delay(5000);
            Console.WriteLine("2'nd");
            //http://social.technet.microsoft.com/wiki/contents/articles/21177.visual-c-thread-sleep-vs-task-delay.aspx    
            await Task.Delay(2000);
            Console.WriteLine("3'rd");
        }

    }
}
