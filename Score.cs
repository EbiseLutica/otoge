using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DotFeather;

namespace Otoge
{
    public class Score
	{
		public string Title { get; private set; }
		public string Artist { get; private set; }
        public string Designer { get; private set; }
        public double Offset { get; private set; }
		public Difficulty Difficulty { get; private set; }
		public double Level { get; private set; }
		public double Bpm { get; private set; }
		public (int, int) Beat { get; private set; }
		public Texture2D Jacket { get; private set; }
		public IAudioSource Source { get; private set; }

		public Score(string data, string cwd)
		{
			var lines = data.Split("\n")
				.Select(line => Regex.Replace(line, "//.+$", "").Trim())
				.Where(line => !string.IsNullOrWhiteSpace(line));

			// Header
			var header = lines.TakeWhile(l => l != "@start");

			// Actual Data
			var note = lines.SkipWhile(l => l != "@start")
							.Skip(1)
							.TakeWhile(l => l != "@end");


			foreach (var line in header)
			{
				var match = Regex.Match(line, "([a-zA-Z]+): ?(.+)");
				if (match.Success)
				{
					var key = match.Groups[1].Value.ToLowerInvariant().Trim();
					var value = match.Groups[2].Value.Trim();
					switch (key)
					{
						case "title":
							Title = value;
							break;
						case "artist":
							Artist = value;
							break;
                        case "designer":
                            Designer = value;
                            break;
						case "difficulty":
							switch (value.ToLowerInvariant())
							{
								case "0":
								case "easy":
									Difficulty = Difficulty.Easy;
									break;
								case "1":
								case "normal":
									Difficulty = Difficulty.Normal;
									break;
								case "2":
								case "hard":
									Difficulty = Difficulty.Hard;
									break;
								case "3":
								case "extra":
                                    Difficulty = Difficulty.Extra;
									break;
								case "4":
								case "madness":
                                    Difficulty = Difficulty.Madness;
									break;
							}
							break;
						case "level":
							Level = float.Parse(value);
							break;
						case "bpm":
                            Bpm = float.Parse(value);
							break;
                        case "offset":
                            Offset = float.Parse(value);
                            break;
						case "beat":
							var v = value.Split('/');
							Beat = (int.Parse(v[0]), int.Parse(v[1]));
							break;
						case "jacket":
							Jacket = Texture2D.LoadFrom(Path.Combine(cwd, value));
							break;
						case "source":
							var path = Path.Combine(cwd, value);
							Source = Path.GetExtension(path) == ".ogg"
								? new VorbisAudioSource(path) as IAudioSource
								: new WaveAudioSource(path);
							break;
					}
				}
			}
		}

		public static Score LoadFrom(string path)
		{
			return new Score(File.ReadAllText(path), Path.GetDirectoryName(path));
		}
	}
}
