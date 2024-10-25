using UnityEngine;

namespace EndlessRoad
{
    public class Ammo : MonoBehaviour, IFireable
    {
        protected LayerMask _impactLayer;
        protected Vector3 _direction;
        protected int _damage;
        protected float _speed;

        protected float _time = 2f;

        private void Update()
        {
            _time -= Time.deltaTime;
            if (_time <= 0)
            {
                _time = 2f;
                gameObject.SetActive(false);
            }
            transform.position += transform.forward * _speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }

        public void InitializeAmmo(Vector3 startPosition, LayerMask impactLayer, int damage, float speed)
        {
            _impactLayer = impactLayer;
            _direction = startPosition;
            _damage = damage;
            _speed = speed;
        }

        public GameObject GetGameObject() => gameObject;

    }
}
