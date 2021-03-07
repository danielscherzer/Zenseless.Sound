using NAudio.Wave;
using System.IO;

namespace Zenseless.Sound
{
	public class SoundLoader
	{
		/// <summary>
		/// Auto convert loader that tries different sound formats
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static Sound? FromStream(Stream stream)
		{
			try
			{
				WaveStream readerStream = new WaveFileReader(stream);
				if (readerStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm && readerStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
				{
					readerStream = WaveFormatConversionStream.CreatePcmStream(readerStream);
					readerStream = new BlockAlignReductionStream(readerStream);
				}
				return new Sound(readerStream);
			}
			catch { }
			try
			{
				return new Sound(new Mp3FileReader(stream));
			}
			catch { }
			try
			{
				return new Sound(new AiffFileReader(stream));
			}
			catch
			{
				return null;
			}
		}
	}
}
