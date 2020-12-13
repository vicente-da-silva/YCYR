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

using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using YCYR.Model;
using YCYR.Models;

namespace YCYR.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        private Dictionary<MenuItemType, NavigationPage> menuPages = new Dictionary<MenuItemType, NavigationPage>();
        private Measurements measurements;
        private EaseMeasurements easeMeasurements;

        public MainPage(string pathForFiles)
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            easeMeasurements = new EaseMeasurements();
            easeMeasurements.RetrieveMeasurments();

            measurements = new Measurements(easeMeasurements);
            measurements.RetrieveMeasurments();

            PatternPage page = new PatternPage(pathForFiles, measurements)
            {
                BindingContext = ((MenuPage)Master).MenuItems[0]
            };
            menuPages.Add(MenuItemType.Pattern, new NavigationPage(page));
            Detail = menuPages[MenuItemType.Pattern];
        }

        public async Task NavigateFromMenu(HomeMenuItem menuItem)
        {
            if (!menuPages.ContainsKey(menuItem.Id))
            {
                switch (menuItem.Id)
                {
                    //case (int)MenuItemType.Pattern:
                    //    MenuPages.Add(id, new NavigationPage(new PatternPage(pathForFiles, measurementsBase, measurementsWithEase)));
                    //    break;
                    case MenuItemType.About:
                        menuPages.Add(menuItem.Id, new NavigationPage(new AboutPage() { BindingContext = menuItem}));
                        break;
                    case MenuItemType.BodyMeasurements:
                        menuPages.Add(menuItem.Id, new NavigationPage(new BodyMeasurementsPage(measurements) { BindingContext = menuItem }));
                        break;
                    case MenuItemType.GarmentMeasurements:
                        menuPages.Add(menuItem.Id, new NavigationPage(new GarmentMeasurementsPage(measurements) { BindingContext = menuItem }));
                        break;
                    case MenuItemType.EaseMeasurements:
                        menuPages.Add(menuItem.Id, new NavigationPage(new EaseMeasurementsPage(easeMeasurements) { BindingContext = menuItem }));
                        break;
                }
            }
                        
            //if (menuItem.Id == (int)MenuItemType.Pattern)
            //{
            //    ((PatternPage)menuPages[MenuItemType.Pattern].CurrentPage).ReCalcPattern(measurements);
            //}
            
            var newPage = menuPages[menuItem.Id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}