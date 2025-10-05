using UnityEngine;

namespace Samurai.UnityFramework.Defs
{
    public abstract class Definition : ScriptableObject, IDefinition
    {
        [SerializeField] private string _id;

        public string Id => _id;
    }
}