using Unity.VisualScripting;
using UnityEngine;

namespace Fantasy3D
{
    public class PlayerMove : MonoBehaviour
    {
        const float MAXSPEED = 7.0f;

        [SerializeField] float _speed;

        float _horizontal;
        float _vertical;
        Rigidbody _rigidbody;
        Vector3 _move;
        Vector3 _lookDirection = new(0,0,0);

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            SetDirection();
        }
        private void FixedUpdate()
        {
            Move();
        }
        void SetDirection()
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");


            _move = new(_horizontal,0, _vertical);
            _lookDirection = _move.normalized;

            if(_lookDirection.magnitude >= 0.1f)
            {
                _speed = _move.magnitude * MAXSPEED;
            }
            else
            {
                _speed = 0.0f;
            }
        }
        void Move()
        {
            Vector3 position = _rigidbody.position;
            position += _speed * Time.fixedDeltaTime * _move;
            _rigidbody.MovePosition(position);
        }

    }
}
