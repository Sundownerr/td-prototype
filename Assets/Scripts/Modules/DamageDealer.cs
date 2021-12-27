using UnityEngine;
using Sirenix.OdinInspector;
using Satisfy.Attributes;
using UniRx;
using System;
using TestTD.Data;

namespace TestTD.Entities
{
    [HideMonoScript]
    public class DamageDealer : InitializableModule
    {
        [SerializeField, Editor_R] private Shooter shooter;
        [SerializeField, Editor_R] private TowerData towerData;
    
        public override void Initialize()
        {
            var damage = towerData.Parameters.AttackDamage.Value;
            
            shooter.AttackSuccesful.Where(x => x != null)
                .Select(x => x.GetComponent<Health>())
                .Subscribe(health =>
                {
                    if (health == null)
                    {
                        Debug.LogError("health module missing");
                        return;
                    }
                    
                    // Debug.Log("dealing damage");
                    health.TakeDamage(damage);
                }).AddTo(this);
        }
    }
}