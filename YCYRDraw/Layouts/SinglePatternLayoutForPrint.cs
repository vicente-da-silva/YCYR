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

namespace YCYR.Layouts
{
    public class SinglePatternLayoutForPrint
    {
        public static List<Vector2> CalcLayout(Pattern pattern, OriginPositionEnum partOriginPosition, Vector2 padding)
        {
            List<Vector2> result = new List<Vector2>();
            Vector2 locationPos = new Vector2();
            for (int i = 0; i < pattern.Parts.Count; i++)
            {
                PatternPart part = pattern.Parts[i];
                PartExtents extents = PartExtents.CalcPartExtents(part);
                Vector2 pageSize = new Vector2(extents.Width,extents.Height);
                Vector2 originOffset = PartExtents.CalcOffsetForOrigin(part, partOriginPosition);

                switch (partOriginPosition)
                {
                    default:
                    case OriginPositionEnum.LeftTop:
                        locationPos += new Vector2(padding.X, padding.Y);
                        locationPos = PlacePart(locationPos, originOffset, result);
                        locationPos -= new Vector2(padding.X, padding.Y);
                        break;

                    case OriginPositionEnum.LeftBottom:
                        locationPos += new Vector2(padding.X, pageSize.Y - padding.Y);
                        locationPos = PlacePart(locationPos, originOffset, result);
                        locationPos -= new Vector2(padding.X, pageSize.Y - padding.Y);
                        break;

                    case OriginPositionEnum.Center:
                        locationPos += new Vector2((pageSize.X + padding.X) / 2, (pageSize.Y + padding.Y) / 2);
                        locationPos = PlacePart(locationPos, originOffset, result);
                        locationPos -= new Vector2((pageSize.X + padding.X) / 2, (pageSize.Y + padding.Y) / 2);
                        break;
                }
            }
            return result;
        }
        private static Vector2 PlacePart(Vector2 locationPos, Vector2 originOffset, List<Vector2> result)
        {
            locationPos += new Vector2(-originOffset.X, -originOffset.Y);
            result.Add(locationPos);
            locationPos += new Vector2(originOffset.X, originOffset.Y);
            return locationPos;
        }
    }
}
