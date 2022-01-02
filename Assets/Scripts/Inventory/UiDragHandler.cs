using System;
using DG.Tweening;
using Satisfy.Attributes;
using Satisfy.Bricks;
using Satisfy.Managers;
using Satisfy.Variables;
using UniRx;
using UnityEngine;
using Event = Satisfy.Bricks.Event;

namespace TestTD.UI
{
    [CreateAssetMenu(fileName = "UI Drag Handler", menuName = "System/UI Drag Handler")]
    [Serializable]
    public class UiDragHandler : ScriptableObjectSystem
    {
        [SerializeField, Variable_R] private GameObjectVariable draggedParent;
        [SerializeField, Variable_R] private Vector2Variable pointerCurrentPos;
        [SerializeField, Variable_R] private Event release;
      
        public IObservable<InventoryDraggable> EndedDrag => endedDrag;
        private readonly Subject<InventoryDraggable> endedDrag = new Subject<InventoryDraggable>();
        
        public IObservable<InventoryDraggable> StartedDrag => startedDrag;
        private readonly Subject<InventoryDraggable> startedDrag = new Subject<InventoryDraggable>();

        private readonly Subject<InventoryDraggable> stopObserve = new Subject<InventoryDraggable>();

        public void Observe(InventoryDraggable draggable)
        {
            draggable.DragStart.TakeUntil(stopObserve.Where(x => x == draggable))
                .Subscribe(_ =>
                {
                    draggable.transform.SetParent(draggedParent.Value.transform);
                    startedDrag.OnNext(draggable);

                    var speed = 0;
                    DOTween.To(() => speed, val => { speed = val; }, 22, 0.3f);

                    Observable.EveryUpdate()
                        .TakeUntil(release.Raised)
                        .Select(x => draggable.transform)
                        .Subscribe(x =>
                        {
                            var currentPosition = x.position;

                            var targetPos = new Vector3(
                                pointerCurrentPos.Value.x,
                                pointerCurrentPos.Value.y,
                                currentPosition.z);

                            currentPosition = Vector3.Lerp(currentPosition, targetPos, Time.deltaTime * speed);

                            x.position = currentPosition;
                        }).AddTo(draggable);

                    release.Raised.Take(1)
                        .Subscribe(_ =>
                        {
                            endedDrag.OnNext(draggable);
                        }).AddTo(draggable);
                }).AddTo(draggable);
            
            draggable.HighlightStart.TakeUntil(stopObserve.Where(x => x == draggable))
                .Subscribe(_ =>
                {
                    draggable.Highlight();
                }).AddTo(draggable);
            
            draggable.HighlightEnd.TakeUntil(stopObserve.Where(x => x == draggable))
                .Subscribe(_ =>
                {
                    draggable.Dehighlight();
                }).AddTo(draggable);
        }

        public void StopObserve(InventoryDraggable draggable)
        {
            stopObserve.OnNext(draggable);
        }

        public void Place(InventoryDraggable draggable, Vector3 position, float time = 0f)
        {
            if(time == 0f)
            {
                draggable.transform.position = position;
                return;
            }

            draggable.transform.DOKill(true);
            draggable.transform.DOMove(position, time);
        }
    }
}


// var oldPos = rect.position;

//                     if (!IsRectTransformInsideSreen(rect)) { rect.position = oldPos; }
//         private bool IsRectTransformInsideSreen(RectTransform rectTransform)
// {
//     rectTransform.GetWorldCorners(corners);

//     var visibleCorners = 0;
//     var rect = new Rect(0, 0, Screen.width, Screen.height);

//     foreach (Vector3 corner in corners)
//         if (rect.Contains(corner))
//         {
//             visibleCorners++;
//         }

//     return visibleCorners == 4;
// }