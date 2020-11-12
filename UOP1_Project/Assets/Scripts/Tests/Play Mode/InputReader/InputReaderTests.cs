using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.TestTools;

namespace Tests.Play_Mode.InputReader
{
	public class InputReaderTests
	{
		class InputReaderTestFixture : InputTestFixture
		{
			// Input Boilerplate
			private Keyboard Keyboard { get; set; }
			private Mouse Mouse { get; set; }
			private Gamepad Gamepad { get; set; }
			// Systems being tested
			private GameInput GameInput { get; set; }
			private global::InputReader InputReader { get; set; }

			// counters for input event calls
			private int MoveEvents { get; set; }
			private int AttackEvents { get; set; }
			private int JumpEvents { get; set; }
			private int JumpCanceledEvents { get; set; }

			// callback targets for input event calls
			public void OnMove(Vector2 v2) => MoveEvents++;
			public void OnAttack() => AttackEvents++;
			public void OnJump() => JumpEvents++;
			public void OnJumpCanceled() => JumpCanceledEvents++;


			[SetUp] // if asynchronous code is necessary use [UnitySetUp]
			public void SetupInput()
			{
				// must AddDevice<T> to get input in a test environment from those mappings
				Keyboard = InputSystem.AddDevice<Keyboard>();
				Mouse = InputSystem.AddDevice<Mouse>();
				Gamepad = InputSystem.AddDevice<Gamepad>();

				// instantiate a SO to act as the event Observer
				InputReader = ScriptableObject.CreateInstance<global::InputReader>();
				GameInput = InputReader.GameInput;

				ResetCounters();

				// add cb's
				InputReader.moveEvent += OnMove;
				InputReader.attackEvent += OnAttack;
				InputReader.jumpEvent += OnJump;
				InputReader.jumpCanceledEvent += OnJumpCanceled;
			}

			/// <summary>
			/// Removes subscriptions from input reader
			/// </summary>
			[TearDown]
			public void Cleanup()
			{
				// remove cb's
				InputReader.moveEvent -= OnMove;
				InputReader.attackEvent -= OnAttack;
				InputReader.jumpEvent -= OnJump;
				InputReader.jumpCanceledEvent -= OnJumpCanceled;

				ResetCounters();
			}

			/// <summary>
			/// Reset counters used to keep track of events being fired off by inputreader.
			/// should be replaced by a spy or mock when we have access to some library that provides the functionality.
			/// </summary>
			private void ResetCounters()
			{
				MoveEvents = 0;
				AttackEvents = 0;
				JumpEvents = 0;
				JumpCanceledEvents = 0;
			}

			public static string[] InputMappings =
			{
				nameof(GamepadMappings),
				nameof(KeyboardMappings),
			};

			[UnityTest]
			public IEnumerator InputReaderHandlesAttackInput([ValueSource(nameof(InputMappings))] string mappingType)
			{
				var mapping = MappingProviderFactory.Create(mappingType);

				Press(mapping.Attack);
				yield return new WaitForEndOfFrame();
				Release(mapping.Attack);
				yield return new WaitForEndOfFrame();

				Assert.That(AttackEvents, Is.EqualTo(1));
			}


			[UnityTest]
			public IEnumerator InputReaderHandlesMoveInput([ValueSource(nameof(InputMappings))] string mappingType)
			{
				var mapping = MappingProviderFactory.Create(mappingType);

				Press(mapping.MoveLeft);
				yield return new WaitForEndOfFrame();
				Release(mapping.MoveLeft);
				yield return new WaitForEndOfFrame();

				Assert.That(MoveEvents, Is.EqualTo(3));
			}


			/// <summary>
			/// Example case for debugging input and a form of documentation.
			/// </summary>
			/// <param name="mappingType">nameof(some input provider)</param>
			/// <returns>null</returns>
			[UnityTest]
			public IEnumerator InputReaderReceivesInput([ValueSource(nameof(InputMappings))] string mappingType)
			{
				var mapping = MappingProviderFactory.Create(mappingType);

				var attackAction = GameInput.Gameplay.Attack;
				var moveAction = GameInput.Gameplay.Move;

				using (var trace = new InputActionTrace())
				{
					// subscribe to relevant actions so we can track their activity
					trace.SubscribeTo(attackAction);
					trace.SubscribeTo(moveAction);

					yield return new WaitForEndOfFrame();
					Press(mapping.Attack);
					yield return new WaitForEndOfFrame();
					Press(mapping.MoveLeft);
					yield return new WaitForEndOfFrame();
					Release(mapping.Attack);
					yield return new WaitForEndOfFrame();
					Release(mapping.MoveLeft);
					yield return new WaitForEndOfFrame();

					// grab the events from the trace
					var actions = trace.ToArray();
					// LogActionsPerformed(actions);

					Assert.That(actions.Length, Is.EqualTo(6));
				}
			}

			/// <summary>
			/// Debugging tool to see which actions have been fired off. requires the usage of an InputTrace.
			/// </summary>
			/// <param name="actions"></param>
			private static void LogActionsPerformed(InputActionTrace.ActionEventPtr[] actions)
			{
				foreach (var a in actions)
				{
					Debug.Log(a);
				}
			}

			/// <summary>
			/// Set "time" of input fixutre. tracked in the event itself.
			/// </summary>
			/// <param name="t">new syste time</param>
			private void SetInputSystemTime(float t)
			{
				var time = t;
				currentTime = time;
			}
		}
	}
}
