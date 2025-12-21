using UnityEngine;

namespace Helpers
{
	public static class Utility
	{
		public static Collider2D[] DetectByLayers(Vector2 hitPosition, float areaOfEffect, LayerMask layersToBeDetected)
		{
			return Physics2D.OverlapCircleAll(hitPosition, areaOfEffect, layersToBeDetected);
		}

		public static Vector2 GetGroundBelowLocation(Vector2 suggestedPosition)
		{
			LayerMask groundMask = LayerMask.GetMask("Ground");

			Vector3 rayStart = suggestedPosition + Vector2.up * 10f;

			if (Physics.Raycast(rayStart, Vector2.down, out RaycastHit hit, 5000f, groundMask))
			{
				return hit.point;
			}

			return Vector2.negativeInfinity;
		}

		public static void IgnoreCollisionsByLayers(bool disable, int currentLayer, LayerMask maskLayersToIgnore)
		{
			Physics2D.IgnoreLayerCollision(currentLayer, Mathf.RoundToInt(Mathf.Log(maskLayersToIgnore.value, 2)),
				disable);
		}

		public static bool IsGroundedOnLayers(Vector2 position, LayerMask groundLayers)
		{
			// Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.2f, groundLayers);
			// return colliders.Length > 0;

			Collider2D[] results = new Collider2D[1];
			int count = Physics2D.OverlapCircleNonAlloc(position, 0.2f, results, groundLayers);
			return count > 0;
		}

		public static void SetLaserPosition(LineRenderer line, params Vector2[] positions)
		{
			for (int i = 0; i < line.positionCount; i++)
			{
				line.SetPosition(i, positions[i]);
			}
		}
	}
}