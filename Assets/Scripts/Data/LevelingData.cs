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
    [CreateAssetMenu(fileName = "LevelingData", menuName = "Data/Leveling data")]
    [HideMonoScript]
    public class LevelingData : ScriptableObject
    {
        [ListDrawerSettings(Expanded = true, DraggableItems = false)]
        [SerializeField] private float[] levelRequirements;

        public int MaxLevel => levelRequirements.Length;

        public float GetExpForLevel(int level)
        {
            if (level >= MaxLevel - 1)
                return 0;

            return levelRequirements[level];
        }
    }
}