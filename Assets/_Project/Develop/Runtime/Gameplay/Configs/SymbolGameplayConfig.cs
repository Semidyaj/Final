using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Configs
{
    public abstract class SymbolGameplayConfig : ScriptableObject
    {
        [field: SerializeField] public int SequenceLength { get; private set; }
        [field: SerializeField, TextArea] public string AllowedSymbols { get; private set; }
    }
}
