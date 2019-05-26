using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleMA;
using CQG.Operators;

namespace UnitTests
{

    [TestClass]
    public class ExpMATests
    {
        /// <summary>
        /// Period for MA.
        /// </summary>
        private const int period = 3;

        /// <summary>
        /// Tests that valid data calculates properly.
        /// </summary>
        [TestMethod]
        public void TestValidDataCalculation()
        {
            // Test data for input.
            double[] TestInputArray = { 1669.33, 1524.55, 1772.57, 1769.66, 1594.48, 1657.28, 1885.74,
            1077.36, 1749.53, 1464.88, 1373.58, 1244.79, 1890.54, 1408.76, 1461.1, 1292.24, 1216.09, 1824.34, 1748.7,
            1883.96, 1372.84, 1139.41, 1972.78, 1695.46, 1321.77 };
            List<double> TestInput = new List<double>(TestInputArray);

            // Test oracle for valid test.
            double[] TestOracleValid = { 1655.483333, 1712.571667, 1653.525833, 1655.402917,
            1770.571458, 1423.965729, 1586.747865, 1525.813932, 1449.696966, 1347.243483, 1618.891742, 1513.825871,
            1487.462935, 1389.851468, 1302.970734, 1563.655367, 1656.177683, 1770.068842, 1571.454421, 1355.43221,
            1664.106105, 1679.783053, 1500.776526 };

            // Prepare array for output
            double[] result = new double[TestOracleValid.Length];
            double[] output = new double[1];
            
            // Prepare object.
            ExpMA expMA = new ExpMA();
            IDeepCloneable state = null;
            var initParams = new Dictionary<string, string> { { "period", "3" } };
            expMA.Initialize(initParams, null);

            // Calculation.
            for (int i = 0; i < result.Length; ++i)
            {
                // Prepare input data chunk.
                var inputData = new double[period];
                TestInput.CopyTo(i, inputData, 0, period);
                // Convert input data chunk to parameter type.
                var input = new List<double>[1];
                input[0] = new List<double>(inputData);
                // Calculate and save result.
                expMA.Calculate(input, output, ref state, null);
                result[i] = output[0];
            }

            // Check result with test oracle.
            Assert.IsTrue(PerElementCompare(result, TestOracleValid));
        }

        [TestMethod]
        public void TestWithInvalidData()
        {
            // Prepare object.
            ExpMA expMA = new ExpMA();
            IDeepCloneable state = null;
            var initParams = new Dictionary<string, string> { { "period", "3" } };
            expMA.Initialize(initParams, null);

            // Test data for input.
            double[] inputArray = { 1772.57, 1769.66, 1594.48, double.NaN };
            var InputList = new List<double>(inputArray);
            
            // Prepare array for output
            double[] output = new double[1];
            double[] result = new double[2];
            
            // Test oracle for valid test.
            double[] oracle = { 1712.236667, double.NaN };

            // Calculation.
            for (int i = 0; i < result.Length; ++i)
            {
                // Prepare input data chunk.
                var inputData = new double[period];
                InputList.CopyTo(i, inputData, 0, period);
                // Convert input data chunk to parameter type.
                var input = new List<double>[1];
                input[0] = new List<double>(inputData);
                // Calculate and save result.
                expMA.Calculate(input, output, ref state, null);
                result[i] = output[0];
            }

            Assert.IsTrue(PerElementCompare(result, oracle));
        }

        /// <summary>
        /// Used to compare two same by length arrays.
        /// </summary>
        /// <param name="arr1">First array.</param>
        /// <param name="arr2">Second array.</param>
        /// <returns>True if arrays equal (with epsilon error), false otherwise.</returns>
        public static bool PerElementCompare(double[] arr1, double[] arr2)
        {
            for (int i = 0; i < arr1.Length; ++i)
            {
                if (Math.Abs(arr1[i] - arr2[i]) > 0.001)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
