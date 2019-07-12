namespace Otoge
{
	/// <summary>
	/// 汎用ヘルパークラスです。
	/// </summary>
	public static class Helper
	{
		/// <summary>
		/// 難易度文字列を生成します。
		/// </summary>
		/// <param name="difficulty"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public static string CreateDifficulty(Difficulty difficulty, double level)
		{
			var d = difficulty.ToString().ToUpperInvariant();
			var lv = (int)level + ((level * 10 - (int)level * 10) >= 7 ? "+" : "");
			return $"{d} {lv}";
		}
	}
}
