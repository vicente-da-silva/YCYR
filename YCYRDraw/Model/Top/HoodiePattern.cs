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
using YCYR.Model.Top.Common;
using YCYR.Model.Top.Hood;
using System.Numerics;
using System;

namespace YCYR.Model.Hoodie
{
    public class ProgressStateChangedEventArgs : EventArgs
    {
        public string State { get; }

        public ProgressStateChangedEventArgs(string state)
        {
            State = state;
        }
    }
    public class HoodiePattern : Pattern
    {
        public event EventHandler ProgressStateChanged;

        protected virtual void OnProgressStateChanged(string e)
        {
            EventHandler handler = ProgressStateChanged;
            handler?.Invoke(this, new ProgressStateChangedEventArgs(e));
        }

        public void Build(Measurements measurements)
        {
            Vector2 start = new Vector2(0, 0);
            Parts.Clear();

            OnProgressStateChanged("Building Hood");
            HoodPart hood = new HoodPart(start, measurements);
            Parts.Add(hood);
            if (measurements.GarmentHoodInsertWidth > 0)
                Parts.Add(new HoodInsertPart(start, hood.HoodPathLengthForCutout, measurements));

            OnProgressStateChanged("Building Sleeve");
            SleevePart sleeve = new SleevePart(start, measurements);
            Parts.Add(sleeve);

            OnProgressStateChanged("Building Front Bodice");
            FrontBodicePart partFront = new FrontBodicePart(start, hood.HoodNeckFrontLength, sleeve.FrontArmBezierLength, measurements);
            Parts.Add(partFront);

            OnProgressStateChanged("Building Back Bodice");
            BackBodicePart partBack = new BackBodicePart(start, hood.HoodNeckBackLength, sleeve.BackArmBezierLength, measurements);
            Parts.Add(partBack);

            OnProgressStateChanged("Building Measurements Panel");
            MeasurementsPart measurementsPart = new MeasurementsPart(start, measurements);
            Parts.Add(measurementsPart);
        }

        public HoodiePattern()
        {
            
        }
    }
}
