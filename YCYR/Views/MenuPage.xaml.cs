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

using YCYR.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace YCYR.Views
{
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        public List<HomeMenuItem> MenuItems { get; }
        public MenuPage()
        {
            InitializeComponent();

            MenuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Pattern, Title="Pattern" },
                new HomeMenuItem {Id = MenuItemType.BodyMeasurements, Title="Body Measurements" },
                new HomeMenuItem {Id = MenuItemType.GarmentMeasurements, Title="Garment Measurements" },
                new HomeMenuItem {Id = MenuItemType.EaseMeasurements, Title="Ease Measurements" },
                new HomeMenuItem {Id = MenuItemType.About, Title="About" }
            };

            ListViewMenu.ItemsSource = MenuItems;
            ListViewMenu.SelectedItem = MenuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                await RootPage.NavigateFromMenu(((HomeMenuItem)e.SelectedItem));
            };
        }
    }
}