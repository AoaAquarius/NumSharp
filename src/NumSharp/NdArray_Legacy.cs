﻿/*
 * NumSharp
 * Copyright (C) 2018 Haiping Chen
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the Apache License 2.0 as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the Apache License 2.0
 * along with this program.  If not, see <http://www.apache.org/licenses/LICENSE-2.0/>.
 */

using NumSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NumSharp
{
    /// <summary>
    /// A powerful N-dimensional array object
    /// Inspired from https://www.numpy.org/devdocs/user/quickstart.html
    /// </summary>
    public partial class NDArray_Legacy<TData>
    {
        public NDArray_Legacy()
        {
            Data = new List<TData>();
        }

        /// <summary>
        /// The number of axes (dimensions) of the array.
        /// </summary>
        public int NDim
        {
            get
            {
                return Regex.Matches(typeof(TData).FullName, @"\.Generic\.|\.NDArray").Count + 1;
            }

        }

        public int Length { get { return Data.Count; } }

        /// <summary>
        /// The total number of elements of the array.
        /// </summary>
        public int Size
        {
            get
            {
                int size = 0;
                for(int d = 0; d < NDim ; d++)
                {
                    if (size == 0) size = 1;
                    size *= DimensionSize(d + 1);
                }

                return size;
            }
        }

        /// <summary>
        /// The dimensions of the array. 
        /// n rows and m columns will be (n, m)
        /// </summary>
        public int Shape { get { return Data.Count; } }

        /// <summary>
        /// 2 dimension shape
        /// </summary>
        public (int, int) Shape2
        {
            get
            {
                int x = DimensionSize(1);
                int y = DimensionSize(2);

                return (x, y);
            }
        }

        private int DimensionSize(int dim)
        {
            int size = 0;

            if (dim == 1)
            {
                return Data.Count;
            }
            else if (dim == 2)
            {
                if (typeof(TData).Name == "List`1")
                {
                    if (typeof(TData).GenericTypeArguments[0] == typeof(Int32))
                    {
                        size = (Data[0] as List<int>).Count;
                    }
                }
                else if (typeof(TData).Name == "NDArray`1")
                {
                    if (typeof(TData).GenericTypeArguments[0] == typeof(double))
                    {
                        size = (Data[0] as NDArray_Legacy<double>).Length;
                    }
                }
            }

            return size;
        }

        public IList<TData> Data { get; set; }

        public TData this[int i]
        {
            get
            {
                return Data[i];
            }

            set
            {
                Data[i] = value;
            }
        }

        public NDArray<TData> this[IEnumerable<int> select]
        {
            get
            {
                int i = 0;
                var array = Data.Where(x => select.Contains(i++)).ToList();
                return new NDArray<TData>().Array(array);
            }
        }

        public NDArray<TData> this[NDArray_Legacy<int> select]
        {
            get
            {
                int i = 0;
                var array = Data.Where(x => select.Data.Contains(i++)).ToList();
                return new NDArray<TData>().Array(array);
            }
        }

        public override string ToString()
        {
            string output = "array([";

            // loop row
            if (NDim > 1)
            {
                for (int r = 0; r < Data.Count; r++)
                {
                    output += (r == 0) ? "[" : ", [";
                    Type type = Data[r].GetType().GenericTypeArguments[0].UnderlyingSystemType;

                    // loop dimension
                    if (type.Equals(typeof(Int32)))
                    {
                        var data = Data[r] as IList<Int32>;

                        for (int d = 0; d < NDim; d++)
                        {
                            output += (d == 0) ? data[d] + "" : ", " + data[d];
                        }
                    }
                    else if (type.Equals(typeof(double)))
                    {
                        var data = Data[r] as IList<double>;

                        for (int d = 0; d < NDim; d++)
                        {
                            output += (d == 0) ? data[d] + "" : ", " + data[d];
                        }
                    }

                    
                    output += "]";
                }
            }
            else
            {
                for (int r = 0; r < Data.Count; r++)
                {
                    output += (r == 0) ? Data[r] + "" : ", " + Data[r];
                }
            }

            output += "])";

            return output;
        }
    }
}
