using ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
    [UpdateAfter(typeof(RespawnElementSystem))]
    public class AppearElementSystem : ComponentSystem
    {
        public struct AnimalGroup
        {
            public Transform Transform;
            public ElementState State;
        }

        public struct SettingsGroup
        {
            public Settings Settings;
        }
        
        protected override void OnUpdate()
        {
            var settings = GetEntities<SettingsGroup>()[0].Settings;
            foreach (var animal in GetEntities<AnimalGroup>())
            {
                if (animal.State.IsMarkedForAppearance == false)
                    continue;

                animal.Transform.localScale = Vector3.Lerp(animal.Transform.localScale, Vector3.one,
                    Time.deltaTime * settings.AppearSpeed);
                if (animal.Transform.localScale != Vector3.one)
                    continue;

                animal.State.IsMarkedForAppearance = false;
                animal.State.IsRemoved = false;
                animal.State.IsReadyForReuse = false;
            }
        }
    }
}