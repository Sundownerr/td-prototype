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

        public IObservable<GameObject> Loaded => loaded;

        private bool isLoading;
        private readonly Subject<GameObject> loaded = new Subject<GameObject>();

        private void Load()
        {
            pool.TrySpawn(ref loadedProjectile,
                          shootPoint.position,
                          Quaternion.LookRotation(shootPoint.forward, shootPoint.up),
                          pool.transform);

            loadedProjectile.transform.position = shootPoint.position;

            Debug.Log("Loading projectle");
            loaded.OnNext(loadedProjectile);

        }

        public override void Initialize()
        {
            targetProvider.GotNewTarget.Where(_ => !isLoading)
                .Subscribe(_ =>
                {
                    isLoading = true;

                    Debug.Log("Start loading");

                    Observable.Interval(TimeSpan.FromSeconds(reloadTime))
                        .TakeWhile(_ => targetProvider.HaveTargets)
                        .Subscribe(_ =>
                        {
                            Load();
                        }).AddTo(this);
                }).AddTo(this);

            targetProvider.ObserveEveryValueChanged(x => x.HaveTargets)
                .Where(x => x == false)
                .Subscribe(_ =>
                {
                    Debug.Log("Stop loading");
                    isLoading = false;
                }).AddTo(this);
        }

        public void DisposeProjectile(GameObject projectile)
        {
            Debug.Log("Dispose projectile");

            pool.Despawn(projectile, 0.2f);
        }
    }
}