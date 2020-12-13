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
using System.Numerics;
using YCYR.Model.Top.Common;

namespace YCYR.Model.Common
{
    public class PartEntityMeasurements : PartEntityContainer
    {
        public PartEntityMeasurements(Vector2 startFirst, List<MeasurmentText> measurements)
        {
            Entities = new List<PartEntity>();
            EntityType = EntityType.Text;

            Vector2 start = startFirst;
            PartExtents boundsFirst = new PartExtents();
            float paddingBetweenLines = 3f;
            foreach (MeasurmentText line in measurements)
            {
                PartExtents bounds = Utils.CalcFontSizeBounds(line.TextSize, line.Text, "Arial");
                if (boundsFirst.IsEmpty())
                    boundsFirst.Copy(bounds);

                start += Utils.Down(bounds.Height + paddingBetweenLines);
                
                PartEntityText entityText = new PartEntityText()
                {
                    Start = start,
                    End = start + Utils.Right(bounds.Width),
                    Text = line.Text,
                    IsItalic = line.IsItalic,
                    IsBold = line.IsBold,
                    TextSize = line.TextSize,
                    EntityType = EntityType.Text,
                    ShowBoundingFrame = false,
                };
                Entities.Add(entityText);
            }

            PartExtents extents = PartExtents.CalcPartExtents(Entities);
            Start = new Vector2(extents.MinX, extents.MinY);
            End = new Vector2(extents.MaxX, extents.MaxY);

            float padding = 10f;
            PartEntityLine line1 = new PartEntityLine()
            {
                Start = Start + Utils.Up(boundsFirst.Height + paddingBetweenLines) + Utils.Left(padding / 2) + Utils.Up(padding / 2),
                End = Start +   Utils.Up(boundsFirst.Height + paddingBetweenLines) + Utils.Right(extents.Width) + Utils.Right(padding/2) + Utils.Up(padding / 2) ,
                EntityType = EntityType.MeasurementLine,
            };
            Entities.Add(line1);
            PartEntityLine line2 = new PartEntityLine()
            {
                Start = line1.End,
                End = line1.End + Utils.Down(extents.Height + boundsFirst.Height + paddingBetweenLines) + Utils.Down(padding),
                EntityType = EntityType.MeasurementLine,
            };
            Entities.Add(line2);
            PartEntityLine line3 = new PartEntityLine()
            {
                Start = line2.End,
                End = line2.End + Utils.Left(extents.Width) + Utils.Left(padding),
                EntityType = EntityType.MeasurementLine,
            };
            Entities.Add(line3);
            PartEntityLine line4 = new PartEntityLine()
            {
                Start = line3.End,
                End = line1.Start,
                EntityType = EntityType.MeasurementLine,
            };
            Entities.Add(line4);

            extents = PartExtents.CalcPartExtents(Entities);
            Start = new Vector2(extents.MinX, extents.MinY);
            End = new Vector2(extents.MaxX,extents.MaxY);
        }

        public override PartEntityOffset CalcOffset(float distance, PerpendicularRotation rotation, EntityType constructionType, EntityType lineType)
        {
            throw new NotImplementedException();
        }
    }
}
