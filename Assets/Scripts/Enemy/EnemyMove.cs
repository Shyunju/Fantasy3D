using UnityEngine;
using UnityEngine.AI;

namespace Fantasy3D
{
    public class EnemyMove : MonoBehaviour
    {
        NavMeshAgent _navMeshAGent;
        [SerializeField] GameObject _target;

        private void Start()
        {
            _navMeshAGent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            _navMeshAGent.SetDestination(_target.transform.position);
            Debug.Log(_navMeshAGent.remainingDistance);
        }
    }
}
