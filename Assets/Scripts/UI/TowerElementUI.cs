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
using TMPro;
using UnityEngine.UI;

namespace TestTD.UI
{
    [Serializable]
    public class TowerElementEvent : UnityEvent<TowerElement> { }

    [HideMonoScript]
    public class TowerElementUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Button investButton;
        [SerializeField] private TowerElementEvent onInvest;
        private readonly Subject<int> elementChanged = new Subject<int>();

        private bool canInvest = true;

        public void SetElement(TowerElement element)
        {
            elementChanged.OnNext(1);

            image.sprite = element.Sprite;

            investButton.onClick.RemoveAllListeners();
            investButton.onClick.AddListener(() =>
            {
                element.Invest();
                onInvest?.Invoke(element);

                canInvest = element.Level.CanLevelUp;
            });

            SetLevel(element.Level.Value);

            element.Level.Changed
                .TakeUntil(elementChanged)
                .Subscribe(SetLevel).AddTo(this);

            this.ObserveEveryValueChanged(x => x.canInvest)
                .Where(x => x == false).Subscribe(_ =>
                {
                    DisableInvest();
                }).AddTo(this);
        }

        private void SetLevel(int level)
        {
            levelText.text = $"{level}";
        }

        public void EnableInvest()
        {
            if (!canInvest)
                return;

            investButton.gameObject.SetActive(true);
        }

        public void DisableInvest()
        {
            investButton.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            investButton.onClick.RemoveAllListeners();
        }
    }
}