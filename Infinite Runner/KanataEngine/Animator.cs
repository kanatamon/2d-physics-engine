using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Infinite_Runner.KanataEditor;

namespace Infinite_Runner.KanataEngine
{
    class Animator : Component
    {
        private List<Animation> animationList = new List<Animation>();
        public Animation[] animations
        {
            get { return animationList.ToArray(); }
        }

        Animation animation;

        public void AddAnimation(Animation anim)
        {
            animationList.Add(anim);
        }

        public void SetAnimation(string animationName)
        {
            // play animation named 'animationName'
            foreach (Animation anim in animationList)
            {
                if (anim.name.Equals(animationName))
                {
                    // reset accumulator , currentFrame in the animation
                    anim.Reset();
                    // set current animation 
                    animation = anim;
                    return;
                }   
            }

        }

        /// <summary>
        /// Update the current animation and set srpite of the GameObject.
        /// </summary>
        public void Animate(GameTime gameTime)
        {
            animation.Update(gameTime);
            
            Renderer2D renderer = gameObject.GetComponent<Renderer2D>();
            if(renderer != null) renderer.sprite = animation.sprite;
        }


    }
}
