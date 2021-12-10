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
using TestTD.Variables;

namespace TestTD
{
    [HideMonoScript]
    public class TowerBuilder : MonoBehaviour
    {
        [SerializeField, Variable_R] private CellVariable selectedCell;
        [SerializeField, Variable_R] private TowerVariable selectedTower;
        [SerializeField, Variable_R] private GameObjectList buildedTowers;
        [SerializeField, Tweakable] private GameObjectEvent onTowerBuilded;

        public void BuildTower(TowerDataVariable towerData)
        {
            if (selectedCell.Value == null)
                return;

            var tower = InstantiateTower(towerData.Value.Prefab, selectedCell.Value.transform.position);
            var towerBehaviour = tower.GetComponentInChildren<TowerBehaviour>();

            towerBehaviour.UseCell(selectedCell.Cell);
            towerBehaviour.Select();

            selectedTower.SetTowerBehaviour(towerBehaviour);
            buildedTowers.Add(tower);

            onTowerBuilded?.Invoke(towerBehaviour.gameObject);
        }

        private GameObject InstantiateTower(GameObject prefab, Vector3 position)
        {
            var tower = Instantiate(prefab, position, Quaternion.identity);

            tower.transform.parent = transform;

            return tower;
        }
    }
}