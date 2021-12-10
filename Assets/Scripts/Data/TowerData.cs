using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TestTD.Data
{
    [HideMonoScript]
    [CreateAssetMenu(fileName = "new-tower", menuName = "Data/Tower")]
    [Serializable]
    public class TowerData : Descriptor
    {
        [SerializeField] private GameObject prefab;

        public GameObject Prefab => prefab;

        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, DraggableItems = false)]
        [SerializeField]
        private List<FloatParameter> parameters = new List<FloatParameter>();

        public float GetParameterValue(FloatParameterDescriptor data)
        {
            return parameters.Find(x => x.Descriptor == data).GetValue();
        }
    }
}