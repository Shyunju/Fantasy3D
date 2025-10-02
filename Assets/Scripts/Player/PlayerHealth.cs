using UnityEngine;
using UnityEngine.UI;

namespace Fantasy3D
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] Image _healthUI;
        float _maxHealth = 100f;
        float _currentHealth = 70f;
        float _originalSize;
        PlayerMove _playerMove;

        private void Start()
        {
            _playerMove = GetComponent<PlayerMove>();
            _originalSize = _healthUI.rectTransform.rect.width;
            ChangeHealth(0f);
            
        }

        public void ChangeHealth(float mount)
        {
            _currentHealth = Mathf.Clamp(_currentHealth + mount, 0, _maxHealth);
            _healthUI.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _originalSize * (_currentHealth / _maxHealth));
            if (_currentHealth == 0)
            {
                //game over
                _playerMove.Anim.SetTrigger("Death");
            }
        }
    }
}
