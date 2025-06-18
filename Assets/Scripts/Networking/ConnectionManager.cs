using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class ConnectionManager : NetworkBehaviour
{
    [SerializeField] GameObject shipPrefab;

    [SerializeField] Button hostButton;
    [SerializeField] Button joinButton;
    [SerializeField] Button spawnShipButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        joinButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
        spawnShipButton.onClick.AddListener(SpawnShip);
    }

    private void SpawnShip()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        SpawnShipServerRpc(worldPos);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnShipServerRpc(Vector2 pos, ServerRpcParams serverRpcParams = default)
    {
        GameObject ship = Instantiate(shipPrefab, pos, Quaternion.identity);
        ship.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);
    }
}
