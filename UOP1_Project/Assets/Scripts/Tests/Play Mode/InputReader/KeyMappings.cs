using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Tests.Play_Mode.InputReader
{
	public interface IMappingProvider
	{
		ButtonControl MoveLeft { get; }
		ButtonControl MoveRight { get; }
		ButtonControl MoveUp { get; }
		ButtonControl MoveDown { get; }

		ButtonControl Jump { get; }
		ButtonControl Interact { get; }
		ButtonControl Attack { get; }
		ButtonControl Extra { get; }

		ButtonControl Pause { get; }
	}

	public static class MappingProviderFactory
	{
		public static IMappingProvider Create(string type)
		{
			switch (type)
			{
				case nameof(KeyboardMappings):
					return new KeyboardMappings();
				case nameof(GamepadMappings):
					return new GamepadMappings();
				default:
					throw new ArgumentException("string provided must be the nameof() some IMappingProvider. e.g. KeyboardMappings or GamepadMappings");
			}
		}
	}

	public class KeyboardMappings : IMappingProvider
	{
		public ButtonControl MoveLeft { get; private set; } = Keyboard.current.aKey;
		public ButtonControl MoveRight { get; private set; } = Keyboard.current.aKey;
		public ButtonControl MoveUp { get; private set; } = Keyboard.current.sKey;
		public ButtonControl MoveDown { get; private set; } = Keyboard.current.wKey;

		public ButtonControl Interact { get; private set; } = Keyboard.current.kKey;
		public ButtonControl Jump { get; private set; } = Keyboard.current.spaceKey;
		public ButtonControl Attack { get; private set; } = Keyboard.current.jKey;
		public ButtonControl Extra { get; private set; } = Keyboard.current.lKey;

		public ButtonControl Pause { get; private set; } = Keyboard.current.escapeKey;
	}

	public class GamepadMappings : IMappingProvider
	{
		public ButtonControl MoveLeft { get; private set; } = Gamepad.current.leftStick.left;
		public ButtonControl MoveRight { get; private set; } = Gamepad.current.leftStick.right;
		public ButtonControl MoveUp { get; private set; } = Gamepad.current.leftStick.up;
		public ButtonControl MoveDown { get; private set; } = Gamepad.current.leftStick.down;

		public ButtonControl Interact { get; private set; } = Gamepad.current.buttonEast;
		public ButtonControl Jump { get; private set; } = Gamepad.current.buttonSouth;
		public ButtonControl Attack { get; private set; } = Gamepad.current.buttonWest;
		public ButtonControl Extra { get; private set; } = Gamepad.current.buttonNorth;

		public ButtonControl Pause { get; private set; } = Gamepad.current.startButton;
	}
}
