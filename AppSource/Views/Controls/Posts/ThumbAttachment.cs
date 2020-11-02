namespace OxygenVK.AppSource.Views.Controls
{
	public class ThumbAttachment
	{
		public double Width { get; set; }

		public double Height { get; set; }

		public double CalcWidth { get; set; }

		public double CalcHeight { get; set; }

		public bool LastColumn { get; set; }

		public bool LastRow { get; set; }

		internal double getRatio()
		{
			return Width / Height;
		}

		internal void SetViewSize(double width, double height, bool lastColumn, bool lastRow)
		{
			CalcWidth = width;
			CalcHeight = height;
			LastColumn = lastColumn;
			LastRow = lastRow;
		}
	}
}
