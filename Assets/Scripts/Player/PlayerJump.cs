using UnityEngine;

namespace Fantasy3D
{
    public class PlayerJump : MonoBehaviour
    {
        Animator _animator;
        Rigidbody _rigidbody;
        bool _isJump = false;
        bool _isFalling = false;
        bool _isLanding = false;
        bool _isGround = false;
        float _gravityAccel = 9.81f;
        float _jump;
        


        [SerializeField] Vector3 _boxSize;
        [SerializeField] LayerMask _layerMask;
        [SerializeField] float _jumpForce = 7.0f;
        [SerializeField][Range(0.05f, 0.1f)] float _maxDistance;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _rigidbody = GetComponent<Rigidbody>(); 
        }
        void Update()
        {
            _jump = Input.GetAxis("Jump");

            _animator.SetBool("IsJump", _isJump);
            _animator.SetBool("IsLanding", _isLanding);
            _animator.SetBool("IsFalling", _isFalling);
        }

        private void FixedUpdate()
        {
            _isGround = GroundCheck();

            if(!_isGround && _rigidbody.linearVelocity.y < -2.0f)
            {
                _isFalling = true;
            }
            Jumping();
        }

        void Jumping()
        {
            Vector3 velocity = _rigidbody.linearVelocity;
            if(!_isGround)
            {
                velocity.y -= _gravityAccel * Time.fixedDeltaTime;
                _rigidbody.linearVelocity = velocity;
            }

            if(_jump > 0.1f)
            {
                if(_isGround)
                {
                    _isJump = true;
                    velocity.y = _jumpForce;
                    _rigidbody.linearVelocity = velocity;  
                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 origin = transform.position + Vector3.up * _maxDistance;
            Vector3 endPosition = origin + Vector3.down * _maxDistance;

            if(_isGround)
            {
                Gizmos.color = Color.green;

            }
            Gizmos.DrawCube(endPosition, _boxSize);
        }

        bool GroundCheck()
        {
            Vector3 origin = transform.position + Vector3.up * _maxDistance;
            if(Physics.BoxCast(origin, _boxSize / 2, Vector3.down, transform.rotation, _maxDistance, _layerMask))
            {
                _isJump = false;
                _isFalling = false;
                _isLanding = true;
                return true;
            }
            else
            {
                _isLanding = false;
                return false;
            }
        }

    }
}
