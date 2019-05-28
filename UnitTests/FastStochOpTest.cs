﻿using System;
using System.Collections.Generic;
using CQG.Operators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MStoch;

namespace UnitTests
{
    [TestClass]
    public class FastStochOpTest
    {
        private int period = 10;

        [TestMethod]
        public void TestValidData()
        {
            double[] inputPriceArray = { 1772.47,1650.91,1132.63,1239.86,1163.95,1243.1,1393.31,1440.41,
                1183.68,1642.86,1164.03,1677.41,1298.44,1908.04,1583.32,1634.98,1748.59,1902.42,1054.49,
                1001.74,1603.43,1350.94,1555.54,1514.51,1026.02,1696.33,1072.49,1362.56,1180.84,1946.64,
                1095.36,1431.42,1949.1,1321.78,1044.65,1718.4,1816,1604.94,1525,1213.88,1253.09,1804.82,
                1012.67,1742.72,1334.54,1606.75,1799.05,1456.94,1134.2,1000.98,1908.43,1235.39,1439.98,
                1869.23,1838.59,1971.16,1517.36,1111.27,1067.03,1598.48,1120.73,1716.78,1132.7,1577.82,
                1065.43,1622.42,1906.47,1194.47,1887.62,1886.06,1166.82,1644.11,1459.16,1691.26,1232.4,
                1410.48,1033.5,1085.81,1340.39,1789.31,1283.11,1099.61,1542.98,1535.94,1286.64,1929.84,
                1775.52,1202.67,1808.12,1294.64,1059.72,1380.73,1397.14,1793.05,1286.31,1263.58,1015.51,
                1857.21,1763.59,1772.14,1463.3,1747.41,1365.7,1062.89,1661.61,1973.71,1632.42,1219.04,
                1380.41,1562.58,1403.93,1388.62,1948.44,1023.35,1909.49,1058.51,1671.77,1005.43,1009.22,
                1359.93,1937.37,1073.38,1179.67,1842.66,1153.51,1907.95,1962.64,1375.61,1540.3,1051.76,
                1596.23,1690.13,1502.56,1530.52,1256.02,1774.8,1410.32,1926.18,1403.53,1997.69,1040.09,
                1918.33,1924.49,1668.82,1458.82,1009.6,1073.11,1479.63,1535.55,1183.4 };
            var inputPrice = new List<double>(inputPriceArray);

            double[] inputHhighArray = {1772.47,1651.91,1132.63,1240.86,1164.95,1243.1,1394.31,1440.41,
                1185.68,1642.86,1166.03,1679.41,1298.44,1910.04,1585.32,1634.98,1749.59,1902.42,1055.49,
                1002.74,1605.43,1350.94,1557.54,1516.51,1027.02,1696.33,1073.49,1362.56,1181.84,1947.64,
                1095.36,1433.42,1950.1,1321.78,1046.65,1719.4,1816,1605.94,1526,1214.88,1255.09,1806.82,
                1014.67,1743.72,1334.54,1607.75,1801.05,1457.94,1135.2,1002.98,1909.43,1237.39,1440.98,
                1869.23,1838.59,1971.16,1519.36,1112.27,1067.03,1598.48,1121.73,1718.78,1133.7,1579.82,
                1067.43,1622.42,1907.47,1196.47,1889.62,1886.06,1168.82,1644.11,1460.16,1692.26,1234.4,
                1412.48,1035.5,1087.81,1340.39,1790.31,1285.11,1099.61,1544.98,1535.94,1287.64,1931.84,
                1775.52,1202.67,1810.12,1295.64,1061.72,1381.73,1397.14,1793.05,1288.31,1264.58,1016.51,
                1858.21,1765.59,1773.14,1463.3,1747.41,1365.7,1063.89,1661.61,1975.71,1633.42,1220.04,
                1382.41,1564.58,1403.93,1388.62,1950.44,1024.35,1911.49,1060.51,1673.77,1007.43,1009.22,
                1360.93,1939.37,1075.38,1181.67,1842.66,1153.51,1907.95,1962.64,1375.61,1540.3,1053.76,
                1598.23,1692.13,1502.56,1530.52,1258.02,1776.8,1410.32,1926.18,1405.53,1997.69,1041.09,
                1920.33,1924.49,1668.82,1458.82,1009.6,1074.11,1481.63,1537.55,1185.4};
            var inputHhigh = new List<double>(inputHhighArray);

            double[] inputLlowArray = {1770.47,1649.91,1130.63,1237.86,1161.95,1243.1,1392.31,1440.41,
                1182.68,1641.86,1164.03,1677.41,1296.44,1908.04,1583.32,1633.98,1746.59,1900.42,1053.49,
                1000.74,1601.43,1348.94,1555.54,1514.51,1025.02,1696.33,1070.49,1360.56,1180.84,1945.64,
                1095.36,1431.42,1949.1,1320.78,1044.65,1716.4,1815,1603.94,1525,1212.88,1253.09,1804.82,
                1011.67,1740.72,1333.54,1605.75,1799.05,1456.94,1132.2,998.98,1906.43,1235.39,1438.98,
                1868.23,1838.59,1971.16,1517.36,1111.27,1067.03,1598.48,1119.73,1714.78,1130.7,1575.82,
                1063.43,1622.42,1904.47,1194.47,1885.62,1886.06,1164.82,1644.11,1458.16,1690.26,1231.4,
                1409.48,1033.5,1085.81,1340.39,1789.31,1283.11,1099.61,1542.98,1535.94,1284.64,1928.84,
                1775.52,1201.67,1807.12,1292.64,1057.72,1379.73,1396.14,1792.05,1284.31,1263.58,1015.51,
                1857.21,1763.59,1772.14,1462.3,1746.41,1365.7,1062.89,1659.61,1972.71,1631.42,1217.04,
                1379.41,1560.58,1403.93,1388.62,1946.44,1023.35,1907.49,1056.51,1670.77,1004.43,1009.22,
                1359.93,1935.37,1072.38,1178.67,1841.66,1153.51,1905.95,1961.64,1373.61,1538.3,1050.76,
                1594.23,1690.13,1502.56,1529.52,1255.02,1773.8,1408.32,1925.18,1403.53,1996.69,1039.09,
                1918.33,1924.49,1667.82,1457.82,1009.6,1071.11,1479.63,1534.55,1181.4};
            var inputLlow = new List<double>(inputLlowArray);

            double[] oracle = {79.80649383,6.407305095,99.63555523,26.37691802,99.73265249,56.20434042,
                63.12918057,78.35819895,98.97856597,0.116747417,0.109974706,66.28065545,38.51314198,
                61.01396679,56.97919439,2.803655399,77.14377606,7.957368468,52.01627395,25.89168907,
                99.89161302,7.623940517,44.04847066,99.89190124,32.07938773,0,74.41051411,85.18968469,
                61.87972831,53.05096913,18.69015407,23.02059749,83.95493953,0.12432708,90.8893116,
                40.14148422,73.98455858,99.02282588,55.99823933,15.40967113,0.247573777,99.8901642,
                25.96628041,48.43758581,95.58460102,92.21923225,100,53.32140139,11.55033019,6.99973256,
                58.7802639,5.939411368,71.86466548,7.263336025,56.49519428,0.220329834,85.2964065,
                99.8815222,15.52533055,97.64821572,97.46339036,12.24941946,68.79768731,46.88521871,
                74.38391545,9.099845149,33.07883929,0,6.110124749,35.99629352,99.86786644,32.98185806,
                8.735349691,67.31940646,66.38918619,33.44828953,99.77736714,81.52311384,12.38359588,
                85.13391731,23.43462745,0.228801538,36.95259232,38.82990894,84.1223173,26.15087173,
                27.36044657,0,99.88133381,88.77180491,89.78640085,53.13753412,86.85178593,41.55571378,
                5.622404177,76.67022665,99.79171006,62.39236651,17.10632984,34.78451392,54.74135098,
                37.36114458,35.68392454,97.0125545,0,93.04674703,3.792512054,69.94142963,0.105707128,
                0.506337142,37.57888394,98.61840784,7.288506464,18.74344878,89.65602071,15.94540826,
                96.63935654,100,38.42902393,52.55992631,0.109663552,59.81817783,70.11558538,49.54599289,
                52.61218581,22.50954073,79.40079835,49.52344223,100,40.29722876,100,0.104318798,
                91.72126017,92.36386397,65.69267682,43.78572919,0,6.427552146,47.56955338,53.22895688,
                18.99681929 };

            // Prepare array for output
            double[] result = new double[oracle.Length];
            double[] output = new double[1];

            // Prepare object.
            var kFast = new SOKFast();
            IDeepCloneable state = null;
            var initParams = new Dictionary<string, string> { { "period", "10" } };
            kFast.Initialize(initParams, null);

            // Calculation.
            for (int i = 0; i < result.Length; ++i)
            {
                // Prepare input data chunk.
                var inputPriceChunk = new double[period];
                inputPrice.CopyTo(i, inputPriceChunk, 0, period);

                var inputHighChunk = new double[period];
                inputHhigh.CopyTo(i, inputHighChunk, 0, period);

                var inputLowChunk = new double[period];
                inputLlow.CopyTo(i, inputLowChunk, 0, period);

                // Convert input data chunk to parameter type.
                var input = new List<double>[3];
                input[0] = new List<double>(inputPriceChunk);
                input[1] = new List<double>(inputHighChunk);
                input[2] = new List<double>(inputLowChunk);
                // Calculate and save result.
                kFast.Calculate(input, output, ref state, null);
                result[i] = output[0];
            }
            Assert.IsTrue(ExpMATests.PerElementCompare(result, oracle));
        }

