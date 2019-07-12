using System.Collections.Generic;
using System.Drawing;
using DotFeather;
using DotFeather.Router;

namespace Otoge
{
    public class TitleScene : Scene
	{
        public override void OnStart(Router router, Dictionary<string, object> args)
		{
			BackgroundColor = Color.White;
			router.Game.Title = "音ゲー";
			var title = new TextDrawable("音ゲー", new Font(FontFamily.GenericSansSerif, 128), Color.Black);
			title.Location = new Vector(router.Game.Width / 2 - title.RenderedTexture.Size.Width / 2, 96);

			var prompt = new TextDrawable("PRESS SPACE KEY", new Font(FontFamily.GenericSansSerif, 32), Color.Black);
			prompt.Location = new Vector(router.Game.Width / 2 - prompt.RenderedTexture.Size.Width / 2, 500);

			Root.Add(title);
			Root.Add(prompt);
		}

		public override void OnUpdate(Router router, DFEventArgs e)
		{
			if (Input.Keyboard.Space.IsPressed)
			{
				router.ChangeScene<SelectorScene>();
			}
		}
	}
}
