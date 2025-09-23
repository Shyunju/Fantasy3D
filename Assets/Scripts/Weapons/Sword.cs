using UnityEngine;

namespace Fantasy3D
{
    public class Sword : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(transform.parent == null && other.tag == "Player")
            {
                PlayerAttack player = other.gameObject.GetComponentInChildren<PlayerAttack>();
                if (player != null)
                {
                    player.EquipRightWeapon(this.gameObject);
                }
            }
            if(other.tag == "Enemy")
            {
                EnemyMove enemy = other.gameObject.GetComponent<EnemyMove>();
                if (enemy != null)
                {
                    enemy.IsDead = true;
                }
            }
        }
    }
}