        [TestMethod]
        public void TestInvalidData()
        {
            double[] inputPriceArray = { 1772.47, 1650.91, 1132.63, 1239.86, 1163.95, 1243.1, 1393.31,
                1440.41, 1183.68, 1642.86, 1164.03, 1677.41, 1298.44, 1908.04, double.NaN, double.NaN };
            var inputPrice = new List<double>(inputPriceArray);

            double[] inputHhighArray = {1772.47, 1651.91, 1132.63, 1240.86, 1164.95, 1243.1, 1394.31,
                1440.41, 1185.68, 1642.86, 1166.03, 1679.41, 1298.44, 1910.04, double.NaN, double.NaN };
            var inputHhigh = new List<double>(inputHhighArray);

            double[] inputLlowArray = {1770.47, 1649.91, 1130.63, 1237.86, 1161.95, 1243.1, 1392.31,
                1440.41, 1182.68, 1641.86, 1164.03, 1677.41, 1296.44, 1908.04, double.NaN, double.NaN };
            var inputLlow = new List<double>(inputLlowArray);

            double[] oracle = { 79.80649383, 6.407305095, 99.63555523, 26.37691802, 99.73265249,
                double.NaN, double.NaN };

            // Prepare array for output
            double[] result = new double[oracle.Length];
            double[] output = new double[1];

            // Prepare object.
            var kFast = new SOKFast();
            IDeepCloneable state = null;
            var initParams = new Dictionary<string, string> { { "period", "10" } };
            kFast.Initialize(initParams, null);

            // Calculation.
            for (int i = 0; i < result.Length; ++i)
            {
                // Prepare input data chunk.
                var inputPriceChunk = new double[period];
                inputPrice.CopyTo(i, inputPriceChunk, 0, period);

                var inputHighChunk = new double[period];
                inputHhigh.CopyTo(i, inputHighChunk, 0, period);

                var inputLowChunk = new double[period];
                inputLlow.CopyTo(i, inputLowChunk, 0, period);

                // Convert input data chunk to parameter type.
                var input = new List<double>[3];
                input[0] = new List<double>(inputPriceChunk);
                input[1] = new List<double>(inputHighChunk);
                input[2] = new List<double>(inputLowChunk);
                // Calculate and save result.
                kFast.Calculate(input, output, ref state, null);
                result[i] = output[0];
            }
            Assert.IsTrue(ExpMATests.PerElementCompare(result, oracle));
        }
    }
}
