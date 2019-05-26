using System;
using System.Collections.Generic;
using CQG.Operators;

namespace SimpleMA
{
    /// <summary>
    /// Simple Moving Average CQG operator. This implementation uses state (stores the sum of last period - 1 inputs,
    /// for new calculation it's enough to add the last element and divide on the period).
    /// </summary>
    /// <input>Any double-value (e.g. Close Price).</input>
    /// <param name="period">Moving average period.</param>
    [CQGOperator("SimpleMaOp2(@, period)")]
    public class SimpleMaOp2 : IOperator
    {
        /// <summary>
        /// Class for keeping state. Implements <see cref="IDeepCloneable"/> interface.
        /// </summary>
        private class State : IDeepCloneable
        {
            /// <summary>
            /// Member for keeping state value.
            /// </summary>
            public double Sum;

            /// <summary>
            /// Implements <see cref="IDeepCloneable.DeepClone"/>
            /// </summary>
            public Object DeepClone()
            {
                return new State()
                {
                    Sum = Sum
                };
            }
        }

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
            State state = (State)io_state;
            IList<double> inputs = i_inputs[0];

            if (state == null)
            {
                state = new State();

                for (int i = 0; i < inputs.Count; ++i)
                {
                    state.Sum += inputs[i];
                }

                io_state = state;
            }
            else
            {
                state.Sum += inputs[inputs.Count - 1];
            }

            io_outputs[0] = state.Sum / _period;
            state.Sum -= inputs[0];
        }

        #endregion
    }
}