﻿using System;
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
    [Serializable, CreateAssetMenu(fileName = "Wave", menuName = "Data/Wave")]
    [HideMonoScript]
    public class WaveSO : ScriptableObject
    {
        [ListDrawerSettings(Expanded = true)]
        [SerializeField, Tweakable] private List<EnemyData> enemies;

        public List<EnemyData> Enemies => enemies;
    }
}