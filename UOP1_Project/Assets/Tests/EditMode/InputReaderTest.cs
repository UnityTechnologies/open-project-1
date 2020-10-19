using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;
using Moq;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Tests
{
    public class InputReaderTest
    {
        private InputReader inputReader;
        [SetUp]
        public void SetUp(){
            inputReader = new InputReader();
        }

        // A Test behaves as an ordinary method
        // [Test]
        // public void InputAttack_CallsAttackFunction()
        // {
        //     Mock<UnityAction> action = new Mock<UnityAction>();
        //     action.Setup(a => a.Invoke());
        //     inputReader.attackEvent += action.Object;
        //     // Can't mock a struct
        //     InputAction.CallbackContext context = new Mock<InputAction.CallbackContext>();
        //     inputReader.OnAttack(context);
        //     action.Verify(a => a.Invoke(), Times.Once());
        // }
    }
}
