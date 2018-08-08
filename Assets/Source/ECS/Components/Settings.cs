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
        public float MoveSpeed = 10f;
        public float RemoveSpeed = 10f;
        public float FallSpeed = 15f;
        public float AppearSpeed = 10f;
        
        public int GemMatch3Score = 15;
        public int GemMatch4Score = 20;
        public int GemMatch5Score = 30;

        public AnimalData GetRandomAnimalData()
        {
            return AnimalDatas[Random.Range(0, AnimalDatas.Length)];
        }
    }
}
