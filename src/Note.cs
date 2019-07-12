namespace Otoge
{
    /// <summary>
    /// ノーツを表現します。
    /// </summary>
    public class Note
    {
		/// <summary>
		/// この <see cref="Note"/> のタイプを取得または設定します。
		/// </summary>
		/// <value></value>
		public NoteType Type { get; set; }

        /// <summary>
        /// ノーツの位置を Tick 単位で取得します。
        /// </summary>
        /// <value>位置。4/4 拍子の 1 小節を 192Tick とした単位。</value>
        public int Tick { get; set; }

        /// <summary>
        /// <see cref="NoteType.Hold"/> などのロングノーツの長さを Tick 単位で取得します。
        /// </summary>
        /// <value>ノーツの長さ。 4/4 拍子の 1 小節を 192Tick とした単位。</value>
        public int Gate { get; set; }

		/// <summary>
		/// レーン番号 (0~3) を取得または設定します。
		/// </summary>
		/// <value></value>
		public int Lane { get; set; }
    }
}
