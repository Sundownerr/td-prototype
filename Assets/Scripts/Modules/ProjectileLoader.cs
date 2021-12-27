using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using Satisfy.Attributes;
using System.Linq;

namespace TestTD.Entities
{
    [HideMonoScript]
    public class ProjectileLoader : InitializableModule
    {
        [SerializeField, Editor_R] private TargetProvider targetProvider;
        [SerializeField, Editor_R] private Transform shootPoint;
        [SerializeField, Editor_R] private Lean.Pool.LeanGameObjectPool pool;
        [SerializeField, Tweakable] private float reloadTime;

        private GameObject loadedProjectile;

        public IObservable<int> Loaded => loaded;

        private bool isLoading;
        private readonly Subject<int> loaded = new Subject<int>();

        private void Load()
        {
            if (loadedProjectile != null)
            {
                loaded.OnNext(1);
                return;
            }

            var shootPointPosition = shootPoint.position;
            
            pool.TrySpawn(ref loadedProjectile,
                          shootPointPosition,
                          Quaternion.LookRotation(shootPoint.forward, shootPoint.up),
                          pool.transform);

            loadedProjectile.transform.position = shootPointPosition;

            // Debug.Log("Loading projectle");
            loaded.OnNext(1);
        }

        public GameObject GiveProjectile()
        {
            var projectile = loadedProjectile;
            loadedProjectile = null;
            return projectile;
        }

        public override void Initialize()
        {
            targetProvider.GotNewTarget.Where(_ => !isLoading)
                .Subscribe(_ =>
                {
                    isLoading = true;

                    // Debug.Log("Start loading");

                    Observable.Interval(TimeSpan.FromSeconds(reloadTime))
                        .TakeWhile(_ => targetProvider.HaveTargets)
                        .Subscribe(_ =>
                        {
                            Load();
                        }).AddTo(this);
                }).AddTo(this);

            targetProvider.ObserveEveryValueChanged(x => x.HaveTargets)
                .Where(x => x == false)
                .Throttle(TimeSpan.FromSeconds(reloadTime))
                .Subscribe(_ =>
                {
                    // Debug.Log("Stop loading");
                    isLoading = false;
                }).AddTo(this);
        }

        public void DisposeProjectile(GameObject projectile)
        {
            // Debug.Log("Dispose projectile");

            pool.Despawn(projectile);
        }
    }
}