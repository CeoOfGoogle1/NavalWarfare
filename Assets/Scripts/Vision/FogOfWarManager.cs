using UnityEngine;

public class FogOfWarManager : MonoBehaviour
{
    public Material fogRevealMaterial; // uses "Hidden/FogOfWarMask" shader
    public Material fogMaterial; // uses "Sprites/FogOfWarShader2D"
    public SelectedList selectedList;
    public RenderTexture fogMaskRT;
    public float defaultFadeAmountRatio = 0.25f;

    void Start()
    {
        if (fogMaskRT == null)
        {
            fogMaskRT = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32);
            fogMaskRT.wrapMode = TextureWrapMode.Clamp;
            fogMaskRT.filterMode = FilterMode.Bilinear;
        }

        fogMaterial.SetTexture("_FogMask", fogMaskRT);
    }

    void Update()
    {
        RenderTexture.active = fogMaskRT;
        GL.Clear(false, true, Color.black);
        
        // For each unit, blit a circle to the fogMaskRT using fogRevealMaterial
        foreach (GameObject unit in selectedList.allUnitsList)
        {
            if (unit == null) continue;

            Vector3 pos = unit.transform.position;
            Unit unitComp = unit.GetComponent<Unit>();
            if (unitComp == null) continue;

            float visionRadius = unitComp.visionRadius;
            float fadeAmount = visionRadius * defaultFadeAmountRatio;

            // Convert world position to normalized UV in [0,1] coordinates for the fog mask
            Vector2 uvPos = WorldToFogUV(pos);

            fogRevealMaterial.SetVector("_Position", new Vector4(uvPos.x, uvPos.y, 0, 0));
            fogRevealMaterial.SetFloat("_RevealRadius", visionRadius / GetWorldSize()); // scale radius to UV space
            fogRevealMaterial.SetFloat("_FadeAmount", fadeAmount / GetWorldSize());

            Graphics.Blit(null, fogMaskRT, fogRevealMaterial);
        }
        RenderTexture.active = null;
    }

    // Convert world position to fog mask UV coords (assuming fog mask covers certain world bounds)
    Vector2 WorldToFogUV(Vector3 worldPos)
    {
        float worldSize = GetWorldSize();

        float halfWorldSize = worldSize / 2f;

        float u = Mathf.InverseLerp(-halfWorldSize, halfWorldSize, worldPos.x);
        float v = Mathf.InverseLerp(-halfWorldSize, halfWorldSize, worldPos.y);
        return new Vector2(u, v);
    }

    // Return world size covered by fog mask (adjust accordingly)
    float GetWorldSize()
    {
        return transform.localScale.x / 3;
    }
}