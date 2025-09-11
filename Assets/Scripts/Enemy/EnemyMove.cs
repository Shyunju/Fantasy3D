using UnityEngine;
using UnityEngine.AI;

namespace Fantasy3D
{
    public class EnemyMove : MonoBehaviour
    {
        NavMeshAgent _navMeshAGent;
        GameObject _target;
        Animator _animator;
        private void Start()
        {
            _navMeshAGent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            _target = GameObject.Find("Player");
        }

        private void Update()
        {
            if(_target != null)
            {
                _navMeshAGent.SetDestination(_target.transform.position);
                if(_navMeshAGent.remainingDistance <= _navMeshAGent.stoppingDistance)
                {
                    _animator.SetTrigger("Attack");

                }
                //Debug.Log(_navMeshAGent.remainingDistance);
            }
        }
    }
}
