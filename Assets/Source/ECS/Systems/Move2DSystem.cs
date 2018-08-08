using ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Systems
{
    public class Move2DSystem : ComponentSystem
    {
        public struct MovableGroup
        {
            public Position2D Position;
            public Move2D Move2D;
            public ElementState State;
        }

        public struct SettingsGroup
        {
            public Settings Settings;
        }
        
        protected override void OnUpdate()
        {
            Settings settings = null;
            foreach (var entity in GetEntities<SettingsGroup>())
            {
                settings = entity.Settings;
            }

            if (settings == null)
                return;
            
            foreach (var entity in GetEntities<MovableGroup>())
            {
                if (entity.Move2D.ShouldMove == false)
                    continue;

                float speed = entity.Move2D.isFalling ? settings.FallSpeed : settings.MoveSpeed;
                Position2D pos = entity.Position;
                pos.Value = Vector2.Lerp(pos.Value.ToVector2(), entity.Move2D.TargetValue,
                    Time.deltaTime * speed);
                if (pos.Value.ToVector2() == entity.Move2D.TargetValue)
                {
                    entity.Move2D.ShouldMove = false;
                    entity.Move2D.isFalling = false;
                    pos.Value = entity.Move2D.TargetValue;
                    entity.State.IsMoved = true;
                }
            }
        }
    }

    public static class float2Extensions
    {
        public static Vector2 ToVector2(this float2 value)
        {
            return new Vector2(value.x, value.y);
        }
    }
}
