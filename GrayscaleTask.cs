namespace Recognizer
{
	public static class GrayscaleTask
	{
		//Перевод изображения в ЧБ согласно относительной яркости
		//https://en.wikipedia.org/wiki/Relative_luminance
		public static double[,] ToGrayscale(Pixel[,] original)
		{
			var width = original.GetLength(0);
			var height = original.GetLength(1);
			var result = new double[width, height];
			for (int row = 0; row < width; row++)
			{
				for (int column = 0; column < height; column++)
				{
					double luma = ((original[row, column].R * 0.299) + 
						(original[row, column].G * 0.587) + 
						(original[row, column].B * 0.114)) / 255;
					result[row, column] = luma;
				}
			}
			return result;
		}
	}
}