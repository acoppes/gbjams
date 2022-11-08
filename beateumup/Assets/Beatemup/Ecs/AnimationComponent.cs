using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Beatemup.Ecs
{
    [Serializable]
    public class AnimationFrame
    {
        // public int frame;
        public Sprite sprite;
        public Sprite fxSprite;
    }
    
    [Serializable]
    public class AnimationDefinition
    {
        public string name;
        public List<AnimationFrame> frames = new ();
        public int TotalFrames => frames.Count;
        
        // public float Duration => TotalFrames / fps;

        public float GetDuration(float fps)
        {
            Assert.IsTrue(fps > 0);
            return TotalFrames / fps;
        }
    }

    public struct AnimationComponent : IEntityComponent
    {
        public const float DefaultFrameRate = 15.0f;
        
        public enum State
        {
            Completed,
            Playing
        }
        
        public AnimationsAsset animationsAsset;

        public float fps;
        
        public int currentAnimation;
        public int currentFrame;
        public float currentTime;
        public int loops;
        public State state;
        public bool paused;

        public float playingTime;
        
        public void Play(int animation, int startFrame, int loops = -1)
        {
            currentAnimation = animation;
            currentFrame = startFrame;
            currentTime = 0;
            playingTime = 0;
            this.loops = loops;
            state = State.Playing;
        }
        
        public void Play(int animation, int loops = -1)
        {
            Play(animation, 0, loops);
        }

        public void Play(string animation, int loops = -1)
        {
            Play(animationsAsset.FindByName(animation), loops);
        }

        public bool IsPlaying(string animationName)
        {
            return currentAnimation == animationsAsset.FindByName(animationName);
        }

        public bool HasAnimation(string animationName)
        {
            return animationsAsset.FindByName(animationName) != -1;
        }
    }
}