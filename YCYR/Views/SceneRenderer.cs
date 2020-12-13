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

using SkiaScene;
using SkiaSharp;
using YCYR.Model.Hoodie;

namespace YCYR.Views
{
    public class SceneRenderer : ISKSceneRenderer
    {
        private readonly HoodiePatternRenderer patterns;

        public SceneRenderer(HoodiePatternRenderer patterns)
        {
            this.patterns = patterns;
        }
        public void Render(SKCanvas canvas, float angleInRadians, SKPoint center, float scale)
        {
            patterns.Paint(canvas);
        }
        public string Print(string path)
        {
            return patterns.Print(path);
        }
    }
}
