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
    [HideMonoScript]
    public class TowerParameters
    {
        [SerializeField, InlineProperty] FloatParameter attackDamage;
        [SerializeField, InlineProperty] FloatParameter attackSpeed;
        [SerializeField, InlineProperty] FloatParameter buildCost;
        [SerializeField, InlineProperty] FloatParameter towerLimitRequirement;
        [SerializeField, InlineProperty] FloatParameter waveRequirement;
        [SerializeField, InlineProperty] FloatParameter elementLevelRequirement;

        public FloatParameter ElementLevelRequirement => elementLevelRequirement;
        public FloatParameter WaveRequirement => waveRequirement;
        public FloatParameter TowerLimitRequirement => towerLimitRequirement;
        public FloatParameter BuildCost => buildCost;
        public FloatParameter AttackSpeed => attackSpeed;
        public FloatParameter AttackDamage => attackDamage;
    }
}