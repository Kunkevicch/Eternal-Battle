using UnityEngine;

namespace EndlessRoad
{
    interface IFireable
    {
        public void ActivateAmmo();
        public void StartLaunch(Vector3 startPoint, Vector3 endPoint, RaycastHit hit, float speed, float duration, int damage);
    }
}