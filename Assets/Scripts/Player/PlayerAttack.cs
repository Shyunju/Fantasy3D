using UnityEngine;

namespace Fantasy3D
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] GameObject _weaponHolder;

        BoxCollider _weaponCollider;
        Animator _animator;

        public bool IsAttack { get; set; }

        private void Start()
        {
            if(_weaponCollider != null )
            {
                _weaponCollider.enabled = false;
            }
            _animator = GetComponentInChildren<Animator>();
        }
        private void Update()
        {
            Attack();
        }

        public void AttackStart()
        {
            if (_weaponCollider != null)
            {
                _weaponCollider.enabled=true;
            }
        }
        public void AttackEnd()
        {
            if (_weaponCollider != null)
            {
                _weaponCollider.enabled=false;

            }
        }

        public void EquipRightWeapon(GameObject obj)
        {
            GameObject go = Instantiate(obj, _weaponHolder.transform);
            go.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            SetWeapon(go);
            Destroy(obj);
        }

        void SetWeapon(GameObject obj)
        {
            _weaponCollider = obj.GetComponent<BoxCollider>();
            if (_weaponCollider != null)
            {
                _weaponCollider.enabled = false;
            }
        }

        void Attack()
        {
            if(Input.GetButtonDown("Fire1"))
            {
                IsAttack = true;
                _animator.SetTrigger("Attack");
            }
        }
    }
}
