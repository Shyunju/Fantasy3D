using Newtonsoft.Json;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fantasy3D
{
    [Serializable]
    public struct JoinRoomData
    {
        public string roomName;
        public string userName;
    }
    [Serializable]
    public struct ChatMessageData
    {
        public string userName;
        public string message;
    }
    public class ChatClient : MonoBehaviour
    {
        [SerializeField] TMP_InputField _serverAddressInput;
        [SerializeField] TMP_InputField _roomNameInput;
        [SerializeField] TMP_InputField _userNameInput;
        [SerializeField] TMP_InputField _messageInput;
        [SerializeField] TMP_Text _chatDisplay;
        [SerializeField] ScrollRect _chatScrollRect;
        [SerializeField] Button _connectButton;
        [SerializeField] Button _joinRoomButton;

        SocketIOUnity _socket;

        string _currentRoomName;
        string _currentUserName;

        private void Start()
        {
            _connectButton.onClick.AddListener(ConnectToServer);
            _joinRoomButton.onClick.AddListener(JoinRoom);
            _messageInput.onSubmit.AddListener(SendChatMessage);

        }
        private void OnDestroy()
        {
            if(_socket != null )
            {
                _socket.Disconnect();
                _socket.Dispose();  //메모리 해제
            }

        }
        private void ConnectToServer()
        {
            string uri = _serverAddressInput.text;
            if(string.IsNullOrEmpty(uri) )
            {
                Debug.Log("plz enter server url");
                return;
            }
            _socket = new SocketIOUnity(uri, new SocketIOOptions
            {
                Query = new Dictionary<string, string>
                {
                    {"token", "UNITY" }
                },
                EIO = EngineIO.V4,
                Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
            });
            _socket.JsonSerializer = new NewtonsoftJsonSerializer();

            _socket.OnConnected += (sender, e) =>
            {
                try
                {
                    Debug.Log("Connecte server");
                }
                catch (Exception ex)
                {
                    Debug.Log($"error on connect : {ex.Message}");
                }
            };

            _socket.OnDisconnected += (sender, e) =>
            {
                Debug.Log("disconnected server");
            };
            _socket.OnError += (sender, e) =>
            {
                Debug.Log($"coneection error: {e}");
            };

            _socket.OnUnityThread("chat message", (response) =>
            {
                try
                {
                    List<ChatMessageData> data = JsonConvert.DeserializeObject<List<ChatMessageData>>(response.ToString());
                    ChatMessageData receivedData = data[0];
                    AppendToChatDisplay($"[{receivedData.userName}]: {receivedData.message}");
                }
                catch(Exception ex)
                {
                    Debug.LogError($"Eroor parsing chat message: {ex.Message}");
                }
            });
            Debug.Log("try to connecting");
            _socket.Connect();
        }
        void JoinRoom()
        {
            if(_socket == null || !_socket.Connected)
            {
                Debug.Log("please connet to the server first");
                return;
            }
            string room = _roomNameInput.text;
            string user = _userNameInput.text;

            if(string.IsNullOrEmpty(room) || string.IsNullOrEmpty(user))
            {
                Debug.Log("please enter your romm name and user name");
                return;
            }

            _currentRoomName = room;
            _currentUserName = user;

            JoinRoomData joinData = new JoinRoomData { roomName = _currentRoomName, userName = _currentUserName };
            string json = JsonUtility.ToJson(joinData);

            _socket.Emit("join room", json);
            AppendToChatDisplay($"{_currentUserName} sent a reauest to join room {_currentRoomName}");

            _connectButton.interactable = false;
            _joinRoomButton.interactable = false;             
            
        }
        void SendChatMessage(string message)
        {
            if (_socket == null || !_socket.Connected || string.IsNullOrEmpty(_currentRoomName))
            {
                AppendToChatDisplay("please join room first");
                return;
            }
            if(string.IsNullOrEmpty(message))
            {
                return;
            }

            ChatMessageData chatData = new ChatMessageData { message = message};
            string json = JsonUtility.ToJson(chatData);
            _socket.Emit("chat message", json);
            _messageInput.text = "";
        }

        void AppendToChatDisplay(string message)
        {
            _chatDisplay.text += message + "\n";
            ScrollToButtom();
        }

        void ScrollToButtom()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_chatScrollRect.content);
            _chatScrollRect.verticalNormalizedPosition = 0f;
        }


    }
}
