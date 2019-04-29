using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotFeather;
using DotFeather.Router;

namespace Otoge
{
	public class SelectorScene : Scene
	{
		public Score CurrentScore => TryGetScore(index);
		public Score PrevScore => TryGetScore(index - 1);
		public Score NextScore => TryGetScore(index + 1);

		/// <summary>
		/// 安全にスコアを取得します。
		/// </summary>
		public Score TryGetScore(int index)
		{
			// ループするように矯正する
			if (index < 0)
			{
				index = scores.Length + index;
			}
			if (scores.Length <= index)
			{
				index = index - scores.Length;
			}

			// それでも存在しえないならnullを
			return 0 <= index && index < scores.Length ? scores[index] : null;
		}

		public override void OnStart(Router router, Dictionary<string, object> args)
		{
            router.Game.Title = "選曲";
			scores = Directory.EnumerateFiles("scores", "*.score", SearchOption.AllDirectories)
				.Select(p => Score.LoadFrom(p))
				.ToArray();
			player.Play(CurrentScore.Source, 0);
			UpdateView(router);
		}

		public override void OnUpdate(Router router, DFEventArgs e)
		{
			var d = Input.Keyboard.D.IsPressed;
            var f = Input.Keyboard.F.IsPressed;
            var j = Input.Keyboard.J.IsPressed;
            var k = Input.Keyboard.K.IsPressed;
			var space = Input.Keyboard.Space.IsPressed;
            var escape = Input.Keyboard.Escape.IsPressed;

			if (d && !prevD || k && !prevK)
			{
				index += d ? -1 : 1;
				if (index < 0) index = scores.Length - 1;
                if (index > scores.Length - 1) index = 0;
				player.Stop();
				// while (player.IsPlaying)
				// 	Thread.Sleep(1);
				player.Play(CurrentScore?.Source, 0);
				Console.WriteLine(CurrentScore?.ToString() ?? "NULL");
				UpdateView(router);
            }
			if (f && !prevF || j && !prevJ)
			{
				// 開始
			}
			if (escape && !prevEscape)
			{
				router.ChangeScene<TitleScene>();
			}

			prevD = d;
			prevF = f;
			prevJ = j;
			prevK = k;
			prevSpace = space;
            prevEscape = escape;
		}

		public void UpdateView(Router router)
		{
			BackgroundColor = Color.Beige;
			Root.Clear();
			var current = CreateCard(CurrentScore, true);
			current.Location = new Vector(router.Game.Width / 2 - 180, router.Game.Height / 2 - 250);
            var prev = CreateCard(PrevScore);
            prev.Location = new Vector(router.Game.Width / 2 - 180 - 360 - 32, router.Game.Height / 2 - 250);
            var next = CreateCard(NextScore);
            next.Location = new Vector(router.Game.Width / 2 - 180 + 360 + 32, router.Game.Height / 2 - 250);
			Root.Add(prev);
            Root.Add(current);
			Root.Add(next);
        }

		public Container CreateCard(Score score, bool isCurrent = false)
		{
            var current = new Container();
			if (score == default)
				return current;
            var g = new Graphic();
			current.Add(g);

			// background
            g.Rect(0, 0, 360, 500, Color.White, 8, isCurrent ? Color.Orange : Color.Black);
			// jacket
			g.Rect(32, 32, 328, 328, Color.Gray);
            // difficulty

            current.Add(new TextDrawable(Helper.CreateDifficulty(score.Difficulty, score.Level), new Font(FontFamily.GenericSansSerif, 12), Color.Black)
			{
				Location = new Vector(32, 332)
			});

            var title = new TextDrawable(score.Title, new Font(FontFamily.GenericSansSerif, 32), Color.Black);
			title.Location = new Vector(180 - title.RenderedTexture.Size.Width / 2, 364);
			current.Add(title);

            var artist = new TextDrawable(score.Artist, new Font(FontFamily.GenericSansSerif, 13), Color.Gray);
            artist.Location = new Vector(180 - artist.RenderedTexture.Size.Width / 2, 428);
            current.Add(artist);

			current.Add(new TextDrawable($"ND: {score.Designer}", new Font(FontFamily.GenericSansSerif, 12), Color.Black)
			{
				Location = new Vector(16, 476)
			});
			
            var bpm = new TextDrawable($"BPM{(int)score.Bpm}", new Font(FontFamily.GenericSansSerif, 12), Color.Black);
			bpm.Location = new Vector(360 - 16 - bpm.RenderedTexture.Size.Width, 476);
			current.Add(bpm);

            return current;
		}

        public override void OnDestroy(Router router)
		{
			player.Dispose();
		}
	 
		Score[] scores;
		int index = 0;
		AudioPlayer player = new AudioPlayer();
		bool prevD = Input.Keyboard.D.IsPressed;
		bool prevF = Input.Keyboard.F.IsPressed;
		bool prevJ = Input.Keyboard.J.IsPressed;
		bool prevK = Input.Keyboard.K.IsPressed;
		bool prevSpace = Input.Keyboard.Space.IsPressed;
        bool prevEscape = Input.Keyboard.Escape.IsPressed;
		CancellationTokenSource cts;
	}
}
