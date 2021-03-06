﻿using UnityEngine;

namespace ECS.Components
{
    public class InputData : MonoBehaviour
    {
        public Vector2 DownPos;
        public Vector2 UpPos;
        public Vector2 SwipeDirection;
        public bool IsSwipe;
        public InputState State;
        public bool BackDown;
    }

    public enum InputState
    {
        None,
        Down,
        Moved,
        Up
    }
}
