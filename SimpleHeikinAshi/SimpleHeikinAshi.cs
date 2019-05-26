using System;
using System.Collections.Generic;
using CQG.Operators;

namespace SimpleHeikinAshi
{
    /// <summary>
    /// CQG operator implementing simple Heikin-Ashi bars.
    /// The operators takes 4 inputs Open,High,Low,Close and returns corresponding 4 outputs.
    /// </summary>
    [CQGOperator("SimpleHA(@,@,@,@) : [o,h,l,c]", FillInvalids = false)]
    public class SimpleHeikinAshi : IOperator
    {
        private const int _extraBars = 100;

        private class State : IDeepCloneable
        {
            public double PrevHAOpen;
            public double PrevHAClose;

            public State(double prevHAOpen = double.NaN, double prevHAClose = double.NaN)
            {
                PrevHAOpen = prevHAOpen;
                PrevHAClose = prevHAClose;
            }

            public bool IsValid
            {
                get
                {
                    return (!double.IsNaN(PrevHAOpen) && !double.IsNaN(PrevHAClose));
                }
            }

            /// <summary>
            /// Implements <see cref="IDeepCloneable.DeepClone"/>
            /// </summary>
            public Object DeepClone()
            {
                return new State(PrevHAOpen, PrevHAClose);
            }
        }

        #region IOperator implementation

        /// <summary>
        /// Implements <see cref="IOperator.Initialize"/>
        /// </summary>
        public int Initialize(IDictionary<string, string> i_params, IBarsRebuilder i_barsRebuilder)
        {
            // We need to accumulate enough bars to make the contribution of the first Open value low
            // (the first Open is calculated on input's Open/Close, not on HA Open/Close)
            return _extraBars; 
        }

        /// <summary>
        /// Implements <see cref="IOperator.Calculate"/>
        /// </summary>
        public void Calculate(IList<double>[] i_inputs, double[] io_outputs, ref IDeepCloneable io_state, IList<object> io_primitives)
        {
            const int openIndex = 0;
            const int highIndex = 1;
            const int lowIndex = 2;
            const int closeIndex = 3;

            const int last = _extraBars - 1;

            State state = (State)io_state;
            if (state == null)
            {
                state = new State();
                io_state = state;
            }

            // Calculate HAClose = (O + H + L + C) / 4
            if (double.IsNaN(i_inputs[openIndex][last]) || double.IsNaN(i_inputs[highIndex][last]) ||
                double.IsNaN(i_inputs[lowIndex][last]) || double.IsNaN(i_inputs[closeIndex][last]))
            {
                io_outputs[closeIndex] = double.NaN;
            }
            else
            {
                io_outputs[closeIndex] = (i_inputs[openIndex][last] + i_inputs[highIndex][last] +
                    i_inputs[lowIndex][last] + i_inputs[closeIndex][last]) * 0.25;
            }

            // Calculate HAOpen = (HAOpen(previous bar) + HAClose(previous bar)) / 2;
            // if previous HA bar is unknown, use Open and Close values from the previous input
            if (state.IsValid)
            {
                io_outputs[openIndex] = (state.PrevHAOpen + state.PrevHAClose) * 0.5;
            }
            else if (!double.IsNaN(i_inputs[openIndex][last - 1]) && !double.IsNaN(i_inputs[openIndex][last - 1]))
            {
                io_outputs[openIndex] = (i_inputs[openIndex][last - 1] + i_inputs[openIndex][last - 1]) * 0.5;
            }
            else
            {
                io_outputs[openIndex] = io_outputs[closeIndex];
            }

            // Calculate HAHigh = = Maximum(H, HAOpen, HAClose) and HALow = Minimum(L, HAOpen, HAClose)
            if (double.IsNaN(io_outputs[openIndex]) || double.IsNaN(io_outputs[closeIndex]))
            {
                io_outputs[highIndex] = double.NaN;
                io_outputs[lowIndex] = double.NaN;
            }
            else
            {
                io_outputs[highIndex] = Math.Max(i_inputs[highIndex][last], Math.Max(io_outputs[openIndex], io_outputs[closeIndex]));
                io_outputs[lowIndex] = Math.Min(i_inputs[lowIndex][last], Math.Min(io_outputs[openIndex], io_outputs[closeIndex]));
            }

            // Update the state
            state.PrevHAClose = io_outputs[closeIndex];
            state.PrevHAOpen = io_outputs[openIndex];
        }

        #endregion
    }
}
