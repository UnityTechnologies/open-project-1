using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KarimCastagnini.PluggableFSM.Example
{
    public class PlayerInputExample : MonoBehaviour
    {
        public bool FireButtonDown => _kb.fKey.isPressed;
        public bool JumpButtonDown => _kb.spaceKey.isPressed;
        public bool DuckButtonDown => _kb.leftCtrlKey.isPressed;

        private Keyboard _kb;       
        private void Awake() => _kb = InputSystem.GetDevice<Keyboard>(); 
    }
}