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
using System.Linq;
using System.Numerics;
using YCYR.Model.Top.Common;

namespace YCYR.Model.Common
{
    public enum OriginPositionEnum
    {
        LeftBottom,
        LeftTop,
        Center
    }
    public class PartExtents
    {
        public float MaxX { get; set; }
        public float MinX { get; set; }
        public float MaxY { get; set; }
        public float MinY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public static PartExtents CalcPartExtents(List<PartEntity> entities)
        {
            bool init = false;
            float xMin = 0;
            float xMax = 0;
            float yMin = 0;
            float yMax = 0;
            foreach (PartEntity entity in entities)
            {
                if (!init)
                {
                    if (xMin == 0) xMin = entity.Start.X;
                    if (xMax == 0) xMax = entity.Start.X;
                    if (yMin == 0) yMin = entity.Start.Y;
                    if (yMax == 0) yMax = entity.Start.Y;
                    init = true;
                }

                if (entity.Start.X < xMin)
                    xMin = entity.Start.X;
                if (entity.End.X < xMin)
                    xMin = entity.End.X;

                if (entity.Start.X > xMax)
                    xMax = entity.Start.X;
                if (entity.End.X > xMax)
                    xMax = entity.End.X;

                if (entity.Start.Y < yMin)
                    yMin = entity.Start.Y;
                if (entity.End.Y < yMin)
                    yMin = entity.End.Y;

                if (entity.Start.Y > yMax)
                    yMax = entity.Start.Y;
                if (entity.End.Y > yMax)
                    yMax = entity.End.Y;
            }

            float width = Math.Abs(xMax-xMin);
            float height = Math.Abs(yMax-yMin);

            return new PartExtents() { MaxX = xMax, MinX = xMin, MaxY = yMax, MinY = yMin, Width = width, Height = height };
        }
        public static PartExtents CalcPartExtents(PatternPart part)
        {
            return CalcPartExtents(part.Entities);
        }
        
        public static Vector2 CalcOffsetForOrigin(PatternPart part, OriginPositionEnum originPosition)
        {
            switch (originPosition)
            {
                default:
                case OriginPositionEnum.LeftBottom:
                    {
                        PartExtents dims = CalcPartExtents(part);
                        return new Vector2(dims.MinX, dims.MaxY);
                    }

                case OriginPositionEnum.LeftTop:
                    {
                        PartExtents dims = CalcPartExtents(part);
                        return new Vector2(dims.MinX, dims.MinY);
                    }
                case OriginPositionEnum.Center:
                    {
                        PartExtents dims = CalcPartExtents(part);
                        float xOff = dims.MinX + ((dims.MaxX - dims.MinX) / 2);
                        float yOff = dims.MinY + ((dims.MaxY - dims.MinY) / 2);
                        return new Vector2(xOff, yOff);
                    }
            }
        }

        public void Copy(PartExtents a)
        {
            Width = a.Width;
            Height = a.Height;
            MaxX = a.MaxX;
            MaxY = a.MaxY;
            MinX = a.MinX;
            MinY = a.MinY;
        }
        public bool IsEmpty()
        {
            if (Width == 0 && Height == 0 && MaxX == 0 && MaxY == 0 && MinX == 0 && MinY == 0)
                return true;
            else
                return false;
        }

    }
    public abstract partial class PatternPart
    {
        public List<PartEntity> Entities { get; set; }
        public Vector2 Start { get; set; }
        public Measurements Measurements { get; set; }
        public PatternPart(Measurements measurements)
        {
            Entities = new List<PartEntity>();
            Measurements = measurements;
        }

        public bool ShowConstruction { get; set; }
        public bool ShowSA { get; set; }
        public bool ShowText { get; set; }
        public bool ShowPath { get; set; }
        public bool ShowAttributes { get; set; }

        public YCYRColors FillColor { get; set; }
        public YCYRColors PathColor { get; set; }

        public virtual void BuildPart(Vector2 start)
        {
            Entities.Clear();
            this.Start = start;
        }
        
