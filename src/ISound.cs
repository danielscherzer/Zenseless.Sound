using NAudio.Wave;
using System;

namespace Zenseless.Sound
{
	public interface ISound : IDisposable
	{
		IWaveProvider WaveStream { get; }

		void Rewind();
	}
}