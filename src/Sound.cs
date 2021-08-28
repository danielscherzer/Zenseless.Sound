using NAudio.Wave;
using Zenseless.Patterns;

namespace Zenseless.Sound
{
	/// <summary>
	/// Implements the <see cref="ISound"/> interface for a wave stream. It is implements the <see cref="System.IDisposable"/> interface.
	/// </summary>
	public class Sound : Disposable, ISound
	{
		/// <summary>
		/// Create a new instance.
		/// </summary>
		/// <param name="waveStream">A wave stream that the sound class will take ownership. It will be disposed.</param>
		public Sound(WaveStream waveStream) => _waveStream = waveStream;

		/// <summary>
		/// Rewind the wave provider
		/// </summary>
		public void Rewind() => _waveStream.Position = 0;

		/// <summary>
		/// Read access to a wave provider
		/// </summary>
		public IWaveProvider WaveProvider => _waveStream;

		/// <summary>
		/// Will be called from the default Dispose method. Implementers should dispose all their resources her.
		/// </summary>
		protected override void DisposeResources() => _waveStream.Dispose();

		private readonly WaveStream _waveStream;
	}
}
