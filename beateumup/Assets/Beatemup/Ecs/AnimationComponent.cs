using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

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
        public List<AnimationFrame> frames = new List<AnimationFrame>();
        public float fps = 30.0f;
        public int TotalFrames => frames.Count;
        
        // public float Duration => TotalFrames / fps;
    }

    public struct AnimationComponent : IEntityComponent
    {
        public enum State
        {
            Completed,
            Playing
        }
        
        public AnimationsAsset animationsAsset;
        
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
    }
}