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
using System.Numerics;

namespace YCYR.Model.Common
{
    public enum EntityType
    {
        Normal,
        Construction,
        PerpConstruction,
        SA,
        Text,
        HA,
        Attribute,
        GrainLine,
        MeasurementLine
    }
    public enum LineDirection
    {
        Left,
        Right,
        Up,
        Down
    }
    public static class Utils
    {
        public static float half = 1F / 2F;
        public static float quarter = 1F / 4F;
        public static float third = 1F / 3F;
        public static float twoThirds = 2F / 3F;
        public static float twoFifths = 2F / 5F;
        public static float threeFifths = 3F / 5F;
        public static float pntPerMM = 360f / 127f;
        public static float mmPerPnt = 127f / 360f;

        public static PartExtents CalcFontSizeBounds(float fontSize, string text, string fontFamily)
        {
            float scale = 1f;

            if (fontFamily != "Arial")
                throw new Exception("Only Arial supported at this time");

            float charHeight = FontHeightToMM(fontSize, scale);
            float charWidth = FontWidthToMM(fontSize, 0.61f, scale);
            return new PartExtents() { Width = charWidth * text.Length, Height = charHeight };
        }
        private static float FontHeightToMM(float fontSize, float scale)
        {
            //1point font = 1/72 inch
            float pointPerMM = 25.4f / 72f;
            return (pointPerMM * fontSize) * scale;
        }
        private static float FontWidthToMM(float fontSize, float aspectRatio, float scale)
        {
            return FontHeightToMM(fontSize, scale) * aspectRatio;
        }

        public static double HypFromSideAndAngle(double side, double angle)
        {
            return side / Math.Cos(angle);
        }
        public static double PythHypotenuse(double side1, double side2)
        {
            return Math.Sqrt(Math.Pow(side1, 2) + Math.Pow(side2, 2));
        }
        public static double PythSide(double hypotenuse, double side)
        {
            return Math.Sqrt(Math.Pow(hypotenuse, 2) - Math.Pow(side, 2));
        }
        public static double PythSide(double hypotenuse)
        {
            return Math.Sqrt((Math.Pow(hypotenuse, 2)) / 2);
        }
        public static Vector2 Up(float amount)
        {
            return new Vector2(0, -amount);
        }
        public static Vector2 Down(float amount)
        {
            return new Vector2(0, amount);
        }
        public static Vector2 Left(float amount)
        {
            return new Vector2(-amount, 0);
        }

        internal static Vector2 UpRight(float adjustment)
        {
            return new Vector2(adjustment, -adjustment);
        }

        public static Vector2 Right(float amount)
        {
            return new Vector2(amount, 0);
        }

        internal static Vector2 UpLeft(float adjustment)
        {
            return new Vector2(-adjustment, -adjustment);
        }

        internal static Vector2 DownRight(float adjustment)
        {
            return new Vector2(adjustment, adjustment);
        }
        internal static Vector2 DownLeft(float adjustment)
        {
            return new Vector2(-adjustment, adjustment);
        }

        public static Vector2 CalcEnd(Vector2 start, float length, float angle)
        {
            float endX = Convert.ToInt32(Math.Round(start.X - length * Math.Sin(Radians(angle))));
            float endY = Convert.ToInt32(Math.Round(start.Y + length * Math.Cos(Radians(angle))));

            return new Vector2(endX, endY);
        }

        public static Vector2 CalcEnd(Vector2 start, float length, LineDirection direction)
        {
            float angle = 0;
            switch (direction)
            {
                case LineDirection.Left:
                    angle = 90;
                    break;
                case LineDirection.Right:
                    angle = 270;
                    break;
                case LineDirection.Up:
                    angle = 180;
                    break;
                case LineDirection.Down:
                    angle = 360;
                    break;
            }
            return CalcEnd(start, length, angle);
        }
        public static double Radians(double dDegrees)
        {
            double result = Math.PI * dDegrees / 180.0;
            return result;
        }

        public static double CalcAngle(Vector2 intersection, Vector2 point1, Vector2 point2)
        {
            //Vector2 p1 = new Vector2(215, 294);
            //Vector2 p2 = new Vector2(174, 228);
            //Vector2 p3 = new Vector2(303, 294);
            //rad = 2.12667   deg = 121.849
            Vector2 v1 = point1 - intersection;
            Vector2 v2 = point2 - intersection;
            double rad = Math.Acos(Vector2.Dot(v1, v2) / (v1.Length() * v2.Length()));
            double deg = rad * 180.0 / Math.PI; //is this always the inner (smaller) angle?
            return deg;
            //if (deg > 180)
            //{
            //    deg = 360 - deg;
            //}
        }

        public static Vector2 CalcIntersection(PartEntityLine line1, PartEntityLine line2)
        {
            Vector2 a = line1.End - line1.Start;
            Vector2 b = line2.Start - line2.End;
            Vector2 intersection = CalcIntersection(a, line1.Start, b, line2.End);
            return intersection;
        }

        private static Vector2 CalcIntersection(Vector2 a, Vector2 tailA, Vector2 b, Vector2 tailB)
        {
            //Vector2 tailA = new Vector2(2, 5);
            //Vector2 tailB = new Vector2(1, 1);

            //Vector2 a = new Vector2(4, -5);
            //Vector2 b = new Vector2(6, 1); 
            Vector2 c = tailB - tailA;//(-1,-4)

            float prepc = Vector2.Dot(PerpendicularVector(c), b);//23
            float prepa = Vector2.Dot(PerpendicularVector(a), b); //34

            float t = prepc / prepa;//0.676

            //TODO: check this logic
            if (t < 0)
            {
                return Vector2.Zero;
            }

            Vector2 IntersectionPoint = (a * t) + tailA;  //(4.7,1.6)
            return IntersectionPoint;
        }

        public static Vector2 PerpendicularVector(Vector2 vector)
        {
            Vector2 left_normal = new Vector2();
            left_normal.X = vector.Y;
            left_normal.Y = -vector.X;
            return left_normal;
        }

    }

}
