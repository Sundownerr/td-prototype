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
using Satisfy.Managers;

namespace TestTD
{
    [Serializable]
    public class CellObjectEvent : UnityEvent<CellObject> { }

    [Serializable, CreateAssetMenu(fileName = "Tower Builder", menuName = "System/Tower Builder")]
    [HideMonoScript]
    public class TowerBuilder : ListenerSystem
    {
        [SerializeField, Variable_R] private GameObjectVariable towerParent;
        [SerializeField, Variable_R] private CellVariable selectedCell;
        [SerializeField, Tweakable] private CellObjectEvent onTowerBuilded;

        public override void Initialize()
        {
            base.Initialize();
        }

        public void BuildTower(TowerDataVariable towerData)
        {
            var buildPosition = selectedCell.Value.transform.position;

            var tower = Instantiate(towerData.Value.Prefab,
                                    buildPosition,
                                    Quaternion.identity,
                                    towerParent.Value.transform);

            var cellObject = tower.GetComponentInChildren<CellObject>();

            cellObject.UseCell(selectedCell.Cell);

            onTowerBuilded?.Invoke(cellObject);
        }

        private void DestroyTower(CellObject towerBehaviour)
        {
            Destroy(towerBehaviour.Reference);
        }

        public void DestroyTower(CellObjectVariable towerVariable) =>
            DestroyTower(towerVariable.CellObject);
    }
}