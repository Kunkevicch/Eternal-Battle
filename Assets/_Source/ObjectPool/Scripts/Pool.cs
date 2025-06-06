using System;
using UnityEngine;

namespace EndlessRoad
{
    [Serializable]
    public struct Pool
    {
        public int poolSize;
        public GameObject prefab;
        public string componentType;
        public bool isInjected;
    }
}