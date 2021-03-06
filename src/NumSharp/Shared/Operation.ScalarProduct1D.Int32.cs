using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;

namespace NumSharp.Shared
{
   internal static partial class ScalarProduct1D
   {
        //start 1 
        internal static Int32[] MuliplyScalarProd1DInt32(Int32[] np1, Int32[]np2)
        {
            Int32 sum = np1.Select((x,idx) => x * np2[idx] ).Sum();

            return new Int32[]{sum};
        }
        //end 1
   }
}
