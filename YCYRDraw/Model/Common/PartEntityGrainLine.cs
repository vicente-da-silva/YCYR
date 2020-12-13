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

namespace YCYR.Model.Common
{
    public class PartEntityGrainLine : PartEntityContainer
    {
        public PartEntityGrainLine(Vector2 start, Vector2 end, string text, bool verticalText)
        {
            PartEntityText entityText = new PartEntityText()
            {
                Start = start,
                Text = text,
                EntityType = EntityType.Text,
                ShowBoundingFrame = false,
            };
            if (verticalText)
                entityText.Rotation = -90;

            Start = start;
            End = end;
            EntityType = EntityType.GrainLine;
            Entities = new List<PartEntity>();
            Entities.Add(entityText);
            BuildGrainLine();
        }

        private void BuildGrainLine()
        {
            int arrowLength = 10;
            PartEntityLine line1 = new PartEntityLine()
            {
                EntityType = EntityType.GrainLine,
            };
            if (((PartEntityText)Entities[0]).Rotation == -90)
            {
                line1.Start = Start + Utils.Right(5);
                line1.End = End +  Utils.Right(5);
            }
            else
            {
                line1.Start = Start + Utils.Left(5);
                line1.End = End + Utils.Left(5);
            }
            Entities.Add(line1);

            PartEntityLine line2 = new PartEntityLine()
            {
                Start = line1.End,
                End = line1.End + Utils.Left(arrowLength) + Utils.Down(arrowLength),
                EntityType = EntityType.GrainLine,
            };
            Entities.Add(line2);
            PartEntityLine line3 = new PartEntityLine()
            {
                Start = line1.End,
                End = line1.End + Utils.Right(arrowLength) + Utils.Down(arrowLength),
                EntityType = EntityType.GrainLine,
            };
            Entities.Add(line3);
            PartEntityLine line4 = new PartEntityLine()
            {
                Start = line2.End,
                End = line3.End,
                EntityType = EntityType.GrainLine,
            };
            Entities.Add(line4);
        }

        public override PartEntityOffset CalcOffset(float distance, PerpendicularRotation rotation, EntityType constructionType, EntityType lineType)
        {
            throw new NotImplementedException();
        }
    }
}
