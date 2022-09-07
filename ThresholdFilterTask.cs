using System;
using System.Collections.Generic;
using System.Linq;

namespace Recognizer
{
	public static class ThresholdFilterTask
	{		
		public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
		{
			var width = original.GetLength(0);
			var height = original.GetLength(1);
			var result = new double[width, height];
			var lumas = new List<double>();

			for (int row = 0; row < width; row++)
			{
				for (int col = 0; col < height; col++)
				{
					lumas.Add(original[row, col]);
				}
			}

			var maxValues = lumas.OrderByDescending(n => n).ToList();
			int pixelsCount = (int)Math.Floor(whitePixelsFraction * lumas.Count);
			if (pixelsCount == 0)
			{
				return new double[width, height];
			}
			
			double threshold = maxValues[pixelsCount - 1];

			for (int row = 0; row < width; row++)
			{
				for (int col = 0; col < height; col++)
				{
					result[row, col] = (original[row, col] >= threshold) ? 1 : 0;
				}
			}

			return result;
		}
	}
}