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
using System.Collections.Generic;
using System.Numerics;
using YCYRDraw.Model.Top.Common;

namespace YCYR.Model.Top.Common
{
    public class SleevePart : PatternPart
    {
        public float FrontArmBezierLength;
        public float BackArmBezierLength;
        public SleevePart(Vector2 start, Measurements measurements)
            : base(measurements)
        {
            BuildPart(start);
        }
                
        public override void BuildPart(Vector2 start)
        {
            base.BuildPart(start);

            SleeveMaker calcResult = new SleeveMaker(start, Measurements);
            calcResult.CalcSleeveForShoulderCirc();

            System.Diagnostics.Debug.WriteLine("SleeveMaker [Before Adjustment] Sleeve Length=" + calcResult.SleeveHeightCalculated + " Arm Length=" + Measurements.ArmLength);

            SleeveMaker calcResultAdjusted = new SleeveMaker(start, Measurements);
            calcResultAdjusted.BuildSleeve(calcResult.ScyeHeightCalculated, calcResult.SleeveHeightCalculated - Measurements.ArmLength);
            Entities.AddRange(calcResultAdjusted.Entities);
            FrontArmBezierLength = calcResultAdjusted.FrontBezierLengthCalculated;
            BackArmBezierLength = calcResultAdjusted.BackBezierLengthCalculated;
                        
            System.Diagnostics.Debug.WriteLine("SleeveMaker [After Adjustment] Sleeve Length=" + calcResultAdjusted.SleeveHeightCalculated + " Arm Length=" + Measurements.ArmLength);

            Vector2 textPos = start + Utils.Right(15) + Utils.Up(100);
            AddTextEntity(textPos, "Sleeve");
            AddPartEntityGrainLine(
               textPos + Utils.Up(50) + Utils.Right(0),
               textPos + Utils.Up(200) + Utils.Right(0),
               "Grain", false);
            
            //add seam and hem allowance entities
            float sa = Measurements.GarmentSeamAllowance;
            float ha = Measurements.GarmentHemAllowance;

            List<PartEntityOffset> offsetLines = new List<PartEntityOffset>
            {
                calcResultAdjusted.SleeveLengthFrontBezier.CalcOffset(sa, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.SA),
                calcResultAdjusted.FrontBezier1.CalcOffset(sa, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.SA),
                calcResultAdjusted.FrontBezier2.CalcOffset(sa, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.SA),
                calcResultAdjusted.BackBezier1.CalcOffset(sa, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.SA),
                calcResultAdjusted.BackBezier2.CalcOffset(sa, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.SA),
                calcResultAdjusted.SleeveLengthBackBezier.CalcOffset(sa, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.SA),
                calcResultAdjusted.WristLine.CalcOffset(ha, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.HA),
            };
            PartEntityOffset.Intersect(offsetLines, EntityType.PerpConstruction);
            offsetLines.ForEach(x => AddLineEntities(x));
        }
    }
}
