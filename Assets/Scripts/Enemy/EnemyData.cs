using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using UniRx;
using Satisfy.Variables;
using Satisfy.Attributes;

namespace TestTD.Data
{
    [Serializable, CreateAssetMenu(fileName = "Enemy Data", menuName = "Data/Enemy")]
    [HideMonoScript]
    public class EnemyData : ScriptableObject
    {
        [SerializeField, Tweakable] private Descriptor descriptor;
        [SerializeField, Tweakable] private GameObject prefab;

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

        public Descriptor Descriptor => descriptor;

        public float GetValue(FloatParameterSO data)
        {
            return parameters.Find(x => x.Data == data).GetValue();
        }
    }
}