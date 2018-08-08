using System;
using ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
    [UpdateAfter(typeof(InputProcessorSystem))]
    public class ElementMoveSystem : ComponentSystem
    {
        public struct InputGroup
        {
            public InputData InputData;
        }

        public struct SettingsGroup
        {
            public Settings Settings;
        }

        public struct AnimalGroup
        {
            public Move2D Move2D;
            public GridCoord Coord;
            public Position2D Pos;
            public BoxCollider2D Collider2D;
        }

        public struct GameStateGroup
        {
            public GameState GameState;
        }
        
        protected override void OnUpdate()
        {
            GameState gameState = GetEntities<GameStateGroup>()[0].GameState;
            InputData inputData = null;
            foreach (var entity in GetEntities<InputGroup>())
            {
                inputData = entity.InputData;
                break;
            }

            if (inputData == null)
                return;

            Settings settings = null;
            foreach (var entity in GetEntities<SettingsGroup>())
            {
                settings = entity.Settings;
            }

            if (settings == null)
                return;

            if (inputData.State != InputState.Up)
                return;

            if (!inputData.IsSwipe)
                return;

            var hasFinishElement = false;
            Vector2 finalcoord;
            
            foreach (var entity in GetEntities<AnimalGroup>())
            {
                if (entity.Collider2D.OverlapPoint(inputData.DownPos) == false)
                    continue;

                var startElement = entity;
                finalcoord = new Vector2(startElement.Coord.Value.x + inputData.SwipeDirection.x,
                    startElement.Coord.Value.y + inputData.SwipeDirection.y);

                if (finalcoord.x >= 0 && finalcoord.x < settings.BoardSize.x)
                {
                    hasFinishElement = true;
                }
                
                if (finalcoord.y >= 0 && finalcoord.y < settings.BoardSize.y)
                {
                    hasFinishElement = true;
                }

                if (hasFinishElement == false)
                    break;
                
                foreach (var finalEntity in GetEntities<AnimalGroup>())
                {
                    if (!(Math.Abs(finalEntity.Coord.Value.x - finalcoord.x) < 0.001f) ||
                        !(Math.Abs(finalEntity.Coord.Value.y - finalcoord.y) < 0.001f)) continue;
                    
                    finalEntity.Coord.Value = startElement.Coord.Value;
                    startElement.Coord.Value = finalcoord;
                    startElement.Move2D.TargetValue = GetPosition(startElement.Coord.Value, settings.ElementSize,
                        settings.StartPoint);
                    finalEntity.Move2D.TargetValue = GetPosition(finalEntity.Coord.Value, settings.ElementSize,
                        settings.StartPoint);
                    startElement.Move2D.ShouldMove = true;
                    finalEntity.Move2D.ShouldMove = true;
                    break;
                }

                break;
            }
        }

        public static Vector2 GetPosition(Vector2 coord, Vector2 elementSize, Vector2 startPoint)
        {
            return new Vector2(coord.x * elementSize.x + startPoint.x, coord.y * elementSize.y * -1 + startPoint.y);
        }
    }
}