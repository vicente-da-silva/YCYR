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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YCYR.Model;

namespace YCYR.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GarmentMeasurementsPage : ContentPage
    {
        private Measurements measurements;
        public GarmentMeasurementsPage(Measurements measurements)
        {
            InitializeComponent();
            this.measurements = measurements;
            MeasurementsStack.BindingContext = this.measurements;

            this.Disappearing += MeasurementsPage_Disappearing;

            ecGarmentBodiceLength.Init();
            ecGarmentHoodLenghtFromTemple.Init();
            ecGarmentHoodZipEdge.Init();
            ecGarmentHoodSetBackFromZip.Init();
            ecGarmentHoodInsertWidth.Init();
            ecGarmentHemAllowance.Init();
            ecGarmentSeamAllowance.Init();
        }

        private void MeasurementsPage_Disappearing(object sender, EventArgs e)
        {
            measurements.StoreMeasurments();
        }

        //private void ButtonSmallMeasurments_Clicked(object sender, EventArgs e)
        //{
        //    Measurements m = new Measurements(new EaseMeasurements());
        //    m.LoadGarmentSmallDefaults();
        //    measurements.CopyGarment(m);
        //    MeasurementsStack.BindingContext = null;
        //    MeasurementsStack.BindingContext = this.measurements;
        //}

        private void ButtonMediumMeasurments_Clicked(object sender, EventArgs e)
        {
            Measurements m = new Measurements(new EaseMeasurements());
            m.LoadGarmentMediumDefaults();
            measurements.CopyGarment(m);
            MeasurementsStack.BindingContext = null;
            MeasurementsStack.BindingContext = this.measurements;
        }

        //private void ButtonLargeMeasurments_Clicked(object sender, EventArgs e)
        //{
        //    Measurements m = new Measurements(new EaseMeasurements());
        //    m.LoadGarmentLargeDefaults();
        //    measurements.CopyGarment(m);
        //    MeasurementsStack.BindingContext = null;
        //    MeasurementsStack.BindingContext = this.measurements;
        //}
    }
}