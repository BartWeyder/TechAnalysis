using System;
using System.Collections.Generic;
using System.Linq;
using CQG.Operators;

namespace MStoch
{
    [CQGOperator("SOKFastOp(@, @, @, period)", FillInvalids = false)]
    public class SOKFast : IOperator
    {
        /// <summary>
        /// Period for Fast stochastics subvalues calculation.
        /// </summary>
        private int _period = 21;

        #region  IOperator implementation

        /// <summary>
        /// Implements <see cref="IOperator.Calculate"/>
        /// </summary>
        public void Calculate(IList<double>[] i_inputs, double[] io_outputs, ref IDeepCloneable io_state, IList<object> io_primitives)
        {
            // Input indexes.
            const int closeIndex = 0;
            const int highIndex = 1;
            const int lowIndex = 2;

            double cost = i_inputs[closeIndex].Last();
            double hhigh = i_inputs[highIndex].Max();
            double llow = i_inputs[lowIndex].Min();
            
            // Skip if value is not valid.
            if (double.IsInfinity(cost) || double.IsNaN(cost) || double.IsInfinity(hhigh) || double.IsNaN(hhigh) || 
                double.IsInfinity(llow) || double.IsNaN(llow))
            {
                io_outputs[0] = double.NaN;
                return;
            }

            // Return calculated %K fast.
            io_outputs[0] = 100 * (cost - llow) / (hhigh - llow);
        }

        /// <summary>
        /// Implements <see cref="IOperator.Initialize"/>
        /// </summary>
        public int Initialize(IDictionary<string, string> i_params, IBarsRebuilder i_barsRebuilder)
        {
            // Get operator parameter.
            string value = i_params["period"];
            if (!String.IsNullOrEmpty(value))
            {
                _period = Convert.ToInt32(value);
            }

            return _period;
        }

        #endregion
    }
}
