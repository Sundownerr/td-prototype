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
using UnityEngine.UI;
using TMPro;

namespace TestTD.UI
{
    [HideMonoScript]
    public class PlayerHealthUI : UIElement
    {
        [SerializeField, Variable_R] FloatVariable playerShieldMax;
        [SerializeField, Variable_R] FloatVariable playerShield;
        [SerializeField, Variable_R] FloatVariable playerHealthMax;
        [SerializeField, Variable_R] FloatVariable playerHealth;
        [SerializeField, Editor_R] Image shieldBar;
        [SerializeField, Editor_R] Image shieldBarPrevious;
        [SerializeField, Editor_R] Image healthBar;
        [SerializeField, Editor_R] Image healthBarPrevious;

        private void Start()
        {
            UpdateShield();
            UpdateHealth();

            playerShield.Changed.Subscribe(x =>
            {
                UpdateShield();
            }).AddTo(this);

            playerHealth.Changed.Subscribe(_ =>
            {
                UpdateHealth();
            }).AddTo(this);
        }

        private void UpdateHealth() =>
            UpdateUI(healthBar, healthBarPrevious, playerHealth.Value, playerHealthMax.Value);

        private void UpdateShield() =>
            UpdateUI(shieldBar, shieldBarPrevious, playerShield.Value, playerShieldMax.Value);

        private void UpdateUI(Image currentImage, Image previousImage, float current, float max)
        {
            var prevFillAmount = currentImage.fillAmount;
            var fillAmount = current / max;
            DOTween.To(() => currentImage.fillAmount, val => { currentImage.fillAmount = val; }, fillAmount, 0.3f);

            previousImage.fillAmount = prevFillAmount;
        }
    }
}