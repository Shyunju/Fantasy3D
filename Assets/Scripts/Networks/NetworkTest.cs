using System.Threading.Tasks;
using UnityEngine;

namespace Fantasy3D
{
    public class NetworkTest : MonoBehaviour
    {
        async void Start()
        {
            //var x = await NetworkManager.Instance.RequestByAsync<ResponseData>("/character/5", eMethod.Get);

            //if(x.errcode == "E0000")
            //{
            //    ReqCharacter[] cha = JsonHelper.GetJsonArray<ReqCharacter>(x.data);

            //    foreach (ReqCharacter ch in cha)
            //    {
            //        Debug.Log(ch.cname);
            //    }
            //}

            ReqCharacter reqch = new()
            {
                user_uno = 10,
                cname ="adsf",
                job = "knight",
                level = 3,
                gender = "m",
                strength = 22,
                agility = 22,
                intelligence = 22,
                maxhealth = 22,
                mana = 22
            };


            string jsonString = JsonUtility.ToJson(reqch);  //Á÷·ÄÈ­
            var x = await NetworkManager.Instance.RequestByAsync<ResponseData>("/character", eMethod.Post, jsonString);
            if (x.errcode == "E0000")
            {
                Debug.Log(x.data);
            }
        }
    }
}
