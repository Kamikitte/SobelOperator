using System.Collections.Generic;
using System.Linq;

namespace Recognizer	
{
	internal static class MedianFilterTask
	{
		//Медианный фильтр для уменьшения шума на изображении
		public static double[,] MedianFilter(double[,] original)
		{
			var width = original.GetLength(0);
			var height = original.GetLength(1);
			var result = new double[width, height];
			for (int row = 0; row < width; row++)
			{
				for (int col = 0; col < height; col++)
				{
					result[row, col] = GetMedian(original, row, col);
				}
			}
			return result;
		}

		private static double GetMedian (double[,] original, int row, int col)
		{
			var neghbours = new List<int[]>();
			var width = original.GetLength(0);
			var height = original.GetLength(1);
			//yanderedev
			if (row - 1 >= 0 && col - 1 >= 0)
			{
				neghbours.Add(new int[] { row - 1, col - 1 });
			}
			if (row - 1 >= 0)
			{
				neghbours.Add(new int[] { row - 1, col });
			}
			if (row - 1 >= 0 && col + 1 < height)
			{
				neghbours.Add(new int[] { row - 1, col + 1 });
			}

			if (col - 1 >= 0)
			{
				neghbours.Add(new int[] { row, col - 1 });
			}
			neghbours.Add(new int[] { row, col});
			if (col + 1 < height)
			{
				neghbours.Add(new int[] { row, col + 1 });
			}

			if (row + 1 < width && col - 1 >= 0)
			{
				neghbours.Add(new int[] { row + 1, col - 1 });
			}
			if (row + 1 < width)
			{
				neghbours.Add(new int[] { row + 1, col });
			}
			if (row + 1 < width && col + 1 < height)
			{
				neghbours.Add(new int[] { row + 1, col + 1 });
			}

			var lumas = new List<double>();
			foreach(var pixel in neghbours)
			{
				lumas.Add(original[pixel[0], pixel[1]]);
			}

			int count = lumas.Count();
			double median;

			if (count % 2 == 0)
				median = lumas.Select(x => x).OrderBy(x => x).Skip((count / 2) - 1).Take(2).Average();
			else
				median = lumas.Select(x => x).OrderBy(x => x).ElementAt(count / 2);

			return median;
		}
	}
}