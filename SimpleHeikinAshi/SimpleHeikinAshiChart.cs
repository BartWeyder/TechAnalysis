using System;
using System.Drawing;
using CQG.Operators;

namespace SimpleHeikinAshi
{
    /// <summary>
    /// Simple Heikin-Ashi chart definition production factory. 
    /// Chart will use <see cref="SimpleHeikinAshi"/> operator for bar calculation.
    /// </summary>
    public sealed class SimpleHeikinAshiChart : IStudyDefinitionFactory
    {
        /// <summary>
        /// Implements <see cref="IStudyDefinitionFactory.Create"/>
        /// </summary>
        public StudyDefinition Create()
        {
            StudyDefinition chart = StudyDefinition.CreateChartTypeDefinition("Simple Heikin-Ashi", "SHA");
            chart.Id = new Guid("BD81E8A1-C299-4578-A662-3F1A45855006");

            // Define outputs
            StudyOutputDefinition outputOpen = chart.Outputs.Add("SHAOpen",
                "SimpleHA(Open(Input), High(Input), Low(Input), Close(Input))");
            outputOpen.Id = new Guid("44C370AA-8477-4FE2-B977-44797E8247B5");
            outputOpen.OperatorOutput = "open";

            StudyOutputDefinition outputHigh = chart.Outputs.Add("SHAHigh", outputOpen.Expression);
            outputHigh.Id = new Guid("6EDFD817-3E64-467D-BF4E-E762B970FC95");
            outputHigh.OperatorOutput = "high";

            StudyOutputDefinition outputLow = chart.Outputs.Add("SHALow", outputOpen.Expression);
            outputLow.Id = new Guid("B28D4AB6-950A-418E-8465-7CA339307C03");
            outputLow.OperatorOutput = "low";

            StudyOutputDefinition outputClose = chart.Outputs.Add("SHAClose", outputOpen.Expression);
            outputClose.Id = new Guid("65E5F718-E54F-4CFF-AB2F-3F36C553837A");
            outputClose.OperatorOutput = "close";

            // Define curve
            IStudyCurveDefinition curve = chart.Curves.Add("SHACurve", outputOpen.Expression, new[]{"open", "high", "low", "close"});
            curve.SetCurveType(CurveType.Candle);
            chart.Layout["", "Color"] = curve.AddColorParameter(Color.Black);
            chart.Layout["Up", "Color"] = curve.AddColorParameter(Color.LawnGreen, 1);
            chart.Layout["Down", "Color"] = curve.AddColorParameter(Color.Red, 2);
            chart.Layout["", "MarkIt"] = curve.AddMarkItParameter();

            return chart;
        }
    }
}
