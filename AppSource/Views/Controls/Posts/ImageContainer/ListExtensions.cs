using System.Collections.Generic;

namespace OxygenVK.AppSource.Views.Controls.Posts.ImageContainer
{
	public static class ListExtensions
	{
		public static List<T> Sublist<T>(this List<T> list, int begin, int end)
		{
			List<T> objList = new List<T>();
			for(int i = begin; i < end; ++i)
			{
				objList.Add(list[i]);
			}

			return objList;
		}
	}
}
