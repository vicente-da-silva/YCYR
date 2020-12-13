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
using Xamarin.Essentials;

namespace YCYR.Model
{
    public class EaseMeasurements
    {
        public Measurement MNeckCircumferenceEase { get; private set; } //ease around neck
        public Measurement MChestCirEasec { get; private set; } //ease around chest
        public Measurement MShoulderCircEase { get; private set; } //ease around shoulder
        public Measurement MBicepCircEase { get; private set; } //ease around bicep
        public Measurement MForeArmCircEase { get; private set; } //ease around forearm
        public Measurement MWristCircEase { get; private set; } //ease arount wrist
        public Measurement MCircTempleToTempleEase { get; private set; } //ease makes hoodie deeper
        public Measurement MShoulderToShoulderOverHeadEase { get; private set; } //ease makes hoodie wider

        public float NeckCircumferenceEase { get { return MNeckCircumferenceEase.Value; } set { MNeckCircumferenceEase.Value = value; } } //ease around neck
        public float ChestCirEasec { get { return MChestCirEasec.Value; } set { MChestCirEasec.Value = value; } } //ease around chest
        public float ShoulderCircEase { get { return MShoulderCircEase.Value; } set { MShoulderCircEase.Value = value; } } //ease around shoulder
        public float BicepCircEase { get { return MBicepCircEase.Value; } set { MBicepCircEase.Value = value; } } //ease around bicep
        public float ForeArmCircEase { get { return MForeArmCircEase.Value; } set { MForeArmCircEase.Value = value; } } //ease around forearm
        public float WristCircEase { get { return MWristCircEase.Value; } set { MWristCircEase.Value = value; } } //ease arount wrist
        public float CircTempleToTempleEase { get { return MCircTempleToTempleEase.Value; } set { MCircTempleToTempleEase.Value = value; } } //ease makes hoodie deeper
        public float ShoulderToShoulderOverHeadEase { get { return MShoulderToShoulderOverHeadEase.Value; } set { MShoulderToShoulderOverHeadEase.Value = value; } } //ease makes hoodie wider

        public EaseMeasurements()
            :this(true)
        {
           
        }

        public EaseMeasurements(bool loadMedium)
        {
            if (loadMedium)
                LoadMediumEaseDefaults();
            else
                LoadZeroEaseDefaults();

            LoadMinMaxValues();
        }

        public void StoreMeasurments()
        {
            Preferences.Set("NeckCircumferenceEase", NeckCircumferenceEase);
            Preferences.Set("ChestCirEasec", ChestCirEasec);
            Preferences.Set("ShoulderCircEase", ShoulderCircEase);
            Preferences.Set("BicepCircEase", BicepCircEase);
            Preferences.Set("ForeArmCircEase", ForeArmCircEase);
            Preferences.Set("WristCircEase", WristCircEase);
            Preferences.Set("CircTempleToTempleEase", CircTempleToTempleEase);
            Preferences.Set("ShoulderToShoulderOverHeadEase", ShoulderToShoulderOverHeadEase);
        }
        public void RetrieveMeasurments()
        {
            NeckCircumferenceEase = Preferences.Get("NeckCircumferenceEase", NeckCircumferenceEase);
            ChestCirEasec = Preferences.Get("ChestCirEasec", ChestCirEasec);
            ShoulderCircEase = Preferences.Get("ShoulderCircEase", ShoulderCircEase);
            BicepCircEase = Preferences.Get("BicepCircEase", BicepCircEase);
            ForeArmCircEase = Preferences.Get("ForeArmCircEase", ForeArmCircEase);
            WristCircEase = Preferences.Get("WristCircEase", WristCircEase);
            CircTempleToTempleEase = Preferences.Get("CircTempleToTempleEase", CircTempleToTempleEase);
            ShoulderToShoulderOverHeadEase = Preferences.Get("ShoulderToShoulderOverHeadEase", ShoulderToShoulderOverHeadEase);
        }

        public List<string> GetEaseMeasurementsList()
        {
            return new List<string>
            {
                "Neck Circumference Ease = " + NeckCircumferenceEase,
                "Chest Circumference Easec = " + ChestCirEasec,
                "Shoulder Circumference Ease = " + ShoulderCircEase,
                "Bicep Circumference Ease = " + BicepCircEase,
                "Fore Arm Circumference Ease = " + ForeArmCircEase,
                "Wrist Circumference Ease = " + WristCircEase,
                "Circ Temple To Temple Ease = " + CircTempleToTempleEase,
                "Shoulder To Shoulder Over Head Ease = " + ShoulderToShoulderOverHeadEase,
            };
        }

