using System;
using DotFeather;
using DotFeather.Router;

namespace Otoge
{
    class Game : GameBase
	{
		public Game(int width, int height, string title = null, int refreshRate = 60) : base(width, height, title, refreshRate)
		{
		}

		protected override void OnLoad(object sender, EventArgs e)
		{
			router = new Router(this);
			router.ChangeScene<TitleScene>();
		}

		protected override void OnUpdate(object sender, DFEventArgs e)
		{
			router.Update(e);
		}

		private Router router;
	}
}
