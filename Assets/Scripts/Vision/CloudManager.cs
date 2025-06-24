using Unity.Netcode;
using UnityEngine;

public class CloudManager : NetworkBehaviour
{
    public Material cloudRevealMaterial;
    public Material cloudMaterial;
    public SelectedList selectedList;
    public RenderTexture cloudMaskRT;
    public float defaultFadeAmountRatio = 0.25f;

    void Start()
    {
        if (cloudMaskRT == null)
        {
            cloudMaskRT = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32);
            cloudMaskRT.wrapMode = TextureWrapMode.Clamp;
            cloudMaskRT.filterMode = FilterMode.Bilinear;
        }

        cloudMaterial.SetTexture("_CloudMask", cloudMaskRT);
    }

    void Update()
    {
        RenderTexture.active = cloudMaskRT;
        GL.Clear(false, true, Color.black);
        
        // For each unit, blit a circle to the fogMaskRT using fogRevealMaterial
        foreach (GameObject unit in selectedList.allUnitsList)
        {
            if (unit == null) continue;

            if (!unit.GetComponent<NetworkObject>().IsOwner) continue;
            

            Vector3 pos = unit.transform.position;
            Unit unitComp = unit.GetComponent<Unit>();
            if (unitComp == null) continue;

            float visionRadius = unitComp.CloudVisionRadius;
            float fadeAmount = visionRadius * defaultFadeAmountRatio;

            // Convert world position to normalized UV in [0,1] coordinates for the cloud mask
            Vector2 uvPos = WorldToCloudUV(pos);

            cloudRevealMaterial.SetVector("_Position", new Vector4(uvPos.x, uvPos.y, 0, 0));
            cloudRevealMaterial.SetFloat("_RevealRadius", visionRadius / GetWorldSize()); // scale radius to UV space
            cloudRevealMaterial.SetFloat("_FadeAmount", fadeAmount / GetWorldSize());

            Graphics.Blit(null, cloudMaskRT, cloudRevealMaterial);
        }
        RenderTexture.active = null;
    }

    // Convert world position to cloud mask UV coords (assuming cloud mask covers certain world bounds)
    Vector2 WorldToCloudUV(Vector3 worldPos)
    {
        float worldSize = GetWorldSize();

        float halfWorldSize = worldSize / 2f;

        float u = Mathf.InverseLerp(-halfWorldSize, halfWorldSize, worldPos.x);
        float v = Mathf.InverseLerp(-halfWorldSize, halfWorldSize, worldPos.y);
        return new Vector2(u, v);
    }

    // Return world size covered by cloud mask (adjust accordingly)
    float GetWorldSize()
    {
        return transform.localScale.x / 3;
    }
}