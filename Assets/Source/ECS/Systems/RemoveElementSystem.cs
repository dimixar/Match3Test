using ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
    [UpdateAfter(typeof(ScoreSystem))]
    public class RemoveElementSystem : ComponentSystem
    {
        public struct AnimalGroup
        {
            public ElementState State;
            public Transform Transform;
        }

        public struct SettingsGroup
        {
            public Settings Settings;
        }
        
        protected override void OnUpdate()
        {
            var animals = GetEntities<AnimalGroup>();
            var settings = GetEntities<SettingsGroup>()[0].Settings;

            foreach (var animal in animals)
            {
                if (animal.State.IsMarkedForRemoval == false)
                    continue;

                animal.Transform.localScale = Vector3.Lerp(animal.Transform.localScale, Vector3.zero,
                    Time.deltaTime * settings.RemoveSpeed);
                if (animal.Transform.localScale != Vector3.zero)
                    continue;

                animal.State.IsMarkedForRemoval = false;
                animal.State.IsRemoved = true;
            }
        }
    }
}
