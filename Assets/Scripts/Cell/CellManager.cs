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
        public void SetCellUsed(CellVariable cellVariable)
        {
            cellVariable.Cell.SetUsed();
            cellVariable.SetNullValue();
        }

        public void SetCellFree(CellVariable cellVariable)
        {
            cellVariable.Cell.SetFree();
        }
    }
}