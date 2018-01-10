using Google.OrTools.ConstraintSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWay.Helpers
{
    public class CityDistance : NodeEvaluator2
    {
        public CityDistance(long[,] distanceMatrix, List<string> cityName)
        {
            this.Matrix = distanceMatrix;
            this.CityName = cityName.ToArray();
        }
        private long[,] Matrix;
        private string[] CityName;
       
        public override long Run(int from_node, int to_node)
        {
            return Matrix[from_node, to_node];
        }
    }
}
