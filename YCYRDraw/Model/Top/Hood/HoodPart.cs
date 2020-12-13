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
    public class HoodPart : PatternPart
    {
        public float HoodPathLengthForCutout { get; private set; }
        public float HoodNeckBackLength { get; private set; }
        public float HoodNeckFrontLength { get; private set; }

        public HoodPart(Vector2 start, Measurements measurements)
            : base(measurements)
        {
            BuildPart(start);
        }

        public override void BuildPart(Vector2 start)
        {
            base.BuildPart(start);

            float garmentHoodBaseStep = 50f;// (Measurements.GarmentBodiceFrontNeckDepth - Measurements.GarmentBodiceBackNeckDepth) * Utils.half;
            float hoodHeight = (Utils.half * Measurements.ShoulderToShoulderOverHead) + (Utils.twoThirds * (garmentHoodBaseStep));
            float hoodHeightLessZipEdge = hoodHeight - Measurements.GarmentHoodZipEdge;
            float hoodInsertWidth = Measurements.GarmentHoodInsertWidth * Utils.half;
            float hoodWidth = Measurements.GarmentHoodLenghtFromTemple + (Utils.half * Measurements.CircTempleToTemple) - hoodInsertWidth;
            float hoodHeadCurveRadius = Measurements.HeadBackCurveRadius;

            float fullHoodWidth = hoodWidth + Measurements.GarmentHoodSetBackFromZip;
            float frontBaseLength = (fullHoodWidth + hoodInsertWidth) * Utils.threeFifths;
            float backBaseLength = fullHoodWidth - frontBaseLength;
            float backBezierVerticalPoint = Utils.twoThirds * garmentHoodBaseStep;

            PartEntityLine hoodZipEdge =
                AddLineEntity(LineDirection.Up, start, Measurements.GarmentHoodZipEdge);
            PartEntityLine hoodSetBackUp1 =
            AddLineEntity(LineDirection.Left, Measurements.GarmentHoodSetBackFromZip, EntityType.Construction);
            PartEntityLine hoodSetBackUp2 =
                AddLineEntity(LineDirection.Up, hoodHeightLessZipEdge * Utils.third, EntityType.Construction);

            //PartEntity zipBezier;
            //if (Measurements.GarmentHoodSetBackFromZip == 0)
            //{
            //    zipBezier =
            //        AddLineEntity(hoodZipEdge.End, hoodSetBackUp2.End);
            //}
            //else
            //{
            //    zipBezier =
            //        AddBezierEntity(hoodZipEdge.End, hoodSetBackUp2.End,
            //        Measurements.GarmentHoodSetBackFromZip > 0 ? Utils.Left(40) : Utils.Left(0),
            //        Measurements.GarmentHoodSetBackFromZip > 0 ? Utils.Down(80) : Utils.Left(0));
            //}

            PartEntityBezier zipBezier =
                    AddBezierEntity(hoodZipEdge.End, hoodSetBackUp2.End,
                    Measurements.GarmentHoodSetBackFromZip > 0 ? Utils.Left(40) : Utils.Left(0),
                    Measurements.GarmentHoodSetBackFromZip > 0 ? Utils.Down(80) : Utils.Left(0));
            
            PartEntityLine hoodHeightLessZipEdgeLine =
                AddLineEntity(LineDirection.Up, zipBezier.End, hoodHeightLessZipEdge * Utils.twoThirds);
            PartEntityLine hoodTopToHeadCurveStart =
                AddLineEntity(LineDirection.Left, hoodWidth - hoodHeadCurveRadius);
            AddLineEntity(LineDirection.Left, hoodHeadCurveRadius, EntityType.Construction);
            PartEntityLine hoodTopToHeadCurveEnd =
                AddLineEntity(LineDirection.Down, hoodHeadCurveRadius, EntityType.Construction);
            PartEntityBezier headBezier =
                AddBezierEntity(hoodTopToHeadCurveStart.End, hoodTopToHeadCurveEnd.End, Utils.Left(100), Utils.Up(100));
            PartEntityLine hoodToEndWidth =
                AddLineEntity(LineDirection.Down, hoodTopToHeadCurveEnd.End, (start.Y - hoodTopToHeadCurveEnd.End.Y) - garmentHoodBaseStep);
            AddLineEntity(LineDirection.Right, backBaseLength, EntityType.Construction);
            PartEntityLine baseNeckLineDown =
                AddLineEntity(LineDirection.Down, garmentHoodBaseStep, EntityType.Construction);
            AddLineEntity(LineDirection.Right, frontBaseLength, EntityType.Construction);
            PartEntityBezier neckBezierBack =
                AddBezierEntity(hoodToEndWidth.End, baseNeckLineDown.End + Utils.Up(backBezierVerticalPoint), Utils.Right(20), Utils.Left(20) + Utils.Up(20));
            PartEntityBezier neckBezierFront =
                AddBezierEntity(neckBezierBack.End, start, Utils.Right(40) + Utils.Down(40), Utils.Left(50));

            Vector2 textPos = start + Utils.Left(fullHoodWidth) + Utils.Right(25) + Utils.Up(75);
            AddTextEntity(textPos, "Hood");
            AddPartEntityGrainLine(
                textPos + Utils.Up(0) + Utils.Right(75),
                textPos + Utils.Up(150) + Utils.Right(75),
                "Grain", false);

            HoodPathLengthForCutout += hoodTopToHeadCurveStart.Length();
            HoodPathLengthForCutout += headBezier.Length; ;
            HoodPathLengthForCutout += hoodToEndWidth.Length();
            HoodNeckFrontLength = neckBezierFront.Length;
            HoodNeckBackLength = neckBezierBack.Length + hoodInsertWidth;

            //add seam and hem allowance entities
            float sa = Measurements.GarmentSeamAllowance;
            float ha = Measurements.GarmentHemAllowance;

            List<PartEntityOffset> offsetLines = new List<PartEntityOffset>
            {
                hoodZipEdge.CalcOffset(ha, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.HA),
                zipBezier.CalcOffset(ha, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.HA),
                hoodHeightLessZipEdgeLine.CalcOffset(ha, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.HA),
                hoodTopToHeadCurveStart.CalcOffset(sa, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.SA),
                headBezier.CalcOffset(sa, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.SA),
                hoodToEndWidth.CalcOffset(sa, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.SA),
                neckBezierBack.CalcOffset(sa, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.SA),
                neckBezierFront.CalcOffset(sa, PerpendicularRotation.AntiClockwise, EntityType.PerpConstruction, EntityType.SA),
            };
            PartEntityOffset.Intersect(offsetLines, EntityType.PerpConstruction);
            offsetLines.ForEach(x => AddLineEntities(x));
        }
    }
}
