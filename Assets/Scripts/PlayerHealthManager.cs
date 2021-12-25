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
using Satisfy.Managers;
using TestTD.Entities;
using Satisfy.Bricks;

namespace TestTD.Systems
{
    [Serializable, CreateAssetMenu(fileName = "Player Life Manager", menuName = "System/Player Life Manager")]
    [HideMonoScript]
    public class PlayerHealthManager : ScriptableObjectSystem
    {
        [SerializeField, Variable_R] FloatVariable shield;
        [SerializeField, Variable_R] FloatVariable health;
        [SerializeField, Variable_R] FloatVariable shieldRegenerationPerWave;
        [SerializeField, Tweakable] UnityEvent onZeroHealth;
        [SerializeField] private EventListenerEmbedded<FloatEvent, float> floatListener;
        [SerializeField] private BaseListener listener;

        public override void Initialize()
        {
            base.Initialize();
            listener.Initialize();
            floatListener.Initialize();
        }

        [Button, Debugging]
        public void TakeDamage(float damage)
        {
            shield.DecreaseBy(damage);

            if (damage >= shield.Value)
            {
                DamageHealth(damage);
            }
        }

        public void TakeDamage(FloatVariable variable) => TakeDamage(variable.Value);

        [Button, Debugging]
        private void DamageHealth(float damage)
        {
            health.DecreaseBy(damage);

            if (health.Value <= 0)
            {
                onZeroHealth?.Invoke();
            }
        }

        [Button, Debugging]
        public void RegenerateShield()
        {
            shield.IncreaseBy(shieldRegenerationPerWave);
        }
    }
}