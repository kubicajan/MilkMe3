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
}