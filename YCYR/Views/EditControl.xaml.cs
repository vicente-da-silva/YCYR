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

namespace YCYR.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditControl : StackLayout
    {
        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText), typeof(string), typeof(EditControl));

        public string LabelText
        {
            get
            {
                return (string)GetValue(LabelTextProperty);
            }
            set
            {
                SetValue(LabelTextProperty, value);
            }
        }

       
        public static readonly BindableProperty MinProperty = BindableProperty.Create(nameof(Min), typeof(int), typeof(EditControl));
        public int Min
        {
            get
            {
                return (int)GetValue(MinProperty);
            }
            set
            {
                SetValue(MinProperty, value);
            }
        }


        public static readonly BindableProperty MaxProperty = BindableProperty.Create(nameof(Max), typeof(int), typeof(EditControl));
        public int Max
        {
            get
            {
                return (int)GetValue(MaxProperty);
            }
            set
            {
                SetValue(MaxProperty, value);
            }
        }

        public static readonly BindableProperty ValProperty = BindableProperty.Create(nameof(Val), typeof(int), typeof(EditControl));
        public int Val
        {
            get
            {
                return (int)GetValue(ValProperty);
            }
            set
            {
                //if (Value > Max || Value < Min)
                //    throw new Exception("EditControl: Value[" + Value + "] not in range Max[" + Max + "] Min[" + Min + "]");

                //System.Diagnostics.Debug.WriteLine("Val=" + Val + " MAx=" + Max + " Min=" + Min);

                SetValue(ValProperty, value);
            }
        }

        public EditControl()
        {
            InitializeComponent();

            //labelMeasurement.WidthRequest = stackEntry.Width + IncButton.Width + DecButton.Width;
        }

        public void Init()
        {
            slider.SetBinding(Slider.MaximumProperty, new Binding("Max", BindingMode.OneWay, null, null, null, EditControlStack));
            slider.SetBinding(Slider.MinimumProperty, new Binding("Min", BindingMode.OneWay, null, null, null, EditControlStack));
            slider.SetBinding(Slider.ValueProperty, new Binding("Val", BindingMode.TwoWay, null, null, null, EditControlStack));
        }

        private void DecButton_Clicked(object sender, EventArgs e)
        {
            if (Val - 1 >= Min)
                Val--;
        }

        private void IncButton_Clicked(object sender, EventArgs e)
        {
            if (Val + 1 <= Max)
                Val++;
        }
    }
}