        public PartEntityMeasurements AddPartEntityMeasurements(Vector2 start, List<MeasurmentText> listLines)
        {
            PartEntityMeasurements entity = new PartEntityMeasurements(start, listLines);
            Entities.Add(entity);
            return entity;
        }
        public PartEntityGrainLine AddPartEntityGrainLine(Vector2 start, Vector2 end, string text, bool verticalText = false)
        {
            PartEntityGrainLine entity = new PartEntityGrainLine(start, end, text, verticalText);
            Entities.Add(entity);
            return entity;
        }
        public PartEntityText AddTextEntity(Vector2 start, string text, bool verticalText = false)
        {
            PartEntityText entity = new PartEntityText()
            {
                Start = start,
                Text = text,
                EntityType = EntityType.Text,
                ShowBoundingFrame = true,
            };
            if (verticalText)
                entity.Rotation = -90;
            Entities.Add(entity);
            return entity;
        }
        public PartEntityBezier AddBezierEntity(Vector2 start, Vector2 end, Vector2 adjustStart, Vector2 adjustEnd, EntityType type = EntityType.Normal)
        {
            PartEntityBezier entity = new PartEntityBezier(
                start,
                end,
                start + adjustStart,
                end + adjustEnd,
                type
            );
            Entities.Add(entity);
            return entity;
        }
        public PartEntityBezier AddBezierEntity(Vector2 end, Vector2 adjustStart, Vector2 adjustEnd, EntityType type = EntityType.Normal)
        {
            PartEntityBezier entity = new PartEntityBezier(
                Entities.Last().End,
                end,
                Entities.Last().End + adjustStart,
                end + adjustEnd,
                type
            );
            Entities.Add(entity);
            return entity;
        }
        public PartEntityLine AddLineEntity(float length, float angle, EntityType type = EntityType.Normal)
        {
            PartEntityLine entity = new PartEntityLine()
            {
                Start = Entities.Last().End,
                End = Utils.CalcEnd(Entities.Last().End, length, angle),
                EntityType = type
            };
            Entities.Add(entity);
            return entity;
        }
        public PartEntityLine AddLineEntity(Vector2 start, float length, float angle, EntityType type = EntityType.Normal)
        {
            PartEntityLine entity = new PartEntityLine()
            {
                Start = start,
                End = Utils.CalcEnd(start, length, angle),
                EntityType = type
            };
            Entities.Add(entity);
            return entity;
        }
        public PartEntityLine AddLineEntity(Vector2 end, EntityType type = EntityType.Normal)
        {
            PartEntityLine entity = new PartEntityLine()
            {
                Start = Entities.Last().End,
                End = end,
                EntityType = type
            };
            Entities.Add(entity);
            return entity;
        }
        public PartEntityLine AddLineEntity(LineDirection direction, float length, EntityType type = EntityType.Normal)
        {
            PartEntityLine entity = new PartEntityLine()
            {
                Start = Entities.Last().End,
                End = Utils.CalcEnd(Entities.Last().End, length, direction),
                EntityType = type
            };
            Entities.Add(entity);
            return entity;
        }
        public PartEntityLine AddLineEntity(LineDirection direction, Vector2 start, float length, EntityType type = EntityType.Normal)
        {
            PartEntityLine entity = new PartEntityLine()
            {
                Start = start,
                End = Utils.CalcEnd(start, length, direction),
                EntityType = type
            };
            Entities.Add(entity);
            return entity;
        }
        public PartEntityLine AddLineEntity(Vector2 start, Vector2 end, EntityType type = EntityType.Normal)
        {
            PartEntityLine entity = new PartEntityLine()
            {
                Start = start,
                End = end,
                EntityType = type
            };
            Entities.Add(entity);
            return entity;
        }

        public void AddLineEntities(PartEntityOffset offset)
        {
            offset.Lines.ForEach(x => Entities.Add(x));
        }
        public List<PartEntity> GetNormalEntitys()
        {
            return Entities.Where(x => x.EntityType == EntityType.Normal).ToList(); ;
        }
        public List<PartEntity> AddSALines(List<PerpConstructionLines> listOfSAContructionLines)
        {
            List<PartEntity> filtered = GetNormalEntitys();
            List<PartEntity> partsAdded = new List<PartEntity>();
            for (int i = 0; i < listOfSAContructionLines.Count; i++)
            {
                PartEntity part;
                if (i + 1 < filtered.Count)
                {
                    part = filtered[i + 1];
                }
                else
                {
                    part = filtered[0];
                }
                if (part is PartEntityLine)
                    partsAdded.Add(AddLineEntity(listOfSAContructionLines[i].LineAt1.End, listOfSAContructionLines[i].LineAt2.End, EntityType.SA));
                else if (part is PartEntityBezier)
                    partsAdded.Add(AddBezierEntity(listOfSAContructionLines[i].LineAt1.End, listOfSAContructionLines[i].LineAt2.End, ((PartEntityBezier)part).Control1, ((PartEntityBezier)part).Control2, EntityType.SA));
            };

            return partsAdded;
        }
        public List<PerpConstructionLines> AddDefaultSAConstructionLines(float length, PerpendicularRotation rotation, EntityType type)
        {
            List<PerpConstructionLines> listOfPerpPoints = new List<PerpConstructionLines>();
            List<PartEntity> filtered = GetNormalEntitys();

            for (int i = 0; i < filtered.Count; i++)
            {
                PartEntity first = filtered[i];
                PartEntity second;
                if (i + 1 < filtered.Count)
                    second = filtered[i + 1];
                else
                    second = filtered[0];

                PerpConstructionLines pcl = new PerpConstructionLines(second, second.End, first.End, 
                    second.End, first.End, length, rotation, type);
                listOfPerpPoints.Add(pcl);
            }
            return listOfPerpPoints;
        }

        
    }
}
