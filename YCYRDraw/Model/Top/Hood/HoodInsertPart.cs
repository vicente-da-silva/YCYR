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

namespace YCYR.Model.Top.Hood
{
    public class HoodInsertPart : PatternPart
    {
        float hoodLengthForCutout;
        public HoodInsertPart(Vector2 start, float hoodLengthForCutout, Measurements measurements)
            : base(measurements)
        {
            this.hoodLengthForCutout = hoodLengthForCutout;
            BuildPart(start);
        }

        public override void BuildPart(Vector2 start)
        {
            base.BuildPart(start);

            PartEntityLine firstLine =
            AddLineEntity(LineDirection.Right, start, Measurements.GarmentHoodInsertWidth);
            PartEntityLine secondLine =
            AddLineEntity(LineDirection.Up, hoodLengthForCutout);
            PartEntityLine thirdLine =
            AddLineEntity(LineDirection.Left, Measurements.GarmentHoodInsertWidth);
            PartEntityLine fourthLine =
            AddLineEntity(LineDirection.Down, hoodLengthForCutout);

            Vector2 textPos = start + Utils.Right(50) + Utils.Up(50);
            AddTextEntity(textPos, "Hood Insert", true);
            AddPartEntityGrainLine(
                textPos + Utils.Up(200),
                textPos + Utils.Up(350),
                "Grain", true);

            //add seam and hem allowance entities
            float sa = Measurements.GarmentSeamAllowance;
            float ha = Measurements.GarmentHemAllowance;

            List<PartEntityOffset> offsetLines = new List<PartEntityOffset>
            {
                firstLine.CalcOffset( sa, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.SA),
                secondLine.CalcOffset(sa, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.SA),
                thirdLine.CalcOffset( ha, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.HA),
                fourthLine.CalcOffset(sa, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.SA)
            };
            PartEntityOffset.Intersect(offsetLines, EntityType.PerpConstruction);
            offsetLines.ForEach(x => AddLineEntities(x));
        }
    }
}
