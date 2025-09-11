using UnityEngine;
using UnityEngine.AI;

namespace Fantasy3D
{
    public class EnemyMove : MonoBehaviour
    {
        NavMeshAgent _navMeshAGent;
        GameObject _target;

        private void Start()
        {
            _navMeshAGent = GetComponent<NavMeshAgent>();
            _target = GameObject.Find("Player");
        }

        private void Update()
        {
            if(_target != null)
            {
                _navMeshAGent.SetDestination(_target.transform.position);
                if(_navMeshAGent.remainingDistance <= _navMeshAGent.stoppingDistance)
                {
                    //todo
                    //공격애니메이션

                }
                //Debug.Log(_navMeshAGent.remainingDistance);
            }
        }
    }
}
