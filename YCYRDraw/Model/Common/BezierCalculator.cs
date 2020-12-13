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
    public static class BezierCalculator
    {
        public static bool layoutOnly = false;

        public enum BezierCalculatorControlPointDirection
        {
            Up,
            Down,
            Left,
            Right,
            UpRight,
            UpLeft,
            DownRight,
            DownLeft
        }
        public class CalcResult
        {
            public Vector2 Firstpoint { get; private set; }
            public Vector2 SecondPoint { get; private set; }
            public float FoundLength { get; private set; }
            public bool SolutionFound { get; set; }

            public CalcResult(Vector2 firstpoint, Vector2 secondPoint, float foundLength)
            {
                Firstpoint = firstpoint;
                SecondPoint = secondPoint;
                FoundLength = foundLength;
                SolutionFound = true;
            }
            public CalcResult()
            {
                Firstpoint = new Vector2();
                SecondPoint = new Vector2();
                SolutionFound = false;
            }
        }
        private static Vector2 BezierCalculatorAdjustVector(float adjustment, BezierCalculatorControlPointDirection dir)
        {
            switch (dir)
            {
                case BezierCalculatorControlPointDirection.Down:
                    return Utils.Down(adjustment);

                case BezierCalculatorControlPointDirection.Up:
                    return Utils.Up(adjustment);

                case BezierCalculatorControlPointDirection.Left:
                    return Utils.Left(adjustment);

                case BezierCalculatorControlPointDirection.Right:
                    return Utils.Right(adjustment);

                case BezierCalculatorControlPointDirection.UpRight:
                    return Utils.UpRight(adjustment);

                case BezierCalculatorControlPointDirection.UpLeft:
                    return Utils.UpLeft(adjustment);

                case BezierCalculatorControlPointDirection.DownRight:
                    return Utils.DownRight(adjustment);

                case BezierCalculatorControlPointDirection.DownLeft:
                    return Utils.DownLeft(adjustment);
            }
            return new Vector2(0, 0);
        }
        public static CalcResult Calc(
            float bezierLengthRequired,
            Vector2 start,
            Vector2 end,
            BezierCalculatorControlPointDirection dir1,
            BezierCalculatorControlPointDirection dir2,
            Vector2 p1Adj = new Vector2(),
            Vector2 p2Adj = new Vector2(),
            int rangeMin = 1,
            int rangeMax = 200)
        {
            if (layoutOnly)
                return new CalcResult();

            int currentIterationValue = ((rangeMax - rangeMin) / 2) + rangeMin;
            int previousIterationValue = 0;
            CalcResult lengthFound = new CalcResult();
            for (int i = 1; i < 51; i++)
            {
                lengthFound = CalcLength(start, end, dir1, dir2, p1Adj, p2Adj, currentIterationValue);

                if (Math.Abs(bezierLengthRequired - lengthFound.FoundLength) <= 0.3F)
                {
                    //System.Diagnostics.Debug.WriteLine(" BezierCalculator requiredLength=" + bezierLengthRequired + " foundLength=" + lengthFound.FoundLength + " i=" + i);
                    lengthFound.SolutionFound = true;
                    return lengthFound;
                }

                if (lengthFound.FoundLength > bezierLengthRequired)
                    rangeMax = currentIterationValue;

                if (lengthFound.FoundLength < bezierLengthRequired)
                    rangeMin = currentIterationValue;

                currentIterationValue = ((rangeMax - rangeMin) / 2) + rangeMin;
                if (currentIterationValue == previousIterationValue)
                    break;
                previousIterationValue = currentIterationValue;
                //System.Diagnostics.Debug.WriteLine("currentIterationValue="+currentIterationValue);
            }
            //System.Diagnostics.Debug.WriteLine(description + " Warning - BezierCalculator, no soltion found after 50 iterations");
            lengthFound.SolutionFound = false;
            return lengthFound;// new CalcResult();
        }
        
        private static CalcResult CalcLength(
            Vector2 start,
            Vector2 end,
            BezierCalculatorControlPointDirection dir1,
            BezierCalculatorControlPointDirection dir2,
            Vector2 p1Adj,
            Vector2 p2Adj,
            int i )
        {
            Vector2 firstpoint = BezierCalculatorAdjustVector(i, dir1) + p1Adj;
            Vector2 secondPoint = BezierCalculatorAdjustVector(i, dir2) + p2Adj;
            float foundLength = new PartEntityBezier(start, end, start + firstpoint, end + secondPoint).Length;
            return new CalcResult(firstpoint, secondPoint, foundLength);
        }
        public static CalcResult CalcLength(
           Vector2 start,
           Vector2 end,
           BezierCalculatorControlPointDirection dir1,
           BezierCalculatorControlPointDirection dir2,
           float p1Adj,
           float p2Adj)
        {
            Vector2 firstpoint = BezierCalculatorAdjustVector(p1Adj, dir1);
            Vector2 secondPoint = BezierCalculatorAdjustVector(p2Adj, dir2);
            float foundLength = new PartEntityBezier(start, end, start + firstpoint, end + secondPoint).Length;
            return new CalcResult(firstpoint, secondPoint, foundLength);
        }
    }
}
