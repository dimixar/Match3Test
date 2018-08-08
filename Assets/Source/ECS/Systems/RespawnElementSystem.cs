using ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
	[UpdateAfter(typeof(FallElementSystem))]
	public class RespawnElementSystem : ComponentSystem
	{
		public struct SettingsGroup
		{
			public Settings Settings;
		}

		public struct AnimalGroup
		{
			public AnimalId Id;
			public ElementState State;
			public SpriteRenderer SpriteRenderer;
		}

		public struct GameStateGroup
		{
			public GameState GameState;
		}
		
		protected override void OnUpdate()
		{
			var state = GetEntities<GameStateGroup>()[0].GameState;
			var settings = GetEntities<SettingsGroup>()[0].Settings;

			if (state.ShouldSpawnNewElements == false)
				return;

			var animals = GetEntities<AnimalGroup>();

			foreach (var animal in animals)
			{
				if (animal.State.IsRemoved == false)
					continue;

				if (animal.State.IsReadyForReuse)
					continue;

				animal.Id.Data = settings.GetRandomAnimalData();
				animal.SpriteRenderer.sprite = animal.Id.Data.GetRandomSprite();
				animal.State.IsMarkedForAppearance = true;
				animal.State.IsMoved = false;
				animal.State.IsReadyForReuse = true;
			}

			state.ShouldSpawnNewElements = false;
		}
	}
}
