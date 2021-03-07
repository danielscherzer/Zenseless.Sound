using NAudio.Wave;
using Zenseless.Patterns;

namespace Zenseless.Sound
{
	public class Sound : Disposable, ISound
	{
		public Sound(WaveStream waveStream) => _waveStream = waveStream;

		public void Rewind() => _waveStream.Position = 0;

		public IWaveProvider WaveStream => _waveStream;

		protected override void DisposeResources() => _waveStream.Dispose();

		private readonly WaveStream _waveStream;
	}
}
