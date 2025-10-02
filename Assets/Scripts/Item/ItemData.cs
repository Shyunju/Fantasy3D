using UnityEngine;

namespace Fantasy3D
{
    public enum ItemType
    {
        Speed,
        Strong,
        Health
    }
    public abstract class ItemData : MonoBehaviour
    {
        [SerializeField] protected LayerMask _layerMask;
        [SerializeField] protected float _radius;
        [SerializeField] protected GameObject _pickUpUI;
        [SerializeField] protected ItemType _itemType;
        protected Collider[] _colliders;
        protected bool _canPickUp = false;
        protected PlayerMove _playerMove;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1) && _canPickUp)
            {
                if (_playerMove != null)
                {
                    SendItemInfo();
                    _pickUpUI.SetActive(false);
                    _canPickUp = false;
                }
            }
        }
        protected abstract void SendItemInfo();

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
                _pickUpUI.SetActive(true);
                _canPickUp = true;
                _playerMove = _colliders[0].GetComponent<PlayerMove>();
            }
            else
            {
                _pickUpUI.SetActive(false);
                _canPickUp = false;
            }
        }
    }
}
