using System;
using System.Collections.Generic;
using System.Linq;
using CQG.Operators;

namespace SimpleMA
{
    /// <summary>
    /// Implementation of Exponential Moving Average operator. Implemented using state, keeping last value
    /// to calculate new one. Keeping only last one.
    /// </summary>
    /// <input>Any double-value (e.g. Close Price).</input>
    /// <param name="period">Moving average period.</param>
    [CQGOperator("ExpMAOp(@, period)", FillInvalids = false)]
    public class ExpMA : IOperator
    {
        /// <summary>
        /// Class to keep Operator state.
        /// Implements <see cref="IDeepCloneable"/> interface.
        /// </summary>
        private class State : IDeepCloneable
        {
            /// <summary>
            /// Previos value of moving average.
            /// </summary>
            public double Avg;

            /// <summary>
            /// The constructor. Using another State for initialization.
            /// </summary>
            /// <param name="i_state">Object to replicate.</param>
            public State(State i_state)
            {
                Avg = i_state.Avg;
            }

            /// <summary>
            /// The constructor. Using Moving average value for creation.
            /// </summary>
            /// <param name="i_avg">Average to save.</param>
            public State (double i_avg = 0.0)
            {
                Avg = i_avg;
            }

            /// <summary>
            /// Implements <see cref="IDeepCloneable.DeepClone"/>
            /// </summary>
            public Object DeepClone()
            {
                return new State(this);
            }
        }

        /// <summary>
        /// Moving average period.
        /// </summary>
        private int _period = 21;

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
            State state = (State)io_state;
            IList<double> inputs = i_inputs[0];

            // Check input for validity
            if (double.IsNaN(inputs.Last()))
            {
                io_outputs[0] = double.NaN;
                io_state = null;
                return;
            }

            // Create new state if current does not exist.
            if (state == null)
            {
                state = new State(SimpleMaOp.SMARawCalculate(inputs, _period));
                // For the first value we calculate Simple MA.
                io_outputs[0] = state.Avg;
                io_state = state;
                return;
            }

            // Calculate value, save state and return result.
            double k = 2.0 / (_period + 1.0);
            double result = state.Avg + k * (inputs.Last() - state.Avg);
            io_outputs[0] = result;
            state.Avg = result;
        }
    }
}
