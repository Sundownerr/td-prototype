﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using UniRx;
using Satisfy.Variables;
using Satisfy.Attributes;
using TestTD.Data;
using Satisfy.Managers;

namespace TestTD.Systems
{
    [Serializable, CreateAssetMenu(fileName = "Tower Generator", menuName = "System/Tower Generator")]
    [HideMonoScript]
    public class TowerGenerator : ScriptableObjectSystem
    {
        [SerializeField, Variable_R] private IntVariable currentWave;
        [SerializeField, Variable_R] private FloatParameterSO waveRequirement;
        [SerializeField, Variable_R] private TowerElementListSO elements;
        [SerializeField, Variable_R] private TowerDataListSO towers;
        [SerializeField, Variable_R] private TowerDataListSO availableTowers;

        [Button]
        public void Generate()
        {
            var leveledElements = elements.List.Where(x => x.Level.Value > 0);

            var matchingTowers = towers.List
                .Where(tower => leveledElements.Any(e => tower.Element == e))
                .Where(tower => tower.Parameters.WaveRequirement.Value >= currentWave.Value);

            // Debug.Log($"Adding {matchingTowers.Count()}");

            foreach (var matchingTower in matchingTowers)
            {
                availableTowers.AddExisting(matchingTower);

                // Debug.Log($"Add tower {matchingTower.name}");
            }
        }

        public override void Initialize()
        {
            // throw new NotImplementedException();
        }
    }
}