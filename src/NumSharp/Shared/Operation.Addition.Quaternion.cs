using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;

namespace NumSharp.Shared
{
   internal static partial class Addition
   {
        //start 1 
        internal static Quaternion[] AddQuaternionArrayToQuaternionArray(Quaternion[] np1, Quaternion[]np2)
        {
            return np1.Select((x,idx) => x + np2[idx]).ToArray();
        }
        //end 1
        //start 2
        internal static Quaternion[] AddQuaternionToQuaternionArray(Quaternion[] np1, Quaternion np2)
        {
            return np1.Select((x) => x + np2).ToArray();
        }
        //end 2
   }
}
