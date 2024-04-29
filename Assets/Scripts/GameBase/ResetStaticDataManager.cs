using System;
using Counter;
using UnityEngine;

namespace GameBase
{
    public class ResetStaticDataManager : MonoBehaviour
    {
        private void Awake()
        {
            CuttingCounter.ResetStaticData();
            BaseCounter.ResetStaticData();
            TrashCounter.ResetStaticData();
        }
    }
}