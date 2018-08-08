using Data;
using ECS.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Systems
{
    public class GameboardSystem : ComponentSystem
    {
        struct Settings
        {
            [ReadOnly] public Components.Settings Value;
        }

        private bool _firstLoad = true;

        protected override void OnStartRunning()
        {
            foreach (var entity in GetEntities<Settings>())
            {
                Vector2 boardSize = entity.Value.BoardSize;
                Vector2 startPoint = entity.Value.StartPoint;
                Vector2 elementSize = entity.Value.ElementSize;
                AnimalData[] data = entity.Value.AnimalDatas;
                var settings = entity.Value;
                
                for (int y = 0; y < boardSize.y; y++)
                {
                    for (int x = 0; x < boardSize.x; x++)
                    {
                        var enemy = Object.Instantiate(settings.AnimalPrefab);
                        var pos = new float2(startPoint.x, startPoint.y);
                        var offset = new float2(x * elementSize.x, y * elementSize.y * -1);
                        enemy.GetComponent<Position2D>().Value = pos + offset;
                        var animalData = settings.GetRandomAnimalData();
                        enemy.GetComponent<SpriteRenderer>().sprite = animalData.GetRandomSprite();
                        enemy.GetComponent<AnimalId>().Data = animalData;
                        enemy.gameObject.name = animalData.name + "( " + x + ", " + y + " )";
                        var gridCoord = enemy.GetComponent<GridCoord>();
                        gridCoord.Value.x = x;
                        gridCoord.Value.y = y;
                        enemy.SetActive(true);
                    }
                }

                break;
            }
        }

        protected override void OnUpdate()
        {
        }
    }

    public static class VectorExtensions
    {
        public static Vector3 ToVector3(this Vector2 vector)
        {
            return new Vector3(vector.x, vector.y);
        }

        public static Vector2 Abs(this Vector2 vector)
        {
            return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
        }
    }
}