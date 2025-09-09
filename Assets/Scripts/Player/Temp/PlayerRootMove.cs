using UnityEngine;

namespace Fantasy3D
{
    public class PlayerRootMove : MonoBehaviour
    {
        Rigidbody _rigidbody;
        Animator _anim;
        void Start()
        {
            _rigidbody = GetComponentInParent<Rigidbody>();
            _anim = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            if(_anim.applyRootMotion)
            {
                _rigidbody.MovePosition(_rigidbody.position + _anim.deltaPosition);
                _rigidbody.MoveRotation(_anim.rootRotation);
                transform.localPosition = Vector3.zero;
            }
        }
    }
}
