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

using System.Numerics;

namespace YCYR.Model.Common
{
    public enum PerpendicularRotation
    {
        Clockwise,
        AntiClockwise
    }

    public class PerpConstructionLines
    {
        //reference line on which the angle is calculated for the construction lines, may or may not be based on a Normal entity
        public Vector2 StartOfLine { get; set; }
        public Vector2 EndOfLine { get; set; }

        public Vector2 GivenLine1End { get; set; }
        public Vector2 GivenLine2End { get; set; }


        public PartEntity LineAt1 { get; protected set; }
        public PartEntity LineAt2 { get; protected set; }
        public float LinesLength { get; set; }
        public EntityType EntityType { get; set; }
        public PerpendicularRotation Rotation { get; set; }
        public PartEntity PartEntity { get; protected set; } //the type of the reference line, if it is based on a Normal Entity

        public bool UseBezierControlPointsAsRefForLine1 { get; set; }
        public bool UseBezierControlPointsAsRefForLine2 { get; set; }

        public Vector2 PerpEndPointStartLine { get; protected set; }
        public Vector2 PerpEndPointEndLine { get; protected set; }

        public PerpConstructionLines(Vector2 line1End, Vector2 line1Start, Vector2 GivenLine1End, Vector2 GivenLine2End,
            float length, PerpendicularRotation rotation, EntityType type) : this(null, line1End, line1Start, GivenLine1End, GivenLine2End, length, rotation, type) { }

        public PerpConstructionLines(PartEntity part, Vector2 line1End, Vector2 line1Start, Vector2 GivenLine1End, Vector2 GivenLine2End,
            float length, PerpendicularRotation rotation, EntityType type)
        {
            this.PartEntity = part;
            this.EndOfLine = line1End;
            this.StartOfLine = line1Start;
            this.GivenLine1End = GivenLine1End;
            this.GivenLine2End = GivenLine2End;
            this.LinesLength = length;
            this.Rotation = rotation;
            this.EntityType = type;
            this.UseBezierControlPointsAsRefForLine1 = true;
            this.UseBezierControlPointsAsRefForLine2 = true;

            Vector2 given1;
            Vector2 given2;
            if (PartEntity != null && PartEntity is PartEntityBezier)
            {
                if (UseBezierControlPointsAsRefForLine1)
                    GivenLine1End = StartOfLine + ((PartEntityBezier)PartEntity).Control1;
                if (UseBezierControlPointsAsRefForLine2)
                    GivenLine2End = EndOfLine + ((PartEntityBezier)PartEntity).Control2;
            }
            given1 = GivenLine1End - StartOfLine;
            given2 = EndOfLine - GivenLine2End;
            

            if (Rotation == PerpendicularRotation.Clockwise)
                PerpEndPointStartLine = new Vector2(given1.Y, -given1.X);//clockwise
            else
                PerpEndPointStartLine = new Vector2(-given1.Y, given1.X);//anticlockwise

            if (Rotation == PerpendicularRotation.Clockwise)
                PerpEndPointEndLine = new Vector2(given2.Y, -given2.X);//clockwise
            else
                PerpEndPointEndLine = new Vector2(-given2.Y, given2.X);//anticlockwise

            PerpEndPointStartLine = Vector2.Normalize(PerpEndPointStartLine) * LinesLength;
            PerpEndPointEndLine = Vector2.Normalize(PerpEndPointEndLine) * LinesLength;
        }
        public void AddLinesToPattern(PatternPart part)
        {
            LineAt1 =
            part.AddLineEntity(StartOfLine, StartOfLine + PerpEndPointStartLine, EntityType);

            LineAt2 =
            part.AddLineEntity(EndOfLine, EndOfLine + PerpEndPointEndLine, EntityType);
        }
    }
}
