using UnityEngine;

namespace ECS.Components
{
    public class ElementState : MonoBehaviour
    {
        public bool IsMoved;
        public bool IsMarkedForRemoval;
        public bool IsRemoved;
        public bool IsReadyForReuse;
        public bool IsMarkedForAppearance;
    }
}