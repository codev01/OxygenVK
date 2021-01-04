using System;
using System.Collections.Generic;

namespace OxygenVK.AppSource
{
	public class WindowNamesHelper
	{
		public static int AuthorizationPageWindowID = 0;
		public static bool AuthorizationPageWindowOpened;

		private readonly string NewWindowName;
		private static List<WindowName> WindowNames { get; set; } = new List<WindowName>();

		private class WindowName
		{
			public string Name { get; set; }
			public int Index { get; set; }
			public int NumberOfActiveWindows { get; set; } = 0;
		}

		public WindowNamesHelper(string newWindowName)
		{
			NewWindowName = newWindowName;
		}

		public void RemoveNameWindow()
		{
			for (int i = 0; i < WindowNames.Count; i++)
			{
				int indexOf = NewWindowName.IndexOf(".");
				if (indexOf != -1)
				{
					if (--WindowNames[i].NumberOfActiveWindows == 0)
					{
						WindowNames.Remove(WindowNames[i]);
						continue;
					}

					if (WindowNames[i].Index == Convert.ToInt32(NewWindowName.Substring(0, indexOf)))
					{
						if (--WindowNames[i].Index == 0)
						{
							WindowNames.Remove(WindowNames[i]);
						}
					}
				}
			}
		}

		public string GetNameWindow()
		{
			if (WindowNames.Count != 0)
			{
				foreach (WindowName item in WindowNames)
				{
					if (item.Name == NewWindowName)
					{
						item.NumberOfActiveWindows++;
						return NewWindowName.Insert(0, ++item.Index + ". ");
					}
				}
			}

			WindowNames.Add(new WindowName
			{
				Index = 1,
				Name = NewWindowName
			});
			return NewWindowName;
		}
	}
}
