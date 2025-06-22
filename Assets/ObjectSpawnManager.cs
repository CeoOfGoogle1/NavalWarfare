using Unity.Netcode;
using UnityEngine;

public class ObjectSpawnManager : NetworkBehaviour
{

    public static ObjectSpawnManager Instance;

    private void Awake()
    {

        DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    

    [ClientRpc]
    public void SpawnVisualBulletClientRpc(string spriteName, Vector3 position, Quaternion rotation)
    {
        if (NetworkManager.Singleton.IsServer) return;

        GameObject obj = new GameObject("Projectile");
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        var proj = obj.AddComponent<ProjectileController>();
        proj.SetOnlyVisual(true);

        Sprite sprite = Resources.Load<Sprite>("Textures/Projectiles/" + spriteName);
        if (sprite == null)
        {
            Debug.LogError("Sprite not found: " + spriteName);
            return;
        }

        var sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
    }
}
