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

namespace TestTD.Data
{
    [CreateAssetMenu(fileName = "Descriptor", menuName = "Data/Descriptor")]
    public class Descriptor : ScriptableObject
    {
        [BoxGroup("Descriptor", false)]
        // [HorizontalGroup("Descriptor/1")]
        [SerializeField, LabelWidth(40), LabelText("Name")]
        private string objectName;

        [BoxGroup("Descriptor", false)]
        // [HorizontalGroup("Descriptor/1")]
        [SerializeField, LabelWidth(70), LabelText("Description"), Multiline]
        private string objectDescription;

        public string Name => objectName;
        public string Description => objectDescription;
    }
}