using System;
using System.Collections.Generic;
using System.Linq;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TestTD.Data
{
    [HideMonoScript]
    [CreateAssetMenu(fileName = "Tower Data", menuName = "Data/Tower")]
    [Serializable]
    public class TowerData : ScriptableObject
    {
        [SerializeField, Tweakable] private Descriptor descriptor;
        [SerializeField, Tweakable] private TowerElement element;
        [SerializeField, Tweakable] private GameObject prefab;
        [SerializeField, Tweakable] private Sprite sprite;

        [Space(10)]

        [SerializeField, Tweakable] private FloatParametersVariable defaultParameters;
        [Button(ButtonSizes.Large), Tweakable]
        private void ValidateParameters()
        {
            ParameterValidator.ValidateAndCorrect(parameters, defaultParameters);
        }

        [Space(10)]

        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, DraggableItems = false)]
        [SerializeField, Tweakable, PropertyOrder(100)]
        private List<FloatParameter> parameters = new List<FloatParameter>();

        public GameObject Prefab => prefab;
        public Sprite Sprite => sprite;
        public TowerElement Element => element;

        public float GetValue(FloatParameterSO data)
        {
            return parameters.Find(x => x.Data == data).GetValue();
        }
    }
}