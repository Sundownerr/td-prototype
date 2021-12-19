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
using System.Linq;
using TestTD.Entities;
using Satisfy.Managers;
using Cysharp.Threading.Tasks;

namespace TestTD.Systems
{
    [Serializable, CreateAssetMenu(fileName = "Enemy Manager", menuName = "System/Enemy Manager")]
    [HideMonoScript]
    public class EnemyManager : ListenerSystem
    {
        [SerializeField, Variable_R] private IntVariable currentWave;
        [SerializeField, Variable_R] private FloatVariable damageToPlayer;
        [SerializeField, Variable_R] private GameObjectVariable spawnPoint;
        [SerializeField, Tweakable] private UnityEvent onAllWavesCompleted;

        private List<WaveSO> waves = new List<WaveSO>();

        public override void Initialize()
        {
            base.Initialize();
        }

        [Button]
        public void SpawnNextWave()
        {
            if (currentWave.Value > waves.Count - 1)
            {
                onAllWavesCompleted?.Invoke();
                return;
            }

            SpawnWave(waves[currentWave.Value]);

            currentWave.IncreaseBy(1);
        }

        public void SetWaves(WaveListSO wavesSO)
        {
            waves = wavesSO.List;
        }

        [Button]
        public async void SpawnWave(WaveSO wave)
        {
            foreach (var enemy in wave.Enemies)
            {
                Create(enemy);
                await UniTask.Delay(400);
            }
        }

        private void Create(EnemyData data)
        {
            var prefab = data.WavePrefabs.First().Groups.First().Prefabs.First().List[0];

            var spawnedEnemy = Instantiate(prefab,
                                           spawnPoint.Value.transform.position,
                                           Quaternion.identity,
                                           spawnPoint.Value.transform)
                .GetComponent<EnemyBehaviour>();

            spawnedEnemy.SetData(data);

            // Observable.IntervalFrame(1).Take(1).Subscribe(_ =>
            // {
            spawnedEnemy.ReachedPlayer.Subscribe(_ =>
            {
                var damage = GetDamageToPlayer(spawnedEnemy.Health, data.Parameters.DamageToPlayer.Value);
                damageToPlayer.SetValueAndPublish(damage);
            });
            // });
        }

        private float GetDamageToPlayer(Health health, float defaultDamage)
        {
            var remainingHealth = health.Value / health.Max;

            return defaultDamage * remainingHealth;
        }
    }
}