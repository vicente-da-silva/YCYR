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

namespace YCYR.Model.Common
{
    public class Pattern
    {
        public List<PatternPart> Parts { get; protected set; }

        public Pattern()
        {
            Parts = new List<PatternPart>();
        }

        public static float CalcTallestPart(Pattern pattern)
        {
            float ret = 0;
            foreach (PatternPart part in pattern.Parts)
            {
                PartExtents dims = PartExtents.CalcPartExtents(part);
                if (ret < dims.Height)
                    ret = dims.Height;
            }
            return ret;
        }
    }
}
