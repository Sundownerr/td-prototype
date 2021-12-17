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

namespace TestTD.Systems
{
    [HideMonoScript]
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField, Editor_R] Transform spawnPoint;

        [Button]
        public void SpawnWave(WaveSO wave)
        {
            var index = 0;

            Observable.Interval(TimeSpan.FromSeconds(0.4f))
                .Take(wave.Enemies.Count)
                .Select(x => wave.Enemies[index])
                .Subscribe(enemy =>
                {
                    var prefab = enemy.WavePrefabs.First().Groups.First().Prefabs.First().List[0];
                    var spawnedEnemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity, spawnPoint)
                        .GetComponent<EnemyBehaviour>();

                    spawnedEnemy.SetData(enemy);

                    index++;
                }).AddTo(this);

        }
    }
}