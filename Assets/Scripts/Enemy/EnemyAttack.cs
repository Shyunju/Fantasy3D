using UnityEngine;

namespace Fantasy3D
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] Collider _attackCollider;
        public void AttackStart()
        {
            _attackCollider.enabled = true;
            
        }
        public void AttackEnd()
        {
            _attackCollider.enabled = false;            
        }
    }
}
