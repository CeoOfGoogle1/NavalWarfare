using System.Collections.Generic;
using UnityEngine;

public class VisualMask : MonoBehaviour
{
    [SerializeField] public RenderTexture visionMask;
    [SerializeField] public Texture2D visionSprite;
    public float pixelsPerUnit = 100f;
    [SerializeField] public List<UnitVision> units;

    void LateUpdate()
    {
        if (visionMask == null || visionSprite == null)
        {
            return;
        }

        RenderTexture.active = visionMask;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, visionMask.width, 0, visionMask.height);
        GL.Clear(true, true, Color.black);

        foreach (UnitVision unit in units)
        {
            Vector2 maskPos = WorldToMaskPosition(unit.transform.position);
            float size = unit.visionRadius * 2f / pixelsPerUnit;

            Rect rect = new Rect(
                maskPos.x - size / 2f,
                visionMask.height - maskPos.y - size / 2f,
                size,
                size
            );

            Graphics.DrawTexture(rect, visionSprite);
        }

        GL.PopMatrix();
        RenderTexture.active = null;
    }

    Vector2 WorldToMaskPosition(Vector3 worldPos)
    {
        Vector2 mapOrigin = Vector2.zero;

        Vector2 mapSize = new Vector2(
            visionMask.width / pixelsPerUnit,
            visionMask.height / pixelsPerUnit);

        float x = (worldPos.x - mapOrigin.x) * visionMask.width;
        float y = (worldPos.y - mapOrigin.y) * pixelsPerUnit;
        return new Vector2(x, y);
    }
}

[System.Serializable]
public class UnitVision
{
    public Transform transform;     // Assign the unit's transform
    public float visionRadius = 5f; // Vision radius in world units
}
