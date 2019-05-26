using System;
using System.Collections.Generic;
using CQG.Operators;

namespace SimpleMA
{
    /// <summary>
    /// Simple Moving Average CQG operator. This implementation doesn't use state (calculates all period each time).
    /// Also this implementation doesn't ask to fill invalids - this means that any input value could be
    /// double.NaN and this situation should be processed correctly.
    /// </summary>
    /// <input>Any double-value (e.g. Close Price).</input>
    /// <param name="period">Moving average period.</param>
    [CQGOperator("SimpleMaOp(@, period)", FillInvalids = false)]
    public class SimpleMaOp : IOperator
    {
        /// <summary>
        /// Moving average period
        /// </summary>
        private int _period = 21;

        #region IOperator implementation

        /// <summary>
        /// Implements <see cref="IOperator.Initialize"/>
        /// </summary>
        public int Initialize(IDictionary<string, string> i_params, IBarsRebuilder i_barsRebuilder)
        {
            string value = i_params["period"];
            if (!String.IsNullOrEmpty(value))
            {
                _period = Convert.ToInt32(value);
            }

            return _period;
        }

        /// <summary>
        /// Implements <see cref="IOperator.Calculate"/>
        /// </summary>
        public void Calculate(IList<double>[] i_inputs, double[] io_outputs, ref IDeepCloneable io_state, IList<object> io_primitives)
        {
            var result = SMARawCalculate(i_inputs[0], _period);
            
            if (double.IsNaN(result))
            {
                io_outputs[0] = double.NaN;
                return;
            }
            
            io_outputs[0] = result;
        }

        /// <summary>
        /// Calculates Simple Moving Average value for specified set of data and periods.
        /// </summary>
        /// <param name="i_inputs">Input data.</param>
        /// <param name="i_periods">Periods of Moving Average.</param>
        /// <returns></returns>
        public static double SMARawCalculate(IEnumerable<double> i_inputs, int i_periods)
        {
            double sum = 0;
            foreach (double elem in i_inputs)
            {
                if (double.IsNaN(elem))
                {
                    return double.NaN;
                }
                sum += elem;
            }
            return sum / i_periods;
        }

        #endregion
    }
}