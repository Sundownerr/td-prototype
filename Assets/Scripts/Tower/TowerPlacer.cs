using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using JetBrains.Annotations;
using UniRx;
using Satisfy.Attributes;
using Satisfy.Variables;
using TestTD.Variables;
using Satisfy.Managers;
using Satisfy.Bricks;

namespace TestTD
{
    public static class SourceTemplate
    {

        [SourceTemplate]
        public static void suba<T>(this IObservable<T> x)
        { /*$
            x.Subscribe(_ =>
            {
                 $END$
            }).AddTo(this);
            */
        }
    }

    [Serializable, CreateAssetMenu(fileName = "Tower Placer", menuName = "System/Tower Placer")]
    [HideMonoScript]
    public class TowerPlacer : ScriptableObjectSystem
    {
        private enum State
        {
            Idle, Placing, BadPosition,
        }
        [SerializeField, Variable_R] private Satisfy.Bricks.Event pointerDown;
        [SerializeField, Variable_R] private Satisfy.Bricks.Event pointerUp;
        [SerializeField, Variable_R] private CellObjectVariable selectedTower;
        [SerializeField, Variable_R] private CellVariable highlightedCell;
        [SerializeField, Variable_R] private CellVariable selectedCell;
        [SerializeField, Variable_R] private CellObjectEvent placed;
        [SerializeField, Variable_R] private GameObjectVariable replaceLineVar;
        [SerializeField, Variable_R] private Satisfy.Bricks.SelectableEvent cellReleased;
        [SerializeField, Tweakable] private Vector3 lineOffset;
        [SerializeField, Tweakable] private UnityEvent onStartPlacing;
        [SerializeField, Tweakable] private UnityEvent onEndPlacingSuccess;
        [SerializeField, Tweakable] private UnityEvent onEndPlacingCanceled;
        [SerializeField] private EventListenerEmbedded<CellObjectEvent, CellObject> cellObjectListener;
        [SerializeField] private BaseListener listener;

        private readonly Subject<int> finishedPlacing = new Subject<int>();
        private readonly Subject<int> canceledPlacing = new Subject<int>();
        
        private State state;
        private LineRenderer endPointLine;
        private IObservable<long> whenPlacing => Observable.EveryUpdate()
            .TakeUntil(finishedPlacing.Merge(canceledPlacing));

        private CellObject targetTower;

        public override void Initialize()
        {
            base.Initialize();
            listener.Initialize();
            cellObjectListener.Initialize();
            
            HideLine();

            replaceLineVar.Changed.Select(x => x.Current)
                .Subscribe(x =>
                {
                    endPointLine = x.GetComponent<LineRenderer>();
                });

            finishedPlacing.Subscribe(_ =>
            {
                HideLine();

                targetTower.UseCell(selectedCell.Cell);

                targetTower.Reference.transform.DOMove(
                    selectedCell.Value.transform.position, 0.2f);

                placed.Raise(targetTower);
             
                onEndPlacingSuccess?.Invoke();
            });

            canceledPlacing.Subscribe(_ =>
            {
                HideLine();
                
                targetTower.Reference.transform.DOMove(
                    targetTower.Cell.transform.position, 0.2f);

                targetTower.UseCell(targetTower.Cell);

                placed.Raise(targetTower);
                
                onEndPlacingCanceled?.Invoke();
            });

            finishedPlacing.Merge(canceledPlacing)
                .Subscribe(_ =>
                {
                    state = State.Idle;
                });
        }

        public void Replace(CellObject tower)
        {
            targetTower = tower;
            var movedObject = tower.Reference.transform;
            
            ShowLine(movedObject);
            cellReleased.Raise(tower.Cell);
            
            Place(movedObject);
        }

        public void Replace(CellObjectVariable tower) => Replace(tower.CellObject);

        public void Place(CellObjectVariable tower){    Place(tower.CellObject);}

        public void Place(CellObject tower)
        {
            targetTower = tower;
            Place(tower.Reference.transform);
        }

        public void Place(Transform tower)
        {
            state = State.Placing;
            onStartPlacing?.Invoke();
            
            whenPlacing.Subscribe(_ =>
            {
                var towerPosition = tower.position;
                var targetPosition = GetTargetPosition(towerPosition) + lineOffset;

                towerPosition = Vector3.Lerp(towerPosition,targetPosition,Time.deltaTime * 25f);
                
                tower.position = towerPosition;
            });

            pointerDown.Raised.Where(_ => highlightedCell != null && !highlightedCell.Cell.IsUsed)
                .Take(1)
                // .TakeUntil(canceledPlacing)
                .Subscribe(_ => { finishedPlacing.OnNext(1); });
        }

        private Vector3 GetTargetPosition(Vector3 currentPos)
        {
            var endPosition = currentPos;

            if (highlightedCell.Value != null)
            {
                endPosition = highlightedCell.Value.transform.position;
            }

            return endPosition;
        }

        public void CancelPlacing()
        {
            if (state == State.Idle)
                return;

            if (selectedTower.Value == null)
                return;

            canceledPlacing.OnNext(1);
        }
        
        private void ShowLine(Transform movedObject)
        {
            endPointLine.SetPosition(0, movedObject.position);
            endPointLine.SetPosition(1, movedObject.position);

            endPointLine.gameObject.SetActive(true);
            DOTween.To(() => endPointLine.widthMultiplier, val => { endPointLine.widthMultiplier = val; }, 1, 0.2f);

            whenPlacing.Subscribe(_ =>
            {
                var targetPosition = GetTargetPosition(movedObject.position) + lineOffset;
                endPointLine.SetPosition(1, targetPosition);
            });
        }

        private void HideLine()
        {
            DOTween.To(() => endPointLine.widthMultiplier, val => { endPointLine.widthMultiplier = val; }, 0, 0.1f)
                .OnUpdate(() =>
                {
                    endPointLine.SetPosition(1, targetTower.Reference.transform.position);
                })
                .OnComplete(() =>
                {
                    endPointLine.gameObject.SetActive(false);
                });
        }
    }
}