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
using System.Numerics;
using System.Collections.Generic;
using System.Linq;


namespace YCYR.Layouts
{
    public class MultiPatternLayout
    {
        public static List<List<Vector2>> CalcLayout(List<Pattern> allPatterns, Vector2 offsetOnSurface)
        {
            Pattern mainPattern = allPatterns.First();
            List<Pattern> layerPatterns = new List<Pattern>();
            for (int i = 1; i < allPatterns.Count; i++) //skip first
                layerPatterns.Add(allPatterns[i]);

            List<List<Vector2>> result = new List<List<Vector2>>();
            result.Add(new List<Vector2>()); //mainPattern result
            layerPatterns.ForEach(x => result.Add(new List<Vector2>()));
            Vector2 start = new Vector2() { X = 0 + offsetOnSurface.X, Y = 0 + offsetOnSurface.Y };
            Vector2 locationPos = new Vector2();
            locationPos += start;
           
            float tallest = Pattern.CalcTallestPart(mainPattern);
            locationPos += new Vector2(0, tallest / 2);
           
            for (int i = 0; i < mainPattern.Parts.Count; i++)
            {
                PatternPart mainPart = mainPattern.Parts[i];
                Vector2 mainPartOriginOffset = PartExtents.CalcOffsetForOrigin(mainPart, OriginPositionEnum.Center);
                PartExtents mainPartExtents = PartExtents.CalcPartExtents(mainPart);
                PartExtents mainPartNextDims = null;
                if (i + 1 < mainPattern.Parts.Count)
                {
                    PatternPart mainPartNext = mainPattern.Parts[i + 1];
                    mainPartNextDims = PartExtents.CalcPartExtents(mainPartNext);
                }
                
                //pattern main
                locationPos += new Vector2(-mainPartOriginOffset.X, -mainPartOriginOffset.Y);
                result[0].Add(locationPos);
                locationPos += new Vector2(mainPartOriginOffset.X, mainPartOriginOffset.Y);

                for (int j = 0; j < layerPatterns.Count; j++) 
                {
                    //pattern layer
                    PatternPart layerPart = layerPatterns[j].Parts[i];
                    Vector2 layerPartOriginOffset = PartExtents.CalcOffsetForOrigin(layerPart, OriginPositionEnum.Center);
                    locationPos += new Vector2(-layerPartOriginOffset.X, -layerPartOriginOffset.Y);
                    result[j+1].Add(locationPos);
                    locationPos += new Vector2(layerPartOriginOffset.X, layerPartOriginOffset.Y);
                }

                //use pattern main to layout
                locationPos += new Vector2(mainPartExtents.Width / 2, 0);
                if (mainPartNextDims != null)
                    locationPos += new Vector2(mainPartNextDims.Width / 2, 0);

                locationPos += new Vector2(50,0);//spacing between parts
            }
            return result;
        }
    }
}
