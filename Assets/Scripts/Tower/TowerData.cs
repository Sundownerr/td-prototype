using System;
using System.Collections.Generic;
using System.Linq;
using Satisfy.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TestTD.Data
{
    [HideMonoScript]
    [CreateAssetMenu(fileName = "Tower Data", menuName = "Data/Tower")]
    [Serializable]
    public class TowerData : ScriptableObject
    {
        [SerializeField, Tweakable] private Descriptor descriptor;
        [SerializeField, Tweakable] private TowerElement element;
        [SerializeField, Tweakable] private GameObject prefab;
        [SerializeField, Tweakable] private Sprite sprite;
        [SerializeField, Tweakable] private TowerParameters parameters;

        public GameObject Prefab => prefab;
        public Sprite Sprite => sprite;
        public TowerElement Element => element;
        public TowerParameters Parameters => parameters;


        [Button]
        public void ToJson()
        {
            (int id, string name, TowerParameters parameters) data =
                (descriptor.GetInstanceID(), descriptor.name, parameters);

            var file = JsonUtility.ToJson(data, true);
            System.IO.File.WriteAllText(Application.dataPath + "/playerData.json", file);
            System.IO.File.Open(System.IO.Path.Combine(Application.dataPath, "/playerData.json"),
                                System.IO.FileMode.Open);
        }
    }
}