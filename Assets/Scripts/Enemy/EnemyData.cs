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
    public class GroupPrefabs
    {
        [SerializeField, HideLabel] private Satisfy.Variables.Variable group;

        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, DraggableItems = false, ShowItemCount = false)]
        [SerializeField]
        private List<GameObjectList> prefabs;

        public Satisfy.Variables.Variable Group => group;
        public IReadOnlyCollection<GameObjectList> Prefabs => prefabs;
    }

    [Serializable]
    public class WaveSettings
    {
        [HorizontalGroup("1")]
        [HorizontalGroup("1/1")]
        [SerializeField, LabelWidth(30), LabelText("From")] private int fromWave;
        [HorizontalGroup("1/1")]
        [SerializeField, LabelWidth(30), LabelText("To")] private int untilWave;

        [HorizontalGroup("1/2")]
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, DraggableItems = false, ShowItemCount = false)]
        [SerializeField] private List<GroupPrefabs> groups;

        public int UntilWave => untilWave;
        public int FromWave => fromWave;
        public IReadOnlyCollection<GroupPrefabs> Groups => groups;
    }

    [Serializable, CreateAssetMenu(fileName = "Enemy Data", menuName = "Data/Enemy")]
    [HideMonoScript]
    public class EnemyData : SerializedScriptableObject
    {
        [SerializeField, Tweakable] private Descriptor descriptor;

        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, ShowItemCount = false, DraggableItems = false)]
        [SerializeField, Tweakable] private List<WaveSettings> wavePrefabs;

        [SerializeField, Tweakable] private EnemyParameters parameters;

        public Descriptor Descriptor => descriptor;
        public EnemyParameters Parameters => parameters;
        public IReadOnlyCollection<WaveSettings> WavePrefabs => wavePrefabs;

        [Button]
        public void ToJson()
        {
            var file = JsonUtility.ToJson(this, true);
            System.IO.File.WriteAllText(Application.dataPath + "/playerData.json", file);
            System.IO.File.Open(System.IO.Path.Combine(Application.dataPath, "/playerData.json"),
                                System.IO.FileMode.Open);
        }
    }
}