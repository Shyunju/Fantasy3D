using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Fantasy3D
{
    public class ChatManager : NetworkBehaviour
    {
        [SerializeField] TMP_InputField _inputField;
        [SerializeField] TMP_Text _chatText;

        private void Start()
        {
            _inputField.onSubmit.AddListener(SendChatMessage);
        }

        void SendChatMessage(string message)
        {
            if(string.IsNullOrWhiteSpace(message))
            {
                return;
            }
            SendMessageServerRpc(message);
            _inputField.text = string.Empty;
        }

        [ServerRpc(RequireOwnership = false)]
        void SendMessageServerRpc(string message, ServerRpcParams rpcParams = default)
        {
            try
            {
                ReceiveMessageClientRpc($"Player{rpcParams.Receive.SenderClientId} : {message}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error sending chat message on server : {e.Message}");
            }
        }
        [ClientRpc]
        void ReceiveMessageClientRpc(string message)
        {
            try
            {
                _chatText.text += message + "\n";
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error receiving chat message on client: {e.Message}");
            }
        }

    }
}
