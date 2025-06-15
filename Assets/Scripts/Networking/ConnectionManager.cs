using Unity.Netcode;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField] Button hostButton;
    [SerializeField] Button joinButton;

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
    }
}
