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
    public enum PartLayoutEnum
    {
        LeftToRight,
        TopToBottom,
    }
    public class SinglePatternLayout
    {
        public static List<Vector2> CalcLayout(
            Pattern pattern, 
            Vector2 offsetOnSurface, 
            PartLayoutEnum layout, 
            OriginPositionEnum partOriginPosition, 
            int spacing)
        {
            List<Vector2> result = new List<Vector2>();
            Vector2 start = new Vector2() { X = 0 + offsetOnSurface.X, Y = 0 + offsetOnSurface.Y };
            Vector2 locationPos = new Vector2();
            locationPos += start;
            if (partOriginPosition == OriginPositionEnum.LeftBottom && layout == PartLayoutEnum.LeftToRight)
            {
                float tallest = Pattern.CalcTallestPart(pattern);
                locationPos += new Vector2(0, tallest);
            }
            if (partOriginPosition == OriginPositionEnum.Center && layout == PartLayoutEnum.LeftToRight)
            {
                float tallest = Pattern.CalcTallestPart(pattern);
                locationPos += new Vector2(0, tallest/2);
            }
            for (int i = 0; i < pattern.Parts.Count; i++)
            {
                PatternPart part = pattern.Parts[i];
                Vector2 originOffset = PartExtents.CalcOffsetForOrigin(part, partOriginPosition);
                PartExtents dims = PartExtents.CalcPartExtents(part);
                PartExtents partNextDims = null;
                if (i+1 < pattern.Parts.Count)
                {
                    PatternPart partNext = pattern.Parts[i+1];
                    partNextDims = PartExtents.CalcPartExtents(partNext);
                }

                switch (layout)
                {
                    default:
                    case PartLayoutEnum.LeftToRight:
                        switch (partOriginPosition)
                        {
                            default:
                            case OriginPositionEnum.LeftTop:
                            case OriginPositionEnum.LeftBottom:
                            case OriginPositionEnum.Center:
                                locationPos = LeftToRight(locationPos, originOffset, dims, result, partOriginPosition, partNextDims, spacing);
                                break;
                        }
                        break;
                    case PartLayoutEnum.TopToBottom:
                        switch(partOriginPosition)
                        {
                            default:
                            case OriginPositionEnum.LeftTop:
                            case OriginPositionEnum.LeftBottom:
                            case OriginPositionEnum.Center:
                                locationPos = TopToBottom(locationPos, originOffset, dims, result, partOriginPosition, partNextDims, spacing);
                                break;
                        }
                        break;
                }
            }
            return result;
        }
        private static Vector2 LeftToRight(
            Vector2 locationPos, 
            Vector2 originOffset, 
            PartExtents dims, 
            List<Vector2> result, 
            OriginPositionEnum originPosition, 
            PartExtents partNextDims, 
            int spacing)
        {
            locationPos += new Vector2(-originOffset.X, -originOffset.Y);
            result.Add(locationPos);
            locationPos += new Vector2(originOffset.X, originOffset.Y);

            if (originPosition == OriginPositionEnum.Center)
            {
                //move forward by 1/2 this ones width and 1/2 of the next ones width
                locationPos += new Vector2(dims.Width / 2, 0);
                if (partNextDims != null)
                    locationPos += new Vector2(partNextDims.Width / 2, 0);
            }
            else
                locationPos += new Vector2(dims.Width, 0);

            locationPos += Utils.Right(spacing);

            return locationPos;
        }
        private static Vector2 TopToBottom(
            Vector2 locationPos, 
            Vector2 originOffset, 
            PartExtents dims, 
            List<Vector2> result, 
            OriginPositionEnum originPosition, 
            PartExtents partNextDims, 
            int spacing)
        {
            locationPos += new Vector2(-originOffset.X, -originOffset.Y);
            result.Add(locationPos);
            locationPos += new Vector2(originOffset.X, originOffset.Y);

            if (originPosition == OriginPositionEnum.Center)
            {
                //move forward by 1/2 this ones width and 1/2 of the next ones width
                locationPos += new Vector2(0, dims.Height / 2);
                if (partNextDims != null)
                    locationPos += new Vector2(0, partNextDims.Height / 2);
            }
            else if (originPosition == OriginPositionEnum.LeftBottom)
            {
                if (partNextDims != null)
                    locationPos += new Vector2(0, partNextDims.Height);
            }
            else
                locationPos += new Vector2(0, dims.Height);

            locationPos += Utils.Down(spacing);

            return locationPos;
        }
    }
}
