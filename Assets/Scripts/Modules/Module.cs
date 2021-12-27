using UnityEngine;
using Sirenix.OdinInspector;

namespace TestTD.Entities
{
    [HideMonoScript, InlineProperty]
    public abstract class Module : MonoBehaviour { }

    [HideMonoScript]
    public abstract class InitializableModule : Module
    {
        public abstract void Initialize();
    }
}