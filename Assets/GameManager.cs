using UnityEngine;

namespace Fantasy3D
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] GameObject _pickUpUI;
        public GameObject PickUpUI { get { return _pickUpUI; } set { _pickUpUI = value; } }
    }
}
