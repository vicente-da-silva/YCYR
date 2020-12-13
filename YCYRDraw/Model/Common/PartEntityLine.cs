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
using System.Collections.Generic;

namespace YCYR.Model.Common
{
    public class PartEntityLine : PartEntity
    {
        public float Length()
        {
            return (float)Utils.PythHypotenuse(Math.Abs(Start.X - End.X), Math.Abs(Start.Y - End.Y));
        }

        private PerpConstructionLines CalcOffsetPoints(float distance, PerpendicularRotation rotation, EntityType type)
        {
            PerpConstructionLines perpConstructionLines = new PerpConstructionLines(
                     End,
                     Start,
                     End,
                     Start,
                     distance,
                     rotation,
                     type
                     );
            return perpConstructionLines;
        }

        public override PartEntityOffset CalcOffset(float distance, PerpendicularRotation rotation, EntityType constructionType, EntityType lineType)
        {
            List<PartEntityLine> result = new List<PartEntityLine>();
            PerpConstructionLines point = CalcOffsetPoints(distance, rotation, constructionType);
            //perpendicular construction lines from origional line
            PartEntityLine lineStart = new PartEntityLine() { Start = point.StartOfLine, End = point.StartOfLine + point.PerpEndPointStartLine, EntityType = constructionType };
            PartEntityLine lineEnd = new PartEntityLine() { Start = point.EndOfLine, End = point.EndOfLine + point.PerpEndPointEndLine, EntityType = constructionType };
            result.Add(lineStart);
            result.Add(lineEnd);
            //line between construction start line and end line
            result.Add(new PartEntityLine() { Start = point.StartOfLine + point.PerpEndPointStartLine, End = point.EndOfLine + point.PerpEndPointEndLine, EntityType = lineType });
            return new PartEntityOffset(result);
        }
    }
}
