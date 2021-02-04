using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct AudioCueKey
{
	public static AudioCueKey Invalid = new AudioCueKey(-1, null);

	internal int Value;
	internal AudioCueSO AudioCue;

	internal AudioCueKey(int value, AudioCueSO audioCue)
	{
		Value = value;
		AudioCue = audioCue;
	}

	public override bool Equals(Object obj)
	{
		return obj is AudioCueKey x && Value == x.Value && AudioCue == x.AudioCue;
	}
	public override int GetHashCode()
	{
		return Value.GetHashCode() ^ AudioCue.GetHashCode();
	}
	public static bool operator ==(AudioCueKey x, AudioCueKey y)
	{
		return x.Value == y.Value && x.AudioCue == y.AudioCue;
	}
	public static bool operator !=(AudioCueKey x, AudioCueKey y)
	{
		return !(x == y);
	}
}
