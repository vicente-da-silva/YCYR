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

using YCYR.Model.Common;
using System;
using System.Numerics;
using YCYRDraw.Model.Top.Common;

namespace YCYR.Model.Top.Common
{
    public class BodiceMaker : PatternPart
    {
        private float neckLengthRequired;
        private BodiceType bodiceType;
        public PartEntityBezier NeckBezier { get; set; }
        public PartEntityLine GarmentLengthLine { get; set; }
        public BodiceMaker(Vector2 start, Measurements measurements ,float neckLengthRequired, BodiceType bodiceType)
            : base(measurements)
        {
            BuildPart(start);
            this.neckLengthRequired = neckLengthRequired;
            this.bodiceType = bodiceType;
        }
        public override void BuildPart(Vector2 start)
        {
            base.BuildPart(start);
        }

        private BezierCalculator.CalcResult BuildBodiceNeck(float neckDepth, float curvinessFactor)
        {
            Entities.Clear();

            float garmentLengthLessNeckDepth = Measurements.GarmentBodiceLength - neckDepth;
            float neckLength = Measurements.NeckCircumference * Utils.quarter;

            GarmentLengthLine =
            AddLineEntity(LineDirection.Up, Start, garmentLengthLessNeckDepth);
            AddLineEntity(LineDirection.Right, neckLength, EntityType.Construction);
            PartEntityLine endNeckCons2 =
            AddLineEntity(LineDirection.Up, neckDepth, EntityType.Construction);

            BezierCalculator.CalcResult adjustmentsNeck = BezierCalculator.CalcLength(
                GarmentLengthLine.End,
                endNeckCons2.End,
                BezierCalculator.BezierCalculatorControlPointDirection.Right,
                BezierCalculator.BezierCalculatorControlPointDirection.Down,
                curvinessFactor * neckLengthRequired,
                curvinessFactor * neckLengthRequired
               );

            NeckBezier = AddBezierEntity(GarmentLengthLine.End, endNeckCons2.End, adjustmentsNeck.Firstpoint, adjustmentsNeck.SecondPoint);

            return adjustmentsNeck;
        }

        public BezierCalculatorResult CalcNeckDepth(float curvinessFactor)
        {
            int rangeMin = 1;
            int rangeMax = (int)Measurements.GarmentBodiceLength;

            int currentIterationValue = ((rangeMax - rangeMin) / 2) + rangeMin;
            int previousIterationValue = 0;

            for (int i = 1; i < 51; i++)
            {
                BezierCalculator.CalcResult result = BuildBodiceNeck(currentIterationValue, curvinessFactor);
                if (Math.Abs(neckLengthRequired - result.FoundLength) <= 0.5F)
                {
                    //System.Diagnostics.Debug.WriteLine("Bodice " + bodiceType.ToString() + " Neck, solution found for neckDepth = " + currentIterationValue + ", required length " + neckLengthRequired + " foundLength = " + result.FoundLength);
                    return BezierCalculatorResult.SolutionFound;
                }
                
                if (result.FoundLength > neckLengthRequired)
                    rangeMax = currentIterationValue;

                if (result.FoundLength < neckLengthRequired)
                    rangeMin = currentIterationValue;

                currentIterationValue = ((rangeMax - rangeMin) / 2) + rangeMin;
                if (currentIterationValue == previousIterationValue)
                    break;
                previousIterationValue = currentIterationValue;
            }
            throw new SolutionFailureException("Bodice " + bodiceType.ToString() + " solution failure, could not calculate neck line length to match hood");
            //return BezierCalculatorResult.NoSolutionFound;
        }
    }
}
