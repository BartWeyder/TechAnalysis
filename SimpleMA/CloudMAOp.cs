using System;
using System.Collections.Generic;
using CQG.Operators;

namespace SimpleMA
{
    /// <summary>
    /// Cloud Moving Average CQG operator. Stateless MA implementation with additional outputs for top and bottom envelope values.
    /// Used by <see cref="CloudMAStudy"/> to draw moving average envelope as a cloud.
    /// </summary>
    /// <input>Any double value (e.g. Close Price).</input>
    /// <param name="period">Moving average period.</param>
    /// <param name="percent">Fraction for cloud building.</param>
    [CQGOperator("CloudMaOp(@, period, percent) : [curve1, high, low]", FillInvalids = false)]
    public class CloudMaOp : IOperator
    {
        /// <summary>
        /// Moving average period
        /// </summary>
        private int _period = 21;

        /// <summary>
        /// Fraction to be added and subtracted to calculate moving average envelope.
        /// </summary>
        private double _fraction = 0.01;

        #region IOperator implementation

        /// <summary>
        /// Implements <see cref="IOperator.Initialize"/>
        /// </summary
        public int Initialize(IDictionary<string, string> i_params, IBarsRebuilder i_barsRebuilder)
        {
            string value = i_params["period"];
            if (!String.IsNullOrEmpty(value))
            {
                _period = Convert.ToInt32(value);
            }
            value = i_params["percent"];
            if (!String.IsNullOrEmpty(value))
            {
                _fraction = Convert.ToDouble(value) / 100;
            }

            return _period;
        }

        /// <summary>
        /// Implements <see cref="IOperator.Calculate"/>
        /// </summary>
        public void Calculate(IList<double>[] i_inputs, double[] io_outputs, ref IDeepCloneable io_state, IList<object> io_primitives)
        {
            double sum = 0;
            foreach (double elem in i_inputs[0])
            {
                if (double.IsNaN(elem))
                {
                    io_outputs[0] = double.NaN;
                    return;
                }
                sum += elem;
            }
            double currentMA = sum / _period;

            io_outputs[0] = currentMA;                   // curve1
            io_outputs[1] = currentMA * (1 + _fraction); // high
            io_outputs[2] = currentMA * (1 - _fraction); // low
        }

        #endregion
    }
}
