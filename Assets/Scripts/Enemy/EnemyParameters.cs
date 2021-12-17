using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TestTD.Data
{
    [Serializable]
    [HideMonoScript]
    public class EnemyParameters
    {
        [SerializeField] FloatParameter speed;
        [SerializeField] FloatParameter health;
        [SerializeField] FloatParameter damageToPlayer;
        [SerializeField] FloatParameter money;

        public FloatParameter Speed => speed;
        public FloatParameter Health => health;
        public FloatParameter DamageToPlayer => damageToPlayer;
        public FloatParameter Money => money;
    }
}