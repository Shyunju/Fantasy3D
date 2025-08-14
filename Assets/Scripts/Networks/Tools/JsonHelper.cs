using UnityEngine;
using System;
namespace Fantasy3D
{
    public class JsonHelper
    {
        [Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
        public static T[] GetJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }
    }
}
