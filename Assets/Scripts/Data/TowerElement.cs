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
    [CreateAssetMenu(fileName = "TowerElement", menuName = "Data/Tower element")]
    [HideMonoScript]	
    public class TowerElement : ScriptableObject
    {
        [SerializeField] private Descriptor descriptor;
        [SerializeField] private Sprite sprite;
        [SerializeField] private int level;
        [SerializeField] private int investedPoints;
        [SerializeField,InlineEditor] private LevelingData levelingData;
        
        public Sprite Sprite => sprite;
        public int Level => level;
        public int InvestedPoints => investedPoints;
        public bool CanInvest => levelingData.IsLevelValid(level + 1);
        public Descriptor Descriptor => descriptor;

        public void Invest()
        {
            var canLevelUp = levelingData.TryGetNeededExp(level + 1, out var neededExp);

            if (!canLevelUp)
                return;
            
            investedPoints++;

            if (investedPoints >= neededExp)
            {
                level++;
                investedPoints = 0;
            }
        }

    
        
        public void ResetValues()
        {
            level = 0;
            investedPoints = 0;
        }
        
    }
}