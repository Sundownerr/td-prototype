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
using UnityEngine.Serialization;

namespace TestTD.UI
{
    [HideMonoScript]	
    public class TowerElementsUI : UIElement
    {
        [SerializeField] private  GameObject elementUIPrefab    ;
        [SerializeField] private Transform elementsUIParent;
        [SerializeField] private IntVariable elementInvestPoints;
        
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false)]
        [SerializeField] private TowerElement[] towerElements;

        private readonly List<TowerElementUI> elementsUI = new List<TowerElementUI>();

        private void Start()
        {
            foreach (var towerElement in towerElements)
            {
                var elementUIGameObject = Instantiate(elementUIPrefab, elementsUIParent);

                var elementUI = elementUIGameObject.GetComponent<TowerElementUI>();
                elementUI.SetElement(towerElement);
                
                elementsUI.Add(elementUI);
            }

            HandleInvestPointsChange(elementInvestPoints.Value);
            
            elementInvestPoints.Changed.Select(x => x.Current)
                .Subscribe(HandleInvestPointsChange).AddTo(this);
            
            Hide();
        }

        private void HandleInvestPointsChange(int currentPoints)
        {
            foreach (var towerElementUI in elementsUI)
            {
                if(currentPoints > 0)
                {
                    towerElementUI.EnableInvest();
                    continue;
                }

                if (currentPoints == 0)
                {
                    towerElementUI.DisableInvest();
                }
            }
        }
    }
}