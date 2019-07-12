using System.Drawing;
using DotFeather;
using DotFeather.Router;

namespace Otoge
{
    public class PlayScene : Scene
    {
		public override void OnStart(Router router, System.Collections.Generic.Dictionary<string, object> args)
		{
			if (!args?.ContainsKey("score") ?? true)
			{
				Root.Add(new TextDrawable("譜面がありません", new Font(FontFamily.GenericSansSerif, 64)));
				return;
			}
			score = args["score"] as Score;
            router.Game.Title = $"{score.Title} - {score.Artist}  {Helper.CreateDifficulty(score.Difficulty, score.Level)}";
			player.Play(score.Source);
        }

        public override void OnDestroy(Router router)
        {
            player.Dispose();
        }

		public override void OnUpdate(Router router, DFEventArgs e)
        {
			if (Input.Keyboard.Escape.IsPressed)
			{
				router.ChangeScene<SelectorScene>();
				return;
			}
		}

		AudioPlayer player = new AudioPlayer();
        Score score;
    }
}
