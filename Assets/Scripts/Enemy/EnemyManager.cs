using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Satisfy.Attributes;
using Satisfy.Bricks;
using Satisfy.Managers;
using Satisfy.Variables;
using Sirenix.OdinInspector;
using TestTD.Data;
using TestTD.Entities;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace TestTD.Systems
{
    [Serializable, CreateAssetMenu(fileName = "Enemy Manager", menuName = "System/Enemy Manager")]
    [HideMonoScript]
    public class EnemyManager : ScriptableObjectSystem
    {
        [SerializeField, Variable_R] private IntVariable currentWave;
        [SerializeField, Variable_R] private FloatEvent damageToPlayer;
        [SerializeField, Variable_R] private GameObjectVariable spawnPoint;
        [SerializeField, Variable_R] private EnemyListSO currentEnemies;
        [SerializeField, Variable_R] private EnemyDataEvent enemyDefeated;
        [SerializeField, Variable_R] private WaveListSO waves;
        [SerializeField, Tweakable] private UnityEvent onWaveCompleted;
        [SerializeField, Tweakable] private UnityEvent onAllWavesCompleted;
        [SerializeField] private BaseListener baseListener;

        public override void Initialize()
        {
            base.Initialize();
            baseListener.Initialize();
        }

        [Button, Debugging]
        public void SpawnNextWave()
        {
            if (currentWave.Value > waves.List.Count - 1)
            {
                onAllWavesCompleted?.Invoke();
                return;
            }

            SpawnWave(waves.List[currentWave.Value]);

            currentWave.IncreaseBy(1);
        }

        [Button, Debugging]
        public async void SpawnWave(WaveSO wave)
        {
            var waveEnemies = new List<EnemyBehaviour>(wave.Enemies.Count);

            foreach (var enemy in wave.Enemies)
            {
                waveEnemies.Add(Create(enemy));
                await UniTask.Delay(400);
            }

            waveEnemies.Select(x => x.Health.ReachedZero.Merge(x.ReachedPlayer)).Merge()
                .Skip(waveEnemies.Count - 1)
                .Take(1)
                .DelayFrame(1)
                .Subscribe(_ =>
                {
                    Debug.Log("wave completed");
                    onWaveCompleted?.Invoke();
                });
        }

        [Button, Debugging]
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
                damageToPlayer.Raise(damage);
            });

            spawnedEnemy.Health.ReachedZero.Subscribe(_ =>
            {
                enemyDefeated.Raise(data);
                Destroy(spawnedEnemy.gameObject);
            });

            spawnedEnemy.ReachedPlayer.Merge(spawnedEnemy.Health.ReachedZero)
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