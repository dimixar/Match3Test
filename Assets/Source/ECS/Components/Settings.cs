using Data;
using UnityEngine;

namespace ECS.Components
{
    public class Settings : MonoBehaviour
    {
        public AnimalData[] AnimalDatas;
        public Vector2 BoardSize;
        public Vector2 ElementSize;

        public Vector2 StartPoint;

        public GameObject AnimalPrefab;
        public float FallStartY;
        public float MoveSpeed = 10f;

        public AnimalData GetRandomAnimalData()
        {
            return AnimalDatas[Random.Range(0, AnimalDatas.Length)];
        }
    }
}
