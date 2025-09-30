using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Fantasy3D
{
    public class SpeedItem : MonoBehaviour
    {
        [SerializeField] LayerMask _layerMask;
        [SerializeField] float _radius;
        [SerializeField] GameObject _pickUpUI;
        Collider[] _colliders;
        bool _canPickUp = false;
        PlayerMove _playerMove;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) && _canPickUp)
            {
                if(_playerMove != null)
                {
                    StartCoroutine(_playerMove.SpeedUPCo(this.gameObject));
                    _pickUpUI.SetActive(false);
                    _canPickUp = false;
                }
                //Destroy(this.gameObject);
            }
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
