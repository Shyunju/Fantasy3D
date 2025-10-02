using UnityEngine;

namespace Fantasy3D
{
    public class EnemyWeapon : MonoBehaviour
    {
        float _damage = -20f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.ChangeHealth(_damage);
                }
            }
        }
    }
}
