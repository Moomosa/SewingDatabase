using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.Shared
{
	public class BaseDataCard
	{
		public int ID { get; set; }
		public string MainText { get; set; }
		public char FirstLetter { get; set; }
		public List<KeyValuePair<string, string>>? ExpandableData { get; set; }
		public string? BackgroundColor { get; set; }
		public string? TextColor
		{
			get
			{
				int brightness = CalculateBrightness(BackgroundColor);
				return brightness > 128 ? "#000000" : "#FFFFFF";
			}
		}

		public List<string>? BottomText { get; set; }

		private int CalculateBrightness(string backgroundColor)
		{
			if (backgroundColor.StartsWith("#") && backgroundColor.Length == 7)
			{
				int backgroundR = int.Parse(backgroundColor.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
				int backgroundG = int.Parse(backgroundColor.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
				int backgroundB = int.Parse(backgroundColor.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);

				return (int)(0.299 * backgroundR + 0.587 * backgroundG + 0.114 * backgroundB);
			}
			else if (backgroundColor.StartsWith("rgb(") || backgroundColor.StartsWith("rgba("))
			{
				var colorComponents = backgroundColor
					.Split(new[] { '(', ',', ')', ' '}, StringSplitOptions.RemoveEmptyEntries)
					.Select(component =>
					{
						if (int.TryParse(component, out int result))
							return result;
						return 0;
					})
					.ToArray();

				if (colorComponents.Length == 4)
				{
					int backgroundR = colorComponents[1];
					int backgroundG = colorComponents[2];
					int backgroundB = colorComponents[3];
					return (int)(0.299 * backgroundR + 0.587 * backgroundG + 0.114 * backgroundB);
				}
			}
			return 128;
		}
	}
}
