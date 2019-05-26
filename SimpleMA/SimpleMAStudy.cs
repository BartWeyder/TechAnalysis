using System;
using System.Drawing;
using CQG.Operators;

namespace SimpleMA
{
    /// <summary>
    /// Simple moving average study definition production factory. 
    /// Study will use <see cref="SimpleMaOp"/>, <see cref="SimpleMaOp2"/> 
    /// or <see cref="ExpMA"/> operator for calculation.
    /// </summary>
    public class SimpleMAStudy : IStudyDefinitionFactory
    {
        public StudyDefinition Create()
        {
            StudyDefinition study = StudyDefinition.CreateStudyDefinition("Simple Moving Average", "SMA", "Study Samples");
            study.Overlayed = true;
            study.Id = new Guid("23F8A379-8F4F-469E-8FF2-C033223DAF86");

            // Defining output.
            StudyOutputDefinition outputMA = study.Outputs.Add("SMA", "MAOperator(Input, Period)");
            outputMA.Id = new Guid("DB927CA6-9AD6-4FAD-8DBB-2CE753DB0999");

            // Defining curve and its parameters.
            IStudyCurveDefinition curveMA = study.Curves.Add("SMA", outputMA);

            study.Layout["Display", "", "Color"] = curveMA.AddColorParameter(Color.Blue);
            study.Layout["Display", "", "Weight"] = curveMA.AddWeightParameter();
            study.Layout["Display", "", "Display"] = curveMA.AddCurveTypeParameter();
            study.Layout["", "MarkIt"] = curveMA.AddMarkItParameter();
            study.Layout["", "Offset"] = curveMA.AddDisplayOffsetParameter();
            study.Layout["Display", "", "ShareScale"] = curveMA.AddShareScaleParameter();

            // Defining study input and parameters.
            study.Parameters.AddInt("Period", 21, 1, 1000);
            study.Layout["", "Operator"] = study.Parameters.AddChoice("MAOperator", "SimpleMaOp", 
                new[] { "SimpleMaOp", "SimpleMaOp2", "ExpMAOp" });

            StudyInputDefinition input = study.Inputs.Add("Input");
            study.Layout["", "Price"] = study.Parameters.AddInput(input, "Close");

            return study;
        }
    }
}
