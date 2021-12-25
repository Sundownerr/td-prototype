using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using UniRx;
using Satisfy.Attributes;
using Satisfy.Variables;
using TestTD.Variables;

namespace TestTD
{
    public class CellManager : MonoBehaviour
    {
        [SerializeField, Tweakable] private UnityEvent<Cell> onSelectedTowerCell;

        public void HandleTowerSelected(CellObjectVariable tower)
        {
            onSelectedTowerCell?.Invoke(tower.CellObject.Cell);
        }

        public void ReleaseCell(Selectable cell)
        {
            cell.GetComponent<Cell>().SetFree();
        }
    }
}