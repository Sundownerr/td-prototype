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
using TestTD.Data;
using Unity.Linq;

namespace TestTD.Entities
{
    [HideMonoScript]
    public class TowerBehaviour : MonoBehaviour
    {
        [SerializeField, Editor_R] Transform moduleContainer;

        private void Start()
        {
            var modules = moduleContainer.gameObject.Children().OfComponent<InitializableModule>();

            foreach (var module in modules)
            {
                module.Initialize();
            }
        }

        public void SetData(TowerData data)
        {

        }
    }
}