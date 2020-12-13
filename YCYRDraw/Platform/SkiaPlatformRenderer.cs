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
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace YCYR
{
    public class SkiaPlatformRenderer : PlatformRendererBase
    {
        private SKColor colorPathMain;
        private SKColor colorPath1;
        private SKColor colorTransparent;
        private SKColor colorConstruction;
        private SKColor colorPerpConstruction;
        private SKColor colorSA;
        private SKColor colorAttributes;
        private SKColor colorHA;
        private SKColor colorText;
        private SKColor colorTextPrint;

        private SKColor colorFillMain;
        private SKColor colorFill1;
        private SKColor colorFill2;
        private SKColor colorPathPrint;
        private SKColor colorFillPrint;

        private SKCanvas canvas;

        public override void PrintPattern(float scale, Pattern pattern, List<Vector2> positions, Vector2 padding, string path)
        {
            this.scale = scale;
            //PDF pages are sized in point units. 1 pt == 1/72 inch == 127/360 mm.
            //< table >
            //< thead >< tr >< th class="tdw1">Size</th><th class="tdw2">Width x Height(mm)</th><th class="tdw2">Width x Height(in)</th></tr></thead>
            //<tbody>
            //<tr><td>4A0</td><td>1682 x 2378 mm</td><td>66.2 x 93.6 in</td></tr>
            //<tr><td>2A0</td><td>1189 x 1682 mm</td><td>46.8 x 66.2 in</td></tr>
            //<tr><td>A0</td><td>841 x 1189 mm</td><td>33.1 x 46.8 in</td></tr>
            //<tr><td>A1</td><td>594 x 841 mm</td><td>23.4 x 33.1 in</td></tr>
            //<tr><td>A2</td><td>420 x 594 mm</td><td>16.5 x 23.4 in</td></tr>
            //<tr><td>A3</td><td>297 x 420 mm</td><td>11.7 x 16.5 in</td></tr>
            //<tr><td>A4</td><td> 210 x 297 mm</td><td>8.3 x 11.7 in</td></tr>
            //<tr><td>A5</td><td>148 x 210 mm</td><td>5.8 x 8.3 in</td></tr>
            //<tr><td>A6</td><td>105 x 148 mm</td><td>4.1 x 5.8 in</td></tr>
            //<tr><td>A7</td><td>74 x 105 mm</td><td>2.9 x 4.1 in</td></tr>
            //<tr><td>A8</td><td>52 x 74 mm</td><td>2.0 x 2.9 in</td></tr>
            //<tr><td>A9</td><td>37 x 52 mm</td><td>1.5 x 2.0 in</td></tr>
            //<tr><td>A10</td><td>26 x 37 mm</td><td>1.0 x 1.5 in</td></tr>
            //</tbody>
            //</table>
            
            SKDocumentPdfMetadata metadata = new SKDocumentPdfMetadata
            {
                Author = "Hoodie Maker",
                Creation = DateTime.Now,
                Creator = "Hoodie Maker",
                Keywords = "PDF, Hoodie, Pattern",
                Modified = DateTime.Now,
                Producer = "Hoodie Maker",
                Subject = "Hoodie Maker Pattern",
                Title = "Hoodie Pattern",
            };

            using (SKFileWStream stream = new SKFileWStream(path))
            using (SKDocument document = SKDocument.CreatePdf(stream, metadata))
            {
                for (int i = 0; i < pattern.Parts.Count; i++)
                {
                    Vector2 position = positions[i];
                    PatternPart part = pattern.Parts[i];
                    //A0 841mm x 1189 mm
                    //float width = pageSize.X * Utils.pntPerMM;
                    //float height = pageSize.Y * Utils.pntPerMM;
                    PartExtents extents = PartExtents.CalcPartExtents(part);
                    using (SKCanvas pdfCanvas = document.BeginPage(Scale(extents.Width + padding.X), Scale(extents.Height + padding.Y)))
                    {
                        canvas = pdfCanvas;
                        DrawPart(part, position,
                            colorFillPrint,
                            MapEntityColorsForPrint(EntityType.Normal),
                            MapEntityColorsForPrint(EntityType.Text));
                        document.EndPage();
                    }
                }
                document.Close();
            }
            System.Diagnostics.Debug.WriteLine(path);
            //Task.Run(async () => await OpenDoc(path));
        }
        public override void DrawPattern(float scale, Pattern pattern, List<Vector2> positions)
        {
            this.scale = scale;
            for (int i = 0; i < pattern.Parts.Count; i++)
            {
                DrawPart(pattern.Parts[i], positions[i],
                    MapColors(pattern.Parts[i].FillColor),
                    MapColors(pattern.Parts[i].PathColor),
                    MapEntityColors(EntityType.Text));
            }
        }
        public override void SetSession(object session)
        {
            canvas = (SKCanvas)session;
        }
        public override void CreateResources()
        {
            colorConstruction = new SKColor(0xF0, 0x52, 0x35); //SKColors.Red;
            colorConstruction = colorConstruction.WithAlpha(ConvertAlpha(0.4F));

            colorPerpConstruction = new SKColor(0xF0, 0x52, 0x35); //SKColors.Orange;
            colorPerpConstruction = colorPerpConstruction.WithAlpha(ConvertAlpha(0.4F));

            colorAttributes = new SKColor(0xF1, 0xf1, 0xd9); // SKColors.Yellow;
            colorAttributes = colorAttributes.WithAlpha(ConvertAlpha(0.4F));

            colorPathMain = new SKColor(0x85, 0xD4,0xE3); //SKColors.White;
            colorPathMain = colorPathMain.WithAlpha(ConvertAlpha(1F));

            colorSA = new SKColor(0x85, 0xD4, 0xE3); //SKColors.DeepSkyBlue;
            colorSA = colorSA.WithAlpha(ConvertAlpha(1F));

            colorHA = new SKColor(0x85, 0xD4, 0xE3); //SKColors.DodgerBlue;
            colorHA = colorHA.WithAlpha(ConvertAlpha(1F));

            colorText = new SKColor(0x85, 0xD4, 0xE3); //SKColors.White;
            colorText = colorText.WithAlpha(ConvertAlpha(1F));

            colorFillMain = new SKColor(0x17, 0x1f, 0x21);// SKColors.Gray;
            colorFillMain = colorFillMain.WithAlpha(ConvertAlpha(0.8F));

            colorFill1 = new SKColor(0xF1, 0xf1, 0xd9);//SKColors.LightBlue;
            colorFill1 = colorFill1.WithAlpha(ConvertAlpha(0.5F));

            colorPath1 = new SKColor(0xF1, 0xf1, 0xd9); //SKColors.White;
            colorPath1 = colorPath1.WithAlpha(ConvertAlpha(1F));

            colorFill2 = new SKColor(0x17, 0x1f, 0x21);//SKColors.LightBlue;
            colorFill2 = colorFill2.WithAlpha(ConvertAlpha(0.8F));

            colorTransparent = SKColors.White;
            colorTransparent = colorTransparent.WithAlpha(ConvertAlpha(0F));

            colorPathPrint = SKColors.Black;
            colorPathPrint = colorPathPrint.WithAlpha(ConvertAlpha(1F));

            colorTextPrint = SKColors.Black;
            colorTextPrint = colorTextPrint.WithAlpha(ConvertAlpha(0.3F));

            colorFillPrint = SKColors.Gray;
            colorFillPrint = colorFillPrint.WithAlpha(ConvertAlpha(0.1F));
        }
        private void DrawPart(PatternPart part, Vector2 position, SKColor fillColor, SKColor pathColor, SKColor textColor)
        {
            using (var pathBuilder = new SKPath())
            {
                pathBuilder.MoveTo(Scale(position.X), Scale(position.Y));

                foreach (PartEntity entity in part.Entities)
                {
                    if (entity is PartEntityBezier)
                    {
                        AddBezier(canvas, pathBuilder, ((PartEntityBezier)entity), Scale(position), MapEntityColors(entity.EntityType),part.ShowAttributes, part.ShowConstruction, part.ShowSA);
                    }
                    else if (entity is PartEntityLine)
                    {
                        AddLine(canvas, pathBuilder, ((PartEntityLine)entity), Scale(position), MapEntityColors(entity.EntityType),part.ShowConstruction, part.ShowSA);
                    }
                }
                pathBuilder.Close();

                SKPaint fillPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = fillColor,
                    IsAntialias = true,
                };

                SKPaint strokePaint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = pathColor,
                    IsAntialias = true,
                    StrokeWidth = 1,
                };

                if (!part.ShowPath)
                    strokePaint.Color = colorTransparent;

                canvas.DrawPath(pathBuilder, fillPaint);
                canvas.DrawPath(pathBuilder, strokePaint);

                foreach (PartEntity entity in part.Entities)
                {
                    if (entity is PartEntityText)
                    {
                        if (part.ShowText)
                            AddText(canvas, (PartEntityText)entity, Scale(position), textColor);
                    }
                    else if (entity is PartEntityContainer)
                    {
                        if (part.ShowText)
                            AddPartPartEntityContainer(canvas, pathBuilder, (PartEntityContainer)entity, Scale(position), textColor, textColor);
                    }
                }
            }
        }
        private void AddPartPartEntityContainer(SKCanvas canvas, SKPath pathBuilder, PartEntityContainer partEntityContainer, Vector2 position, SKColor textColor, SKColor lineColor)
        {
            foreach (PartEntity entity in partEntityContainer.Entities)
            {
                if(entity is PartEntityText)
                    AddText(canvas, (PartEntityText)entity, position, textColor);
                if (entity is PartEntityLine)
                    AddLine(canvas, pathBuilder, (PartEntityLine)entity, position, lineColor, true, true);
            }
        }
        
        private void AddText(SKCanvas canvas, PartEntityText textEntity, Vector2 position, SKColor textColor)
        {
            Vector2 start = Scale(textEntity.Start) + position;
            int padding = 20;
            using (var paintText = new SKPaint())
            using (var paintRect = new SKPaint())
            {
                paintText.TextSize = textEntity.TextSize * scale;
                paintText.IsAntialias = true;
                paintText.Color = textColor;
                paintText.IsStroke = false;
                paintText.Typeface = SKTypeface.FromFamilyName(
                    "Arial",
                    textEntity.IsBold?SKFontStyleWeight.ExtraBold : SKFontStyleWeight.Normal,
                    SKFontStyleWidth.Normal,
                    textEntity.IsItalic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright);

                paintRect.IsAntialias = true;
                paintRect.Color = textColor;
                paintRect.IsStroke = true;
                paintRect.StrokeWidth = 1;

                canvas.Save();
                canvas.RotateDegrees(textEntity.Rotation, start.X, start.Y);
                canvas.DrawText(textEntity.Text, start.X, start.Y, paintText);

                SKRect textBounds = new SKRect();
                paintText.MeasureText(textEntity.Text, ref textBounds);

                if (textEntity.ShowBoundingFrame)
                {
                    canvas.DrawRoundRect(
                       (start.X) - padding / 2,
                       (start.Y - textBounds.Height) - padding / 2,
                       textBounds.Width + padding,
                       textBounds.Height + padding,
                       3, 3,
                       paintRect);
                }
                canvas.Restore();
            }
        }
        private Vector2 AddLine(SKCanvas canvas, SKPath pathBuilder, PartEntityLine entity, Vector2 position, SKColor lineColor, 
            bool showConstruction, bool showSA)
        {
            Vector2 start = new Vector2(Scale(entity.Start.X) + position.X, Scale(entity.Start.Y) + position.Y);
            Vector2 end = new Vector2(Scale(entity.End.X) + position.X, Scale(entity.End.Y) + position.Y);

            if (entity.EntityType == EntityType.Normal)
                pathBuilder.LineTo(end.X, end.Y);
            else
            {
                bool draw = true;
                if ((entity.EntityType == EntityType.Construction || entity.EntityType == EntityType.PerpConstruction) && !showConstruction)
                        draw = false;
                if ((entity.EntityType == EntityType.SA || entity.EntityType == EntityType.HA) && !showSA)
                    draw = false;
                
                if (draw)
                {
                    using (var paint = new SKPaint())
                    {
                        paint.IsAntialias = true;
                        paint.Color = lineColor;
                        paint.IsStroke = true;
                        paint.StrokeWidth = 1;
                        canvas.DrawLine(ToP(start), ToP(end), paint);
                    }
                }
            }
            return entity.End;
        }
        private Vector2 AddBezier(SKCanvas canvas, SKPath pathBuilder, PartEntityBezier entity, Vector2 position, SKColor lineColor, 
            bool showAttribue, bool showConstruction, bool showSA)
        {
            if (showAttribue)
            {
                using (var paint = new SKPaint())
                {
                    paint.IsAntialias = true;
                    paint.Color = colorAttributes;
                    paint.IsStroke = false;
                    canvas.DrawCircle(ToP(Scale(entity.Control1) + position), 3f, paint);
                    canvas.DrawLine(ToP(Scale(entity.Control1) + position), ToP(Scale(entity.Start) + position), paint);
                    canvas.DrawCircle(ToP(Scale(entity.Control2) + position), 3f, paint);
                    canvas.DrawLine(ToP(Scale(entity.Control2) + position), ToP(Scale(entity.End) + position), paint);
                }
            }

            entity.Lines.ForEach(x =>
            {
                AddLine(canvas, pathBuilder, x, position, lineColor, showConstruction, showSA);
            });
           
            return entity.End;
        }
        public SKColor MapColors(YCYRColors color)
        {
            switch (color)
            {
                case YCYRColors.colorFillMain:
                    return this.colorFillMain;
                case YCYRColors.colorFill1:
                    return this.colorFill1;
                case YCYRColors.colorFill2:
                    return this.colorFill2;
                case YCYRColors.colorTransparent:
                    return this.colorTransparent;
                case YCYRColors.colorPathMain:
                    return this.colorPathMain;
                case YCYRColors.colorPath1:
                    return this.colorPath1;
                default:
                    return this.colorPathMain;
            }
        }
        public SKColor MapEntityColors(EntityType type)
        {
            switch (type)
            {
                case EntityType.Construction:
                    return this.colorConstruction;
                case EntityType.PerpConstruction:
                    return this.colorPerpConstruction;
                case EntityType.HA:
                    return this.colorHA;
                case EntityType.Normal:
                    return this.colorPathMain;
                case EntityType.SA:
                    return this.colorSA;
                case EntityType.Text:
                    return this.colorText;
                case EntityType.Attribute:
                    return this.colorAttributes;
                case EntityType.GrainLine:
                    return this.colorText;
                case EntityType.MeasurementLine:
                    return this.colorText;
                default:
                    return this.colorPathMain;
            }
        }
        public SKColor MapEntityColorsForPrint(EntityType type)
        {
            switch (type)
            {
                case EntityType.Construction:
                    return this.colorConstruction;
                case EntityType.PerpConstruction:
                    return this.colorPerpConstruction;
                case EntityType.HA:
                    return this.colorHA;
                case EntityType.Normal:
                    return this.colorPathPrint;
                case EntityType.SA:
                    return this.colorSA;
                case EntityType.Text:
                    return this.colorTextPrint;
                case EntityType.Attribute:
                    return this.colorAttributes;
                case EntityType.GrainLine:
                    return this.colorTextPrint;
                case EntityType.MeasurementLine:
                    return this.colorText;
                default:
                    return this.colorPathMain;
            }
        }
        private byte ConvertAlpha(float val)
        {
            return (byte)(255 * val);
        }
        private SKPoint ToP(Vector2 vector)
        {
            return new SKPoint(vector.X, vector.Y);
        }
    }
}
