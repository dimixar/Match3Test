using System;
using System.Collections.Generic;
using ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
	public class ScoreSystem : ComponentSystem
	{
		public struct AnimalGroup
		{
			public AnimalId Id;
			public GridCoord Coord;
			public ElementState State;
		}

		public struct SettingsGroup
		{
			public Settings Settings;
		}

		public struct ScoreGroup
		{
			public Score Score;
		}
	
		protected override void OnUpdate()
		{
			var animals = GetEntities<AnimalGroup>();
			var settings = GetEntities<SettingsGroup>()[0].Settings;
			var candidates = new List<AnimalGroup>();
			var score = GetEntities<ScoreGroup>()[0].Score;

			for (var i = 0; i < animals.Length; i++)
			{
				if (animals[i].State.IsMoved == false)
					continue;

				if (animals[i].State.IsMarkedForRemoval || animals[i].State.IsRemoved)
					continue;
			
				candidates.Add(animals[i]);
				animals[i].State.IsMoved = false;
			}

			var allRemoveCandidates = new List<AnimalGroup>();
			foreach (var candidate in candidates)
			{
				var count = 0;
				var removeCandidates = new List<AnimalGroup>();
				CollectCandidates(Vector2.left, candidate, removeCandidates, settings.BoardSize);
				CollectCandidates(Vector2.right, candidate, removeCandidates, settings.BoardSize);
				if (removeCandidates.Count > 1)
				{
					allRemoveCandidates.AddRange(removeCandidates);
					count += removeCandidates.Count;
					removeCandidates.Clear();
				}
			
				CollectCandidates(Vector2.down, candidate, removeCandidates, settings.BoardSize);
				CollectCandidates(Vector2.up, candidate, removeCandidates, settings.BoardSize);
				if (removeCandidates.Count > 1)
				{
					allRemoveCandidates.AddRange(removeCandidates);
					count += removeCandidates.Count;
					removeCandidates.Clear();
				}
				
				if (count > 0)
                    allRemoveCandidates.Add(candidate);
				AssignScore(score, settings, count + 1);
			}

			foreach (var animalGroup in allRemoveCandidates)
			{
				if (animalGroup.State.IsRemoved == false)
					animalGroup.State.IsMarkedForRemoval = true;
			}
		}

		private void CollectCandidates(Vector2 direction, AnimalGroup candidate, List<AnimalGroup> removeCandidates, Vector2 size)
		{
			var candidateId = candidate.Id.Data;
			var nextCoord = candidate.Coord.Value;
			var animals = GetEntities<AnimalGroup>();

			var condition = Math.Abs(direction.x) > 0.001f
				? new Func<Vector2, Vector2, bool>((coord, s) => coord.x >= 0 && coord.x < s.x)
				: (coord, s) => coord.y >= 0 && coord.y < s.y;

			nextCoord += direction;
			bool breakOccured = false;
			while (condition(nextCoord, size) && breakOccured == false)
			{
				foreach (var animal in animals)
				{
					if (animal.Coord.Value != nextCoord)
						continue;
				
					if (animal.Id.Data == candidateId)
						removeCandidates.Add(animal);
					else
						breakOccured = true;
					break;
				}
				nextCoord += direction;
			}
		}

		private void AssignScore(Score score, Settings settings, int count)
		{
			if (count == 3)
			{
				score.Value += count * settings.GemMatch3Score;
			}
			else if (count == 4)
			{
				score.Value += count * settings.GemMatch4Score;
			}
			else if (count >= 5)
			{
				score.Value += count * settings.GemMatch5Score;
			}
		}
	}
}
