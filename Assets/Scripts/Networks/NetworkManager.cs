using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Fantasy3D
{
    public enum eMethod
    {
        Get,
        Post,
        Put,
        Delete
    }
    public class NetworkManager : Singleton<NetworkManager>
    {
        string _baseUrl = "http://localhost";

        public  async Task<T> RequestByAsync<T>(string uri, eMethod method, string jsonstr = null)
        {
            string requestUrl = _baseUrl + uri;
            UnityWebRequest uwr = UnityWebRequest.Get(requestUrl);

            if (method == eMethod.Post || method == eMethod.Put)
            {
                uwr = UnityWebRequest.PostWwwForm(requestUrl, jsonstr);
                if (method == eMethod.Put) uwr = UnityWebRequest.Put(requestUrl, jsonstr);
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonstr);
                uwr.uploadHandler = new UploadHandlerRaw(jsonToSend); //요청
                uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer(); //응답
                uwr.SetRequestHeader("Content-Type", "application/json");
            }
            else if (method == eMethod.Delete)
            {
                uwr = UnityWebRequest.Delete(requestUrl);
                uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            }

            UnityWebRequestAsyncOperation ao = uwr.SendWebRequest();
            await ao;

            if(ao.isDone)
            {
                try
                {
                    var data = JsonUtility.FromJson<ResponseData>(uwr.downloadHandler.text);
                    if(data.errcode.Equals("E0000"))
                    {
                        T res = JsonUtility.FromJson<T>(uwr.downloadHandler.text);
                        uwr.Dispose();
                        return res;
                    }
                }
                catch(Exception e)
                {
                    Debug.Log(e.Message);
                    uwr.Dispose();
                }
            }
            return default;
        }

    }
}
