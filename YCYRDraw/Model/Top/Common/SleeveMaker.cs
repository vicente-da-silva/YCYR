// *************************************************************************
// YCYR
// Open Source Clothing Pattern Creation
// Copyright (C) 2020  Vicente Da Silva
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/
// *************************************************************************

using System;
using System.Numerics;
using YCYR.Model;
using YCYR.Model.Common;

namespace YCYRDraw.Model.Top.Common
{
    public enum BezierCalculatorResult
    {
        NoSolutionFound,
        SolutionFound
    }
    public class SleeveMaker : PatternPart
    {
        public float FrontBezierLengthCalculated { get; set; }
        public float BackBezierLengthCalculated { get; set; }

        public float ScyeHeightCalculated { get; set; }
        public float SleeveHeightCalculated { get; set; }

        public PartEntityBezier FrontBezier1 { get; set; }
        public PartEntityBezier FrontBezier2 { get; set; }
        public PartEntityBezier BackBezier1 { get; set; }
        public PartEntityBezier BackBezier2 { get; set; }
        public PartEntityLine WristLine { get; set; }
        public PartEntityBezier SleeveLengthBackBezier { get; set; }
        public PartEntityBezier SleeveLengthFrontBezier{ get; set; }

        public SleeveMaker(Vector2 start, Measurements measurements)
            : base(measurements)
        {
            BuildPart(start);
        }
        public override void BuildPart(Vector2 start)
        {
            base.BuildPart(start);
        }

        public BezierCalculatorResult CalcSleeveForShoulderCirc()
        {
            int rangeMin = 1;
            int rangeMax = 200;
            int currentIterationValue = ((rangeMax - rangeMin) / 2) + rangeMin;
            for (int i = 1; i < 51; i++)
            {
                BuildSleeve(currentIterationValue);

                float shoulderCircCalcuated = FrontBezierLengthCalculated + BackBezierLengthCalculated;

                if (Math.Abs(Measurements.ShoulderCirc - shoulderCircCalcuated) <= 1F)
                {
                    System.Diagnostics.Debug.WriteLine("Sleeve Scye solution found, requiredLength=" + Measurements.ShoulderCirc + " foundLength=" + shoulderCircCalcuated + " i=" + i);
                    return BezierCalculatorResult.SolutionFound;
                }

                if (shoulderCircCalcuated > Measurements.ShoulderCirc)
                    rangeMax = currentIterationValue;

                if (shoulderCircCalcuated < Measurements.ShoulderCirc)
                    rangeMin = currentIterationValue;

                currentIterationValue = ((rangeMax - rangeMin) / 2) + rangeMin;
            }
            //System.Diagnostics.Debug.WriteLine("Warning - Sleeve BezierCalculator, no soltion found after 50 iterations");
            //return BezierCalculatorResult.NoSolutionFound;
            throw new SolutionFailureException("Sleeve solution failure, could not calculate sleeve line length to match shoulder circumference");
        }

        public void BuildSleeve(float scpPeakLength, float adjustForLength = 0)
        {
            Entities.Clear();

            float frontStartCurvatureAdjustment = 60;
            float backEndCurvatureAdjustment = 40;
            float frontScyBaseLineLength = Measurements.BicepCirc * Utils.half;
            float backScyBaseLineLength = Measurements.BicepCirc * Utils.half;
            float forearmDeltaLength = (Measurements.ForeArmCirc - Measurements.WristCirc) * Utils.half;
            float scylineDeltaLength = (Measurements.BicepCirc - Measurements.ForeArmCirc) * Utils.half;

            PartEntityLine writstLine =
                AddLineEntity(LineDirection.Right, Start, Measurements.WristCirc);
                AddLineEntity(LineDirection.Up, Measurements.WristToForeArm - adjustForLength, EntityType.Construction);
                AddLineEntity(LineDirection.Right, forearmDeltaLength, EntityType.Construction);
                AddLineEntity(LineDirection.Up, Measurements.ForeArmToBicep + Measurements.BicepToUpperBicep, EntityType.Construction);
            PartEntityLine scyRightSideLine =
                AddLineEntity(LineDirection.Right, scylineDeltaLength, EntityType.Construction);

            PartEntityLine frontScyBaseLine =
                AddLineEntity(LineDirection.Left, frontScyBaseLineLength, EntityType.Construction);
            PartEntityLine scyLeftSideLine =
                AddLineEntity(LineDirection.Left, backScyBaseLineLength, EntityType.Construction);

                AddLineEntity(LineDirection.Right, scylineDeltaLength, EntityType.Construction);
                AddLineEntity(LineDirection.Down, Measurements.ForeArmToBicep + Measurements.BicepToUpperBicep, EntityType.Construction);
                AddLineEntity(LineDirection.Right, forearmDeltaLength, EntityType.Construction);
                AddLineEntity(LineDirection.Down, Measurements.WristToForeArm, EntityType.Construction);

            PartEntityBezier sleeveLengthFrontBezier =
               AddBezierEntity(writstLine.End, scyRightSideLine.End, Utils.Up(40) + Utils.Right(10), Utils.Down(40));

            PartEntityLine scyPeakLine =
                AddLineEntity(LineDirection.Up, frontScyBaseLine.End, scpPeakLength, EntityType.Construction);

                AddLineEntity(LineDirection.Right, scyPeakLine.End, frontScyBaseLineLength * Utils.threeFifths, EntityType.Construction);
            PartEntityLine frontBezierEndLine1 =
                AddLineEntity(LineDirection.Down, scpPeakLength * Utils.threeFifths, EntityType.Construction);

                AddLineEntity(LineDirection.Left, scyPeakLine.End, backScyBaseLineLength * Utils.threeFifths, EntityType.Construction);
            PartEntityLine backBezierEndLine1 =
                AddLineEntity(LineDirection.Down, scpPeakLength * Utils.threeFifths, EntityType.Construction);

            PartEntityBezier frontBezier1 =
               AddBezierEntity(scyRightSideLine.End, frontBezierEndLine1.End, Utils.Left(frontStartCurvatureAdjustment), Utils.Down(20) + Utils.Right(10));

            PartEntityBezier frontBezier2 =
                AddBezierEntity(frontBezier1.End, scyPeakLine.End, Utils.Up(20) + Utils.Left(5), Utils.Right(80));

            PartEntityBezier backBezier1 =
               AddBezierEntity(backBezierEndLine1.End, Utils.Left(80), Utils.Up(20) + Utils.Right(15));

            PartEntityBezier backBezier2 =
                AddBezierEntity(scyLeftSideLine.End, Utils.Down(20) + Utils.Left(15), Utils.Right(backEndCurvatureAdjustment));

            PartEntityBezier sleeveLengthBackBezier =
               AddBezierEntity(scyLeftSideLine.End, Start, Utils.Down(40), Utils.Up(40) + Utils.Left(10));

            FrontBezierLengthCalculated = frontBezier1.Length + frontBezier2.Length;
            BackBezierLengthCalculated = backBezier1.Length + backBezier2.Length;
            FrontBezier1 = frontBezier1;
            FrontBezier2 = frontBezier2;
            BackBezier1 = backBezier1;
            BackBezier2 = backBezier2;
            ScyeHeightCalculated = scpPeakLength;
            WristLine = writstLine;
            SleeveLengthFrontBezier = sleeveLengthFrontBezier;
            SleeveLengthBackBezier = sleeveLengthBackBezier;
            SleeveHeightCalculated = Math.Abs(Start.Y - scyPeakLine.End.Y);
        }
    }
}