        public EaseMeasurements Clone()
        {
            return new EaseMeasurements()
            {
                MNeckCircumferenceEase = new Measurement(NeckCircumferenceEase),
                MChestCirEasec = new Measurement(ChestCirEasec),
                MShoulderCircEase = new Measurement(ShoulderCircEase),
                MBicepCircEase = new Measurement(BicepCircEase),
                MForeArmCircEase = new Measurement(ForeArmCircEase),
                MWristCircEase = new Measurement(WristCircEase),
                MCircTempleToTempleEase = new Measurement(CircTempleToTempleEase),
                MShoulderToShoulderOverHeadEase = new Measurement(ShoulderToShoulderOverHeadEase),
            };
        }

        public void Copy(EaseMeasurements m)
        {
            NeckCircumferenceEase = m.NeckCircumferenceEase;
            ChestCirEasec = m.ChestCirEasec;
            ShoulderCircEase = m.ShoulderCircEase;
            BicepCircEase = m.BicepCircEase;
            ForeArmCircEase = m.ForeArmCircEase;
            WristCircEase = m.WristCircEase;
            CircTempleToTempleEase = m.CircTempleToTempleEase;
            ShoulderToShoulderOverHeadEase = m.ShoulderToShoulderOverHeadEase;
        }

        private void LoadZeroEaseDefaults()
        {
            MNeckCircumferenceEase = new Measurement(0f); //ease around neck
            MChestCirEasec = new Measurement(0f); //ease around chest
            MShoulderCircEase = new Measurement(0f); //ease around shoulder
            MBicepCircEase = new Measurement(0f); //ease around bicep
            MForeArmCircEase = new Measurement(0f); //ease around forearm
            MWristCircEase = new Measurement(0f); //ease arount wrist
            MCircTempleToTempleEase = new Measurement(0f); //ease makes hoodie deeper
            MShoulderToShoulderOverHeadEase = new Measurement(0f); //ease makes hoodie wider
        }

        private void LoadMinMaxValues()
        {
            MNeckCircumferenceEase.SetMinMax(0, 20);
            MChestCirEasec.SetMinMax(0, 200);
            MShoulderCircEase.SetMinMax(0, 150);
            MBicepCircEase.SetMinMax(0, 100);
            MForeArmCircEase.SetMinMax(0, 100);
            MWristCircEase.SetMinMax(0, 100);
            MCircTempleToTempleEase.SetMinMax(0, 20);
            MShoulderToShoulderOverHeadEase.SetMinMax(0, 40);
        }

        public void LoadSmallEaseDefaults()
        {
            MNeckCircumferenceEase = new Measurement(5f); //ease around neck
            MChestCirEasec = new Measurement(50f); //ease around chest
            MShoulderCircEase = new Measurement(35f); //ease around shoulder
            MBicepCircEase = new Measurement(25f); //ease around bicep
            MForeArmCircEase = new Measurement(25f); //ease around forearm
            MWristCircEase = new Measurement(25f); //ease arount wrist
            MCircTempleToTempleEase = new Measurement(0f); //ease makes hoodie deeper
            MShoulderToShoulderOverHeadEase = new Measurement(10f); //ease makes hoodie wider
        }

        public void LoadMediumEaseDefaults()
        {
            MNeckCircumferenceEase = new Measurement(10f); //ease around neck
            MChestCirEasec = new Measurement(100f); //ease around chest
            MShoulderCircEase = new Measurement(70f); //ease around shoulder
            MBicepCircEase = new Measurement(50f); //ease around bicep
            MForeArmCircEase = new Measurement(50f); //ease around forearm
            MWristCircEase = new Measurement(50f); //ease arount wrist
            MCircTempleToTempleEase = new Measurement(10f); //ease makes hoodie deeper
            MShoulderToShoulderOverHeadEase = new Measurement(20f); //ease makes hoodie wider
        }

        public void LoadLargeEaseDefaults()
        {
            MNeckCircumferenceEase = new Measurement(20f); //ease around neck
            MChestCirEasec = new Measurement(200f); //ease around chest
            MShoulderCircEase = new Measurement(140f); //ease around shoulder
            MBicepCircEase = new Measurement(100f); //ease around bicep
            MForeArmCircEase = new Measurement(100f); //ease around forearm
            MWristCircEase = new Measurement(100f); //ease arount wrist
            MCircTempleToTempleEase = new Measurement(20f); //ease makes hoodie deeper
            MShoulderToShoulderOverHeadEase = new Measurement(40f); //ease makes hoodie wider
        }
    }
}




