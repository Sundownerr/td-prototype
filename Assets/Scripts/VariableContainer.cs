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
using System.Linq;

namespace TestTD
{
    [HideMonoScript]
    public class VariableContainer : MonoBehaviour
    {
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, DraggableItems = false)]
        [LabelText(" ")]
        [SerializeField, Variable_R]
        private Satisfy.Variables.Variable[] data;

        public bool Contains(Satisfy.Variables.Variable variable)
        {
            return data.Contains(variable);
        }
    }
}