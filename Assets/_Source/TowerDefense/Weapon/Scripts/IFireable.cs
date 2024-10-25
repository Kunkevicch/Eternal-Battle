using UnityEngine;

namespace EndlessRoad
{
    interface IFireable
    {
        public void InitializeAmmo(Vector3 fireDirection, LayerMask impactLayer, int damage, float speed);
        public GameObject GetGameObject();
    }
}