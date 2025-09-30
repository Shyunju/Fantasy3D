using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Fantasy3D
{
    public enum CameraStyle
    {
        Basic,  //tps
        TopDown
    }
    public class PlayerMove : MonoBehaviour
    {
        //const 
        float MAXSPEED = 7.0f;

        [SerializeField] float _speed;
        [SerializeField] Transform _cam;
        [SerializeField] float _turnSmoothTime = 0.3f;
        [SerializeField] GameObject _topCam;
        [SerializeField] GameObject _tpsCam;

        float _horizontal;
        float _vertical;
        float _turnSmoothVelocity;
        float _higherSpeed = 15f;
        
        Rigidbody _rigidbody;
        Animator _anim;
        Vector3 _move;
        Vector3 _lookDirection = new(0,0,0);

        PlayerAttack _playerAttack;


        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _anim = GetComponentInChildren<Animator>();
            _playerAttack = GetComponentInChildren<PlayerAttack>();
        }
        private void Update()
        {
            SetDirection();
            if(Input.GetKeyDown(KeyCode.Alpha1)) SwitchCamera(CameraStyle.Basic);
            if(Input.GetKeyDown(KeyCode.Alpha2)) SwitchCamera(CameraStyle.TopDown);
            
            _anim.SetFloat("Speed",_speed / MAXSPEED);
        }
        private void FixedUpdate()
        {
            if(!_playerAttack.IsAttack)
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

        void SwitchCamera(CameraStyle newStyle)
        {
            _tpsCam.SetActive(false);
            _topCam.SetActive(false);

            if (newStyle == CameraStyle.Basic) _tpsCam.SetActive(true);
            if (newStyle == CameraStyle.TopDown) _topCam.SetActive(true);
        }
        public void PickUpItem(GameObject go)
        {
            //여기서 코루틴 호출하게, 아이템은 이 메소드를 호출하게
        }
        public IEnumerator SpeedUPCo(GameObject item)
        {
            float temp = MAXSPEED;
            MAXSPEED = _higherSpeed;
            yield return new WaitForSecondsRealtime(1f);
            MAXSPEED = temp;
            Destroy(item.gameObject);
        }

    }
}
