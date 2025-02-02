using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace EndlessRoad
{
    public class Clouds : MonoBehaviour
    {
        private void Start()
        {
            transform
                .DORotate(new Vector3(0, 360, 0), 50f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear)
                .Play();
        }
    }
}
