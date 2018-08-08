using ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
    [UpdateAfter(typeof(RemoveElementSystem))]
    public class FallElementSystem : ComponentSystem
    {
        public struct AnimalGroup
        {
            public GridCoord Coord;
            public Position2D Pos;
            public Move2D Move2D;
            public ElementState State;
        }

        public struct GameStateGroup
        {
            public GameState State;
        }

        public struct SettingsGroup
        {
            public Settings Settings;
        }
        
        protected override void OnUpdate()
        {
            var animals = GetEntities<AnimalGroup>();
            var state = GetEntities<GameStateGroup>()[0].State;
            var settings = GetEntities<SettingsGroup>()[0].Settings;

            state.ShouldSpawnNewElements = false;

            int countRemoved = 0;

            foreach (var animal in animals)
            {
                if (animal.State.IsRemoved == false)
                    continue;

                countRemoved += 1;

                Vector2 aboveCoord = animal.Coord.Value - Vector2.up;
                foreach (var fallingAnimal in animals)
                {
                    if (fallingAnimal.State.IsRemoved)
                        continue;
                    if (fallingAnimal.Coord.Value != aboveCoord)
                        continue;

                    fallingAnimal.Coord.Value = animal.Coord.Value;
                    animal.Coord.Value = aboveCoord;
                    fallingAnimal.Move2D.TargetValue = ElementMoveSystem.GetPosition(fallingAnimal.Coord.Value,
                        settings.ElementSize, settings.StartPoint);
                    animal.Move2D.TargetValue = ElementMoveSystem.GetPosition(animal.Coord.Value, settings.ElementSize,
                        settings.StartPoint);
                    animal.Move2D.isFalling = true;
                    fallingAnimal.Move2D.isFalling = true;
                    animal.Move2D.ShouldMove = true;
                    fallingAnimal.Move2D.ShouldMove = true;
                    return;
                }
            }

            state.ShouldSpawnNewElements = countRemoved > 0;
        }
    }
}
