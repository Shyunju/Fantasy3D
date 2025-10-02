using UnityEngine;

namespace Fantasy3D
{
    public enum ItemType
    {
        Speed,
        Strong,
        Health
    }
    public class ItemData : MonoBehaviour
    {
        [SerializeField] LayerMask _layerMask;
        [SerializeField] float _radius;
        [SerializeField] ItemType _itemType;
        Collider[] _colliders;
        bool _canPickUp = false;
        PlayerMove _playerMove;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1) && _canPickUp)
            {
                if (_playerMove != null)
                {
                    SendItemInfo();
                    GameManager.Instance.PickUpUI.SetActive(false);
                    _canPickUp = false;
                }
            }
        }
        void SendItemInfo()
        {
            _playerMove.PickUpItem(_itemType, this.gameObject);
            Destroy(this.gameObject);
        }

        private void FixedUpdate()
        {
            PickUpCheck();
        }
        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;

        //    Gizmos.DrawSphere(this.transform.position, _radius);
        //}
        private void PickUpCheck()
        {
            _colliders = Physics.OverlapSphere(this.transform.position, _radius, _layerMask);
            if (_colliders.Length > 0)
            {
                GameManager.Instance.PickUpUI.SetActive(true);
                _canPickUp = true;
                _playerMove = _colliders[0].GetComponent<PlayerMove>();
            }
            else
            {
                GameManager.Instance.PickUpUI.SetActive(false);
                _canPickUp = false;
            }
        }
    }
}
