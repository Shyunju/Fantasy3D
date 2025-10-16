using UnityEngine;
using UnityEngine.UI;

namespace Fantasy3D
{
    public class NetworkManagerUI : MonoBehaviour
    {
        [SerializeField] Button _serverButton;
        [SerializeField] Button _hostButton;
        [SerializeField] Button _clientButton;
        void Awake()
        {
            Debug.Log("networkmanagerui awake : initializing");

            _serverButton.onClick.AddListener(() =>
            {
                Debug.Log("server button clicked");
                Unity.Netcode.NetworkManager.Singleton.StartServer();
            });

            _hostButton.onClick.AddListener(() =>
            {
                Debug.Log("host button clicked");
                Unity.Netcode.NetworkManager.Singleton.StartHost();
            });

            _clientButton.onClick.AddListener(() =>
            {
                Debug.Log("client button clicked");
                Unity.Netcode.NetworkManager.Singleton.StartClient();
            });
        }

        private void Start()
        {
            Unity.Netcode.NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
            {
                Debug.Log($"client connected , Id : {id}");
            };
            Unity.Netcode.NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
            {
                Debug.Log($"client disconnected , Id : {id}");
            };
            Unity.Netcode.NetworkManager.Singleton.OnServerStarted += () =>
            {
                Debug.Log("server started");
            };
        }

    }
}
