using UnityEngine;
using UnityEngine.AI;

namespace Fantasy3D
{
    public class EnemyMove : MonoBehaviour
    {
        NavMeshAgent _navMeshAgent;
        GameObject _target;
        Animator _animator;
        Collider[] _colliders; //오버랩에 들어온 콜리더 배열
        float _maxSpeed = 2.0f;

        [SerializeField] float _attckRange = 1.0f;
        [SerializeField] float _radius = 20f;
        [SerializeField] LayerMask _layer;

        public bool IsDead { get; set; }

        private void Start()
        {
            IsDead = false;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            //_target = GameObject.Find("Player");

        }

        private void Update()
        {
            DeathCheck();
            if(_target != null && !IsDead)
            {
                _animator.SetBool("Walking", true);
                _navMeshAgent.SetDestination(_target.transform.position);
                _navMeshAgent.speed = _maxSpeed;
                if(_navMeshAgent.remainingDistance <= _attckRange && _navMeshAgent.remainingDistance > 0)
                {
                    _animator.SetTrigger("Attack");

                }
                //Debug.Log(_navMeshAGent.remainingDistance);
            }
            else
            {
                _animator.SetBool("Walking", false);
                _navMeshAgent.speed = 0;
            }
        }
        private void FixedUpdate()
        {
            _colliders = Physics.OverlapSphere(this.transform.position, _radius, _layer);
            if (_colliders.Length == 0)
            {
                _target = null;

            }
            else
            {
                foreach (Collider collider in _colliders)
                {
                    _target = collider.gameObject;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, _radius);
        }
        void DeathCheck()
        {
            if(IsDead)
            {
                _navMeshAgent.speed = 0;
                _animator.SetTrigger("Death");
            }

            if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Destroy(this.gameObject);
            }

        }
    }
}
