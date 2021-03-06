﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;

namespace NumSharp.Extensions
{
    public static partial class NDArrayExtensions
    {
        public static NDArray<int> reshape(this NDArray<int> np, params int[] shape)
        {
            np.Shape = shape;

            return np;
        }

        /// <summary>
        /// Gives a new shape to an array without changing its data.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        public static NDArray<T> ReShape<T>(this NDArray<T> np, params int[] shape)
        {
            var count = shape.Length;
            var idx = FindNegativeIndex(shape);
            if (idx == -1)
                np.Shape = shape;
            else
                np.Shape = CalculateNegativeShape(idx, np.Shape, shape);

            //np.Shape = newShape;

            return np;
        }

        private static int FindNegativeIndex(params int[] shape)
        {
            var count = shape.Length;
            var negOne = false;
            var indexOfNegOne = -1;
            for (int i = 0; i < count; i++)
            {
                if (shape[i] == -1)
                {
                    if (negOne)
                        throw new ArgumentException("Only allowed to pass one shape dimension as -1");

                    negOne = true;
                    indexOfNegOne = i;
                }
            }

            return indexOfNegOne;
        }

        private static IList<int> CalculateNegativeShape(int negativeIndex, IList<int> currentShape, params int[] shapeParams)
        {
            var currentShapeCount = currentShape.Count;
            var shapeParamCount = shapeParams.Length;
            var newShape = new List<int>();
            var curShapeVolume = currentShape.Aggregate((x, y) => x * y);
            if (negativeIndex > -1)
            {
                int x = shapeParams[0];
                int y = 0;
                if (shapeParamCount >= 1)
                    y = shapeParams[1];
                if (shapeParamCount > 2)
                    throw new ArgumentException("We cannot currently handle reshapes of more than 2 dimensions");

                if (negativeIndex == 0 && shapeParamCount == 2)
                {
                    var mod = curShapeVolume % y == 0;
                    if (!mod)
                        throw new ArgumentException($"Wrong Reshape. {curShapeVolume} is not evenly divisible by {y}");
                    else
                    {
                        var a = curShapeVolume / y;
                        var b = y;
                        newShape.Add(a);
                        newShape.Add(b);
                    }
                }
                else if (negativeIndex == 1 && shapeParamCount == 2)
                {
                    var mod = curShapeVolume % x == 0;
                    if (!mod)
                        throw new ArgumentException($"Wrong Reshape. {curShapeVolume} is not evenly divisible by {x}");
                    else
                    {
                        var a = x;
                        var b = curShapeVolume / x;
                        newShape.Add(a);
                        newShape.Add(b);
                    }
                }
            }
            else
                return currentShape;

            return newShape;
        }
    }
}
