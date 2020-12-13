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

using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace YCYR.Model.Common
{
    public class PartEntityOffset
    {
        public List<PartEntityLine> Lines { get; protected set; }

        public PartEntityOffset(List<PartEntityLine> lines)
        {
            this.Lines = lines;
        }

        public static void Intersect(List<PartEntityOffset> offsetlines, EntityType type)
        {
            for(int i = 0; i < offsetlines.Count; i++)
            {
                if( i + 1 < offsetlines.Count)
                {
                    Intersect(offsetlines[i], offsetlines[i + 1], type);
                }
                else
                {
                    Intersect(offsetlines[i], offsetlines[0], type);
                }
            }
        }

        private static void Intersect(PartEntityOffset offsetLine1, PartEntityOffset offsetLine2, EntityType type)
        {
            List<PartEntityLine> firstLinesSA = offsetLine1.Lines.Where(x => x.EntityType != type).ToList();
            List<PartEntityLine> secondLinesSA = offsetLine2.Lines.Where(x => x.EntityType != type).ToList();
            PartEntityLine firstLineSA = firstLinesSA.Last();
            PartEntityLine secondLineSA = secondLinesSA.First();

            Vector2 intersection = Utils.CalcIntersection(firstLineSA, secondLineSA);
            if (intersection != Vector2.Zero)
                if(!float.IsNaN(intersection.X) && !float.IsNaN(intersection.Y))
                {
                    firstLineSA.End = intersection;
                    secondLineSA.Start = intersection;
                }
        }
    }
}
