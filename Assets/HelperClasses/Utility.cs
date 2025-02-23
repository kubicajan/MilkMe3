using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

public static class Utility
{
    public static Collider2D[] DetectByLayers(Vector2 hitPosition, float areaOfEffect, LayerMask layersToBeDetected)
    {
        return Physics2D.OverlapCircleAll(hitPosition, areaOfEffect, layersToBeDetected);
    }

    public static void IgnoreCollisionsByLayers(bool disable, int currentLayer, LayerMask maskLayersToIgnore)
    {
        Physics2D.IgnoreLayerCollision(currentLayer, Mathf.RoundToInt(Mathf.Log(maskLayersToIgnore.value, 2)), disable);
    }

    public static bool IsGroundedOnLayers(Vector2 position, LayerMask groundLayers)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.2f, groundLayers);
        return colliders.Length > 0;
    }

    public static void SetLaserPosition(LineRenderer line, params Vector2[] positions)
    {
        for (int i = 0; i < line.positionCount; i++)
        {
            line.SetPosition(i, positions[i]);
        }
    }
}