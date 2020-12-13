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

namespace YCYR.Model.Top.Common
{
    public class FrontBodicePart : BodicePart
    {
        public FrontBodicePart(Vector2 start, float neckBezierLengthFront, float sleeveBezierFrontLength, Measurements measurements)
            :base(0.35f, neckBezierLengthFront, sleeveBezierFrontLength, BodiceType.Front, measurements)
        {
            BuildPart(start);
        }
    }
    public class BackBodicePart : BodicePart
    {
        public BackBodicePart(Vector2 start, float neckBezierLengthBack, float sleeveBezierBackLength, Measurements measurements)
            :base(0.4f, neckBezierLengthBack, sleeveBezierBackLength, BodiceType.Back, measurements)
        {
            BuildPart(start);
        }
    }
    public enum BodiceType
    {
        Front,
        Back,
    }
    public abstract class BodicePart : PatternPart
    {
        private readonly float curvinessFactor;
        private readonly float neckBezierLength;
        private readonly float sleeveBezierLength;
        private readonly BodiceType bodiceType;
        public BodicePart(float curvinessFactor, float neckBezierLength, float sleeveBezierLength, BodiceType bodiceType, Measurements measurements)
            : base(measurements)
        {
            this.curvinessFactor = curvinessFactor;
            this.neckBezierLength = neckBezierLength;
            this.sleeveBezierLength = sleeveBezierLength;
            this.bodiceType = bodiceType;
        }

        public override void BuildPart(Vector2 start)
        {
            base.BuildPart(start);

            BodiceMaker maker = new BodiceMaker(Start, Measurements, neckBezierLength, bodiceType);
            maker.CalcNeckDepth(curvinessFactor);
            Entities.AddRange(maker.Entities);

            PartEntityLine shoulderLine =
            AddLineEntity(Measurements.ShoulderLength, 270 + Measurements.ShoulderAngle);

            float bodiceSleeveCutoutHeightFromShoulderCircumference = Measurements.ShoulderCirc * 0.44f;

            PartEntityLine endScyeDepthCons1 =
            AddLineEntity(LineDirection.Down, bodiceSleeveCutoutHeightFromShoulderCircumference, EntityType.Construction);
            //1/4 of chest circ - measurement from start (CF) to shoulder end (start of scy depth line)
            float chest = (Measurements.ChestCirc * Utils.quarter) - (endScyeDepthCons1.End.X - start.X);
            PartEntityLine endScyeDepthCons2 =
            AddLineEntity(LineDirection.Right, chest, EntityType.Construction);

            BezierCalculator.CalcResult adjustmentsSleeve = BezierCalculator.Calc(
               sleeveBezierLength,
               shoulderLine.End,
               endScyeDepthCons2.End,
               BezierCalculator.BezierCalculatorControlPointDirection.Down,
               BezierCalculator.BezierCalculatorControlPointDirection.Left);

            if (!adjustmentsSleeve.SolutionFound)
                throw new SolutionFailureException("Bodice " + bodiceType.ToString() + " solution failure, could not calculate sleeve line length to match sleeve");
            else
                System.Diagnostics.Debug.WriteLine("Bodice " + bodiceType.ToString() + " Shoulder solution found, required length " + sleeveBezierLength + " foundLength =" + adjustmentsSleeve.FoundLength);

            //arm
            PartEntityBezier armBezier =
            AddBezierEntity(shoulderLine.End, endScyeDepthCons2.End, adjustmentsSleeve.Firstpoint, adjustmentsSleeve.SecondPoint);

            PartEntityLine remainingLengthLine =
            AddLineEntity(endScyeDepthCons2.End, new Vector2(endScyeDepthCons2.End.X, start.Y));
            PartEntityLine lastLine =
            AddLineEntity(remainingLengthLine.End, start);

            Vector2 textPos = start + Utils.Right(25) + Utils.Up(100);
            AddTextEntity(textPos, bodiceType.ToString());
            AddPartEntityGrainLine(
               textPos + Utils.Up(50),
               textPos + Utils.Up(200),
               "Grain", false);

            //add seam and hem allowance entities
            float sa = Measurements.GarmentSeamAllowance;
            float ha = Measurements.GarmentHemAllowance;

            List<PartEntityOffset> offsetLines = new List<PartEntityOffset>
            {
               maker.GarmentLengthLine.CalcOffset(sa, PerpendicularRotation.Clockwise, EntityType.PerpConstruction, EntityType.SA),
               maker.NeckBezier.CalcOffset(sa, PerpendicularRotation.Clockwise, EntityType.PerpConstruction, EntityType.SA),
               shoulderLine.CalcOffset(sa, PerpendicularRotation.Clockwise, EntityType.PerpConstruction, EntityType.SA),
               armBezier.CalcOffset(sa, PerpendicularRotation.Clockwise, EntityType.PerpConstruction, EntityType.SA),
               remainingLengthLine.CalcOffset(sa, PerpendicularRotation.Clockwise, EntityType.PerpConstruction, EntityType.SA),
               lastLine.CalcOffset(ha, PerpendicularRotation.Clockwise, EntityType.PerpConstruction, EntityType.HA),
            };
            PartEntityOffset.Intersect(offsetLines, EntityType.PerpConstruction);
            offsetLines.ForEach(x => AddLineEntities(x));

            //List<PerpConstructionLines> consLines = AddDefaultSAConstructionLines(sa, PerpendicularRotation.Clockwise, EntityType.ConstructionSA);
            //consLines.ForEach(x => x.AddLinesToPattern(this));
            //AddSALines(consLines);
        }
    }
}
