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
using TestTD.Variables;

namespace TestTD.Systems
{
    [Serializable, CreateAssetMenu(fileName = "Enemy Manager", menuName = "System/Enemy Manager")]
    [HideMonoScript]
    public class EnemyManager : ListenerSystem
    {
        [SerializeField, Variable_R] private IntVariable currentWave;
        [SerializeField, Variable_R] private FloatVariable damageToPlayer;
        [SerializeField, Variable_R] private GameObjectVariable spawnPoint;
        [SerializeField, Variable_R] private EnemyListSO currentEnemies;
        [SerializeField, Variable_R] private EnemyDataVariable enemyDefeated;
        [SerializeField, Tweakable] private UnityEvent onWaveCompleted;
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
            var waveEnemies = new List<EnemyBehaviour>(wave.Enemies.Count);

            foreach (var enemy in wave.Enemies)
            {
                waveEnemies.Add(Create(enemy));
                await UniTask.Delay(400);
            }

            Observable.Merge(waveEnemies.Select(x => Observable.Merge(x.Health.Dead, x.ReachedPlayer)))
                .Skip(waveEnemies.Count - 1)
                .Take(1)
                .DelayFrame(1)
                .Subscribe(_ =>
                {
                    Debug.Log("wave completed");
                    onWaveCompleted?.Invoke();
                });
        }

        private EnemyBehaviour Create(EnemyData data)
        {
            var prefab = data.WavePrefabs.First().Groups.First().Prefabs.First().List[0];

            var spawnedEnemy = Instantiate(prefab,
                                           spawnPoint.Value.transform.position,
                                           Quaternion.identity,
                                           spawnPoint.Value.transform)
                .GetComponent<EnemyBehaviour>();

            spawnedEnemy.SetData(data);

            spawnedEnemy.ReachedPlayer.Subscribe(_ =>
            {
                var damage = GetDamageToPlayer(spawnedEnemy.Health, data.Parameters.DamageToPlayer.Value);
                damageToPlayer.SetValueAndPublish(damage);
            });

            spawnedEnemy.Health.Dead.Subscribe(_ =>
            {
                enemyDefeated.SetValueAndPublish(data);
            });

            Observable.Merge(
                spawnedEnemy.ReachedPlayer,
                spawnedEnemy.Health.Dead)
                .Subscribe(_ =>
                {
                    currentEnemies.Remove(spawnedEnemy);
                });

            currentEnemies.Add(spawnedEnemy);

            return spawnedEnemy;
        }

        private float GetDamageToPlayer(Health health, float defaultDamage)
        {
            var remainingHealth = health.Value / health.Max;

            return defaultDamage * remainingHealth;
        }
    }
}