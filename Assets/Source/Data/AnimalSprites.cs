using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "AnimalSprites", menuName = "Configs/Animal Sprites")]
    public class AnimalSprites : ScriptableObject
    {
        public Sprite[] Sprites;
    }
}
