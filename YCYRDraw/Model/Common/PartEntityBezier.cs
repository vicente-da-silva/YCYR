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

using FastBezier;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace YCYR.Model.Common
{

    public class PartEntityBezier : PartEntity
    {
        public Vector2 Control1 { get; set; }
        public Vector2 Control2 { get; set; }

        public int LineCount { get; set; }
        public List<PartEntityLine> Lines { get; set; }

        public float Length { get; protected set; }

        public PartEntityBezier(Vector2 start, Vector2 end, Vector2 control1, Vector2 control2, EntityType type = EntityType.Normal,int linesCount = 30)
        {
            Start = start;
            End = end;
            Control1 = control1;
            Control2 = control2;
            Lines = new List<PartEntityLine>();
            //LineCount = (int)(end - start).Length() / 8;
            LineCount = linesCount;
            EntityType = type;
            CalcCurveLines();
        }

        private void CalcCurveLines()
        {
            Lines.Clear();
            Length = 0;

            List<Vector2> result = CalcCurvePoints();
            for (int i = 0; i < result.Count; i++)
            {
                Vector2 startV = result[i];
                if (i + 1 > result.Count - 1)
                    break;
                Vector2 endV = result[i + 1];
                Lines.Add(new PartEntityLine() { Start = startV, End = endV, EntityType = EntityType });
                //Length += (endV - startV).Length();
            }
            CalcLength();
        }
        private List<Vector2> CalcCurvePoints()
        {
            List<Vector2> result = new List<Vector2>();
            float t;
            for (int i = 0; i < LineCount + 1; i++)
            {
                t = i / (float)LineCount;
                result.Add(Cubic(Start, Control1, Control2, End, t));
            }
            return result;
        }

        private void CalcLength()
        {
            Bezier3 b = new Bezier3(Bezier3.Vector2ToV3D(Start), Bezier3.Vector2ToV3D(Control1), Bezier3.Vector2ToV3D(Control2), Bezier3.Vector2ToV3D(End));
            Length = (float)b.InterpolatedLength;
        }


        public static Vector2 Cubic(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            //https://en.wikipedia.org/wiki/Bézier_curve
            // polynomial form (recursive)
            return (1 - t) * ((1 - t) * (p0 + t * (p1 - p0)) + t * (p1 + t * (p2 - p1))) + t * ((1 - t) * (p1 + t * (p2 - p1)) + t * (p2 + t * (p3 - p2)));
            // Alternative recursive function:
            // return (1 - t)*Quadratic(p0, p1, p2, t) + t*Quadratic(p1, p2, p3, t);
            // explicit form
            // return (1 - t)*(1 - t)*(1 - t)*p0 + 3*(1 - t)*(1 - t)*t*p1 + 3*(1 - t)*t*t*p2 + t*t*t*p3;
        }

        private List<PerpConstructionLines> CalcOffsetPoints(float distance, PerpendicularRotation rotation, EntityType type)
        {
            List<PerpConstructionLines> result = new List<PerpConstructionLines>();
            for (int i = 0; i < Lines.Count; i++)
            {
                PartEntityLine line = Lines[i];
                PerpConstructionLines perpConstructionLines = new PerpConstructionLines(
                    line.End,
                    line.Start,
                    line.End,
                    line.Start,
                    distance,
                    rotation,
                    type
                    );
                result.Add(perpConstructionLines);
            }
            return result;
        }

        public override PartEntityOffset CalcOffset(float distance, PerpendicularRotation rotation, EntityType constructionType, EntityType lineType)
        {
            List<PerpConstructionLines> points = CalcOffsetPoints(distance, rotation, constructionType);
            List<PartEntityLine> result = new List<PartEntityLine>();
            for (int i = 0; i < points.Count; i++)
            {
                PerpConstructionLines point = points[i];
                //perpendicular construction lines from origional line
                PartEntityLine lineStart = new PartEntityLine() { Start = point.StartOfLine, End = point.StartOfLine + point.PerpEndPointStartLine, EntityType = constructionType };
                PartEntityLine lineEnd = new PartEntityLine() { Start = point.EndOfLine, End = point.EndOfLine + point.PerpEndPointEndLine, EntityType = constructionType };
                result.Add(lineStart);
                result.Add(lineEnd);
                //line between construction start line and end line
                result.Add(new PartEntityLine() { Start = point.StartOfLine + point.PerpEndPointStartLine, End = point.EndOfLine + point.PerpEndPointEndLine, EntityType = lineType });
            }
            //project to intersecions or trim to intersection
            List < PartEntityLine > saLines = result.Where(x=> x.EntityType == lineType).ToList();
            for (int i = 0; i < saLines.Count; i++)
            {
                PartEntityLine line = saLines[i];
                if (i + 1 < saLines.Count)
                {
                    PartEntityLine lineNext = saLines[i + 1];
                    Vector2 intersection = Utils.CalcIntersection(line, lineNext);
                    //TODO: check this logic
                    if (intersection != Vector2.Zero)
                        if (!float.IsNaN(intersection.X) && !float.IsNaN(intersection.Y))
                        {
                            line.End = intersection;
                            lineNext.Start = intersection;
                        }
                }
                //System.Diagnostics.Debug.WriteLine("[" + i + "] " + line.ToString());
            }
            return new PartEntityOffset(result);
        }
    }
}
