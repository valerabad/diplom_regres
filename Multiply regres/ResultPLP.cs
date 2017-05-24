using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiply_regres
{
    class ResultPLP
    {
        double p;
        double lambda_opt;
        double p_opt;

        public ResultPLP(double p_, double lambda_opt_, double p_opt_)
        {
            this.p = p_;
            this.lambda_opt = lambda_opt_;
            this.p_opt = p_opt_;
        }
    }
}
