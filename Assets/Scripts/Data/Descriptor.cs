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
    public class Descriptor : ScriptableObject
    {
        [SerializeField, LabelWidth(90), LabelText("Name")]
        private string objectName;
        [SerializeField, LabelWidth(90), LabelText("Description")]
        private string objectDescription;

        public string Name => objectName;
        public string Description => objectDescription;
    }
}