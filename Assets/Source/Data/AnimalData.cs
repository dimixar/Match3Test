using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "AnimalSprites", menuName = "Configs/Animal Sprites")]
    public class AnimalData : ScriptableObject
    {
        public Sprite Sprites;

        public Sprite GetRandomSprite()
        {
            return Sprites;
        }
    }
}
