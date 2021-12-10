using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TestTD.Data
{
    [HideMonoScript]
    [CreateAssetMenu(fileName = "Float parameter data", menuName = "Data/Float parameter data")]
    [Serializable]
    public class FloatParameterSO : ScriptableObject
    {
        [SerializeField] private Descriptor descriptor;
        [SerializeField, LabelWidth(90)] private float min = 0;
        [SerializeField, LabelWidth(90)] private float max = 0;

        public float Min => min;
        public float Max => max;
        public bool NoMaxLimit => Mathf.Approximately(max, 0);

        public Descriptor Descriptor => descriptor;
    }
}