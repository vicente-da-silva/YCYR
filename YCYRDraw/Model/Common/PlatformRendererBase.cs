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
using System.Numerics;

namespace YCYR.Model.Common
{
    public abstract class PlatformRendererBase
    {
        protected float scale = 1;

        public abstract void DrawPattern(float scale, Pattern pattern, List<Vector2> positions);

        public abstract void PrintPattern(float scale, Pattern pattern, List<Vector2> positions, Vector2 padding, string directory);

        public abstract void CreateResources();

        public abstract void SetSession(object session);

        //protected static Vector2 Offset(Vector2 pointToOffset, Vector2 offset)
        //{
        //    return new Vector2(Offset(offset.X, pointToOffset.X), Offset(offset.Y, pointToOffset.Y));
        //}
        //protected static float Offset(float offset, float point)
        //{
        //    return (float)((point + offset));
        //}
        protected static float Scale(float unit, float scale)
        {
            return (unit * Utils.pntPerMM) * scale;
        }
        protected static float Unscale(float unit, float scale)
        {
            return (unit / scale) / Utils.pntPerMM;
        }
        protected static Vector2 Scale(Vector2 unit, float scale)
        {
            return new Vector2(Scale(unit.X, scale), Scale(unit.Y, scale));
        }
        protected float Scale(float unit)
        {
            return Scale(unit, scale);
        }
        protected float Unscale(float unit)
        {
            return Unscale(unit, scale);
        }
        protected Vector2 Scale(Vector2 unit)
        {
            return Scale(unit, scale);
        }
    }
}
