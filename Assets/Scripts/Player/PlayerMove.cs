using Unity.VisualScripting;
using UnityEngine;

namespace Fantasy3D
{
    public class PlayerMove : MonoBehaviour
    {
        const float MAXSPEED = 7.0f;

        [SerializeField] float _speed;
        [SerializeField] Transform _cam;
        [SerializeField] float _turnSmoothTime = 0.3f;

        float _horizontal;
        float _vertical;
        float _turnSmoothVelocity;
        
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
                _speed = Mathf.Clamp(_move.magnitude * MAXSPEED, 0.0f, MAXSPEED);
            }
            else
            {
                _speed = 0.0f;
            }
        }
        void Move()
        {
            //Vector3 position = _rigidbody.position;
            //position += _speed * Time.fixedDeltaTime * _move;
            //_rigidbody.MovePosition(position);

            float targetAngle = Mathf.Atan2(_lookDirection.x, _lookDirection.z) * Mathf.Rad2Deg + _cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            _rigidbody.MoveRotation(Quaternion.Euler(0f, angle, 0f));
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _rigidbody.MovePosition(_rigidbody.position + moveDir.normalized * Time.fixedDeltaTime * _speed);
        }

    }
}
