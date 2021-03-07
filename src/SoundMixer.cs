using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenseless.Patterns;

namespace Zenseless.Sound
{
	/// <summary>
	/// This class allows the playing of multiple sounds at the same time (mixing). 
	/// Sounds can be read from streams.
	/// All sounds are required to have the same sampling frequency.
	/// </summary>
	public class SoundMixer : Disposable
	{
		public event EventHandler<ISound>? SoundEnded;

		/// <summary>
		/// Create a new instance of the sound mixer.
		/// </summary>
		/// <param name="sampleRate">For all input sounds that will be played</param>
		/// <param name="channelCount">Output channel count</param>
		public SoundMixer(int sampleRate = 44100, int channelCount = 2)
		{
			_outputDevice = new WaveOutEvent();
			_mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount))
			{
				ReadFully = true
			};
			_outputDevice.Init(_mixer);
			_outputDevice.Play();
			_mixer.MixerInputEnded += MixerInputEnded;
		}

		public bool IsPlaying(ISound sound) => _inputSounds.ContainsValue(sound);

		/// <summary>
		/// Removes a wave as a mixer input
		/// </summary>
		/// <param name="sound"></param>
		public void Stop(ISound sound)
		{
			foreach (var sampleProvider in _inputSounds.Where(pair => pair.Value == sound).Select(pair => pair.Key).ToList())
			{
				_inputSounds.Remove(sampleProvider);
				_mixer.RemoveMixerInput(sampleProvider);
			}
		}

		/// <summary>
		/// Adds a wave as a mixer input.
		/// </summary>
		/// <param name="sound">input sound</param>
		/// <param name="volume">loudness</param>
		public void Play(ISound sound, float volume = 1f)
		{
			sound.Rewind();
			var sampleChannel = new SampleChannel(sound.WaveStream, false)
			{
				Volume = volume
			};
			var sampleProvider = ConvertChannelCount(sampleChannel);
			_mixer.AddMixerInput(sampleProvider);
			_inputSounds[sampleProvider] = sound;
		}

		/// <summary>
		/// Implements disposable pattern object disposal. Here it disposes the output device
		/// </summary>
		protected override void DisposeResources()
		{
			_outputDevice.Stop();
			_outputDevice.Dispose();
		}

		private readonly IWavePlayer _outputDevice;
		private readonly MixingSampleProvider _mixer;

		/// <summary>
		/// Currently playing waves
		/// </summary>
		private readonly Dictionary<ISampleProvider, ISound> _inputSounds = new();

		/// <summary>
		/// Each sample provider must have the same channel count as the mixer
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private ISampleProvider ConvertChannelCount(ISampleProvider input)
		{
			if (input.WaveFormat.Channels == _mixer.WaveFormat.Channels)
			{
				return input;
			}
			if (input.WaveFormat.Channels == 1 && _mixer.WaveFormat.Channels == 2)
			{
				return new MonoToStereoSampleProvider(input);
			}
			throw new NotImplementedException("Not yet implemented this channel count conversion");
		}

		private void MixerInputEnded(object? sender, SampleProviderEventArgs e)
		{
			if(_inputSounds.TryGetValue(e.SampleProvider, out var wave))
			{
				SoundEnded?.Invoke(this, wave);
				_inputSounds.Remove(e.SampleProvider);
			}
		}
	}
}