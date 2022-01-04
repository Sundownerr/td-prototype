using Satisfy.Attributes;
using UnityEngine;

namespace TestTD.UI
{
    public class InventoryDataContainer<T> : MonoBehaviour
    {
        [SerializeField, Tweakable] private T data;
        public T Data => data;
    }
}