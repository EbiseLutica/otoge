using System;

namespace Otoge
{
	static class Entry
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			new Game(1280, 720, "Otoge").Run();
		}
	}
}
