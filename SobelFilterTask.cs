using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
		//Реализация фильтра собеля для кастомно заданных матриц свёртки
		//https://en.wikipedia.org/wiki/Sobel_operator
		public static double[,] SobelFilter(double[,] image, double[,] matrixX)
        {
            var width = image.GetLength(0);
            var height = image.GetLength(1);
			var result = new double[width, height];

			var matrixY = Transpose(matrixX);
			int slip = matrixX.GetLength(0) / 2;

			for (int x = 0 + slip; x < width - slip; x++)
                for (int y = 0 + slip; y < height - slip; y++)
                {
					var sample = GetSubmatrix(image, x, y, matrixX.GetLength(0));

					var gx = Convolve(sample, matrixX);
					var gy = Convolve(sample, matrixY);

					result[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }
            return result;
        }
		//Метод для транспонирования матрицы
		private static double[,] Transpose(double[,] matrix)
		{
			var width = matrix.GetLength(0);
			var height = matrix.GetLength(1);
			double[,] transposedMatrix = new double[height, width];

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					transposedMatrix[j, i] = matrix[i, j];
				}
			}
			return transposedMatrix;
		}
		//Метод для получения кастомных подматриц
		private static double[,] GetSubmatrix(double[,] matrix, int x, int y, int size)
		{
			double[,] result = new double[size, size];
			int slip = size / 2;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					result[i, j] = matrix[i + x - slip, j + y - slip];
				}
			}
			return result;
		}
		//Метод свёртки: получения интенсивности пикселя на основе окружающих пикселей
		private static double Convolve(double[,] sample, double[,] gradient)
		{
			var width = sample.GetLength(0);
			var height = sample.GetLength(1);

			double result = 0;

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					result += sample[i, j] * gradient[i, j];
				}
			}

			return result;
		}
	}

	//тесты
    [TestFixture]
	public class SobelFilterTest
	{
		static IEnumerable<TestCaseData> TestCases()
		{
			yield return new TestCaseData
				(
					new double[,] { { 1 } },
					new double[,] { { 2 } },
					new double[,] { { 2.828427 } }
				);

			yield return new TestCaseData
				(
					new double[,]
					{
						{ 1, 1, 1 },
						{ 1, 1, 0 },
						{ 1, 0, 0 }
					},
					new double[,]
					{
						{ 1 , 2 , 1  },
						{ 0 , 0 , 0  },
						{ -1, -2, -1 }
					},
					new double[,]
					{
						{ 0, 0       , 0 },
						{ 0, 4.242640, 0 },
						{ 0, 0       , 0 }
					}
				);

			yield return new TestCaseData
				(
					new double[,]
					{
						{ 0.9, 0.3, 0.4, 0.2, 0.8, 0.4, 0.1 },
						{ 0.6, 0.9, 0.4, 0.1, 0.3, 0.6, 0.1 },
						{ 0.3, 0.1, 0.4, 0.8, 0 ,  0.1, 0.9 },
						{ 0.8, 0.4, 0.2, 0.2, 0.7, 0.3, 0.9 },
						{ 0.4, 0.9, 0.8, 0.5, 0 ,  0.5, 0.8 },
						{ 0.3, 0.9, 0.8, 0.3, 0.2, 0.5, 0.5 },
					},
					new double[,]
					{
						{ 1, 4, 6 , 4, 1 },
						{ 2, 8, 12, 8, 2 },
						{ 0, 0, 0 , 0, 0 },
						{ 2, 8, 12, 8, 2 },
						{ 1, 4, 6 , 4, 1 },
					},
					new double[,]
					{
						{ 0, 0 , 0        , 0        , 0        , 0 , 0 },
						{ 0, 0 , 0        , 0        , 0        , 0 , 0 },
						{ 0, 0 , 58.409759, 48.659017, 55.169013, 0 , 0 },
						{ 0, 0 , 67.296359, 55.059422, 54.450711, 0 , 0 },
						{ 0, 0 , 0        , 0        , 0        , 0 , 0 },
						{ 0, 0 , 0        , 0        , 0        , 0 , 0 },
					}
				);
		}

		[TestCaseSource(nameof(TestCases))]

		public void Test(double[,] original, double[,] sx, double[,] expected)
		{
			var dimOne = original.GetLength(0);
			var dimTwo = original.GetLength(1);
			var result = SobelFilterTask.SobelFilter(original, sx);
			for (int i = 0; i < dimOne; i++)
			{
				for (int j = 0; j < dimTwo; j++)
				{
					Assert.AreEqual(result[i, j], expected[i, j], 10e-5);
				}
			}
		}
	}
}
