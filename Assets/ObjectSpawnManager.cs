using Unity.Netcode;
using UnityEngine;

public class ObjectSpawnManager : NetworkBehaviour
{

    public static ObjectSpawnManager Instance;

    [SerializeField] private GameObject bulletPrefab;

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
    public void SpawnVisualBulletClientRpc(Vector3 position, Quaternion rotation)
    {
        if (NetworkManager.Singleton.IsServer) return;

        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        bullet.GetComponent<Projectile>().SetOnlyVisual(true);
    }
}
