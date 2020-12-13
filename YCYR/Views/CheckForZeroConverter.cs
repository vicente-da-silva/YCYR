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
using System.Globalization;
using Xamarin.Forms;

namespace YCYR.Views
{
    public class CheckForZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (int)value == 0)
                return null;

            System.Diagnostics.Debug.WriteLine("CheckForZeroConverter:" + value);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (double)value == 0)
                return null;

            return value;
        }
    }
}
