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
    [Serializable]
    public class WavePrefabs
    {
        [HorizontalGroup("1")]
        [HorizontalGroup("1/1")]
        [SerializeField, LabelWidth(30), LabelText("From")] private int fromWave;
        [HorizontalGroup("1/1")]
        [SerializeField, LabelWidth(30), LabelText("To")] private int untilWave;

        [HorizontalGroup("1/2")]
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, DraggableItems = false, ShowItemCount = false)]
        [SerializeField] List<GameObject> prefabs;

        public int UntilWave => untilWave;
        public int FromWave => fromWave;
        public IReadOnlyCollection<GameObject> Prefabs => prefabs;
    }

    [Serializable, CreateAssetMenu(fileName = "Enemy Data", menuName = "Data/Enemy")]
    [HideMonoScript]
    public class EnemyData : SerializedScriptableObject
    {
        [SerializeField, Tweakable] private Descriptor descriptor;

        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, ShowItemCount = false, DraggableItems = false)]
        [SerializeField, Tweakable] private List<WavePrefabs> wavePrefabs;

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