using NAudio.Wave;
using System;

namespace Zenseless.Sound
{
	/// <summary>
	/// Interface for a sound source
	/// </summary>
	public interface ISound : IDisposable
	{
		/// <summary>
		/// Read access to a wave provider
		/// </summary>
		IWaveProvider WaveProvider { get; }

		/// <summary>
		/// Rewind the wave provider
		/// </summary>
		void Rewind();
	}
}