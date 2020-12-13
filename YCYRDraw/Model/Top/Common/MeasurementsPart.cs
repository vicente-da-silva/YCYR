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
using System.Collections.Generic;
using System.Numerics;


namespace YCYR.Model.Top.Common
{
    public class MeasurmentText
    {
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public string Text { get; set; }
        public float TextSize { get; set; }
    }
    public class MeasurementsPart : PatternPart
    {
        public MeasurementsPart(Vector2 start, Measurements measurements)
            : base(measurements)
        {
            BuildPart(start);
        }

        public override void BuildPart(Vector2 start)
        {
            base.BuildPart(start);

            Measurements measurementsBase = Measurements.Clone();
            measurementsBase.UnEase();

            List<MeasurmentText> allMeasurements = new List<MeasurmentText>();
            allMeasurements.Add(new MeasurmentText() { Text = "Garment Measurements", IsBold = true, TextSize = 24f });
            allMeasurements.AddRange(convertRange(measurementsBase.GetGarmentMeasurementsList()));
            allMeasurements.Add(new MeasurmentText() { Text = "Base Measurements", IsBold = true, TextSize = 24f });
            allMeasurements.AddRange(convertRange(measurementsBase.GetBodyMeasurementsList()));
            allMeasurements.Add(new MeasurmentText() { Text = "Ease Measurements", IsBold = true, TextSize = 24f });
            allMeasurements.AddRange(convertRange(measurementsBase.EaseMeasurements.GetEaseMeasurementsList()));
            allMeasurements.Add(new MeasurmentText() { Text = "Base Measurements with Ease Applied", IsBold = true, TextSize = 24f });
            allMeasurements.AddRange(convertRange(Measurements.GetBodyMeasurementsList()));

            AddPartEntityMeasurements(start, allMeasurements);
        }

        private List<MeasurmentText> convertRange(List<string> input)
        {
            return input.ConvertAll(x => { return new MeasurmentText() { Text = x , TextSize = 12f}; });
        }
    }
}
