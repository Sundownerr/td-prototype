using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TestTD.Data
{
    [Serializable]
    [HideMonoScript]
    public class EnemyParameters
    {
        [SerializeField] private FloatParameter speed;
        [SerializeField] private FloatParameter health;
        [SerializeField] private FloatParameter damageToPlayer;
        [SerializeField] private FloatParameter money;

        public FloatParameter Speed => speed;
        public FloatParameter Health => health;
        public FloatParameter DamageToPlayer => damageToPlayer;
        public FloatParameter Money => money;
    }
}