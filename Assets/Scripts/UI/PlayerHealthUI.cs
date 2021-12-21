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
        [SerializeField, Variable_R] private FloatVariable playerShieldMax;
        [SerializeField, Variable_R] private FloatVariable playerShield;
        [SerializeField, Variable_R] private FloatVariable playerHealthMax;
        [SerializeField, Variable_R] private FloatVariable playerHealth;
        [SerializeField, Editor_R] private Image shieldBar;
        [SerializeField, Editor_R] private Image shieldBarPrevious;
        [SerializeField, Editor_R] private Image healthBar;
        [SerializeField, Editor_R] private Image healthBarPrevious;

        private float shieldFillAmount => playerShield.Value / playerShieldMax.Value;
        private float healthFillAmount => playerHealth.Value / playerHealthMax.Value;

        private void Start()
        {
            shieldBar.fillAmount = shieldFillAmount;
            healthBar.fillAmount = healthFillAmount;

            shieldBarPrevious.fillAmount = shieldFillAmount;
            healthBarPrevious.fillAmount = healthFillAmount;

            var throttleTime = 0.6f;
            var fillTime = 0.3f;

            playerShield.Changed.Subscribe(x =>
            {
                ChangeFillAmount(shieldBar, shieldFillAmount, fillTime);
            }).AddTo(this);

            playerShield.Changed.Throttle(TimeSpan.FromSeconds(throttleTime))
                .Subscribe(_ =>
                {
                    ChangeFillAmount(shieldBarPrevious, shieldBar.fillAmount, fillTime);
                }).AddTo(this);

            playerHealth.Changed.Subscribe(_ =>
            {
                ChangeFillAmount(healthBar, healthFillAmount, fillTime);
            }).AddTo(this);

            playerHealth.Changed.Throttle(TimeSpan.FromSeconds(throttleTime))
                .Subscribe(_ =>
                {
                    ChangeFillAmount(healthBarPrevious, healthBar.fillAmount, fillTime);
                }).AddTo(this);
        }

        private void ChangeFillAmount(Image image, float targetFillAmount, float time)
        {
            DOTween.Kill(image.fillAmount, true);
            DOTween.To(() => image.fillAmount, val => { image.fillAmount = val; }, targetFillAmount, time);
        }
    }
}