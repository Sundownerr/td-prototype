using System;
using Satisfy.Variables;
using UnityEngine;

namespace TestTD.Variables
{
    [CreateAssetMenu(fileName = "Selectable", menuName = "Variables/Custom/Selectable")]
    [Serializable]
    public class SelectableVariable : VariableSO<Selectable>
    {
        public void SetNullValue()
        {
            base.SetValue(null);
            
        }

        public virtual void SetValue(GameObject value)
        {
            if (value == null)
                return;

            SetValue(value.GetComponent<Selectable>());
        }
    }
}
