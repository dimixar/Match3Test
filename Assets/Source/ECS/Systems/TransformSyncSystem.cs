using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class TransformSyncSystem : ComponentSystem
{
	public struct Data
	{
		[ReadOnly] public Position2D Position;
		public Transform Output;
	}
	
	protected override void OnUpdate()
	{
		foreach (var entity in GetEntities<Data>())
		{
			float2 pos = entity.Position.Value;
			entity.Output.position = new float3(pos.x, pos.y, 0);
		}
	}
}
