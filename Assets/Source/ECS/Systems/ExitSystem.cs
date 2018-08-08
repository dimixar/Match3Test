using ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
    public class ExitSystem : ComponentSystem
    {
        public struct InputDataGroup
        {
            public InputData InputData;
        }
        
        protected override void OnUpdate()
        {
            var data = GetEntities<InputDataGroup>()[0].InputData;
            
            if (data.BackDown)
                Application.Quit();
        }
    }
}