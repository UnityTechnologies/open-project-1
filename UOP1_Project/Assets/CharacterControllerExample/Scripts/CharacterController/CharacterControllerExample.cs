#pragma warning disable 0649
using UnityEngine;

namespace KarimCastagnini.PluggableFSM.Example
{
    public class CharacterControllerExample : StateMachine<Character>
    {
        [SerializeField]
        private Character _character;
        [SerializeField]
        private CharacterTransitionTable _characterTable;

        protected override Character GetData() => _character;
        protected override TransitionTable<Character> GetTransitionTable() => _characterTable;
    }
}