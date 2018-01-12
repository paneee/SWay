using Google.OrTools.ConstraintSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWay.Helpers
{
    public class Demand : NodeEvaluator2
    {
        public Demand(int[] order_demands)
        {
            this.order_demands_ = order_demands;
        }

        public override long Run(int first_index, int second_index)
        {
            if (first_index < order_demands_.Length)
            {
                return order_demands_[first_index];
            }
            return 0;
        }

        private int[] order_demands_;
    };
}
