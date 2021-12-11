using System;
using Satisfy.Variables;
using UnityEngine;

namespace TestTD.Variables
{
    [CreateAssetMenu(fileName = "CellObject", menuName = "Variables/Custom/CellObject")]
    [Serializable]
    public class CellObjectVariable : SelectableVariable
    {
        [SerializeField] private CellObject cellObject;

        public CellObject CellObject => cellObject;

        public void SetCellObject(CellObject cellObject)
        {
            this.cellObject = cellObject;
            SetValue(cellObject);
        }
    }
}