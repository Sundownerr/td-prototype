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
        public  const int MaxLevelReached = -11;
        
        [ListDrawerSettings(Expanded = true, DraggableItems = false)]
        [SerializeField] private float[] levelRequirements;

        public bool TryGetNeededExp(int level, out float requiredExp)
        {
            requiredExp = 0;
            
            if (!IsLevelValid(level))
                return false;

            requiredExp = levelRequirements[level];
            return true;
        }

        public bool IsLevelValid(int level) => level <= levelRequirements.Length - 1;
    }
}