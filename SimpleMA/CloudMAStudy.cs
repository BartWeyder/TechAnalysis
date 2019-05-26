using System;
using System.Drawing;
using CQG.Operators;

namespace SimpleMA
{
    /// <summary>
    /// Study definition production factory for moving average with envelope drawn as cloud. 
    /// Study uses <see cref="CloudMaOp"/> operator for calculation.
    /// </summary>
    public class CloudMAStudy : IStudyDefinitionFactory
    {
        /// <summary>
        /// Implements <see cref="IStudyDefinitionFactory.Create"/>
        /// </summary>
        public StudyDefinition Create()
        {
            StudyDefinition study = StudyDefinition.CreateStudyDefinition("Cloud Moving Average", "CMA", "Study Samples");
            study.Overlayed = true;
            study.Id = new Guid("10F881F1-C308-4300-BE9E-797728B4483B");

            // CMA curve
            StudyOutputDefinition outputMA = study.Outputs.Add("CMA", "CloudMAOp(Input, Period, Percent)");
            outputMA.Id = new Guid("482F23AC-840F-46D0-B1E8-E4E150DA7910");

            IStudyCurveDefinition curveMA = study.Curves.Add("CMA", outputMA);

            study.Layout["Display", "", "Color"] = curveMA.AddColorParameter(Color.Blue);
            study.Layout["Display", "", "Weight"] = curveMA.AddWeightParameter();
            study.Layout["Display", "", "Display"] = curveMA.AddCurveTypeParameter();
            study.Layout["", "MarkIt"] = curveMA.AddMarkItParameter();
            StudyParamDefinition offsetParam = study.Layout["", "Offset"] = curveMA.AddDisplayOffsetParameter();
            StudyParamDefinition shareScaleParam = study.Layout["Display", "", "ShareScale"] = curveMA.AddShareScaleParameter();

            // CMATE output for value in the cursor box.
            StudyOutputDefinition outputMATE = study.Outputs.Add("CMATE", outputMA.Expression);
            outputMATE.Id = new Guid("7750059E-7282-49F3-AA0C-3EEF468DFE65");
            outputMATE.OperatorOutput = "high";
            outputMATE.Switches.Add("Percent", "0");

            // CMABE output for value in the cursor box.
            StudyOutputDefinition outputMABE = study.Outputs.Add("CMABE", outputMA.Expression);
            outputMABE.Id = new Guid("C4844BF0-704C-495C-A545-7FED6CBFDED2");
            outputMABE.OperatorOutput = "low";
            outputMABE.Switches.Add("Percent", "0");

            // Cloud.
            IStudyCurveDefinition curveCloud = study.Curves.Add("CMACloud", outputMA.Expression, new[]{ "high", "low" });

            study.Layout["Display", "Cloud", "Color"] = curveCloud.AddColorParameter(Color.Red, 1);
            study.Layout["Display", "Cloud", "Weight"] = curveCloud.AddWeightParameter(3);
            curveCloud.SetCurveType(CurveType.Cloud);
            curveCloud.AddDisplayOffsetParameter(offsetParam);
            curveCloud.AddShareScaleParameter(shareScaleParam);
            curveCloud.AddOutputsSwitch("Percent", "0");

            // Operator parameters
            study.Parameters.AddInt("Period", 21, 1, 1000);
            study.Parameters.AddFloat("Percent", 1, 0, 100);
            StudyInputDefinition input = study.Inputs.Add("Input");
            study.Layout["", "Price"] = study.Parameters.AddInput(input, "Close");

            return study;
        }
    }
}
