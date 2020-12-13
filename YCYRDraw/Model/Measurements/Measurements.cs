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
using Xamarin.Essentials;
using System.Collections.Generic;

namespace YCYR.Model
{
    public class Measurements
    {
        public Measurement MGarmentBodiceLength { get; private set; } //length of garment from bottom to body neckline
        public Measurement MGarmentHoodLenghtFromTemple { get; private set; }//how far from temple should hood extend
        public Measurement MGarmentHoodZipEdge { get; private set; } //use with garmentHoodSetBackFromZip this can be used for a zip edge to cater for set zip lengths
        public Measurement MGarmentHoodSetBackFromZip { get; private set; } //can be made 0 for no zip edge
        public Measurement MGarmentHoodInsertWidth { get; private set; } //can be made 0 for no insert
        public Measurement MGarmentSeamAllowance { get; private set; }
        public Measurement MGarmentHemAllowance { get; private set; }

        public Measurement MWristToForeArm { get; private set; } //no ease needed unless we want a slighly longer sleeve
        public Measurement MForeArmToBicep { get; private set; } //no ease needed
        public Measurement MBicepToUpperBicep { get; private set; }
        public Measurement MArmLength { get; private set; }//no ease needed, for integrity check only

        public Measurement MShoulderAngle { get; private set; }//no ease needed
        public Measurement MNeckCircumference { get; private set; }
        public Measurement MShoulderLength { get; private set; }
        public Measurement MChestCirc { get; private set; }
        public Measurement MShoulderCirc { get; private set; }
        public Measurement MBicepCirc { get; private set; }
        public Measurement MForeArmCirc { get; private set; }
        public Measurement MWristCirc { get; private set; }

        public Measurement MCircTempleToTemple { get; private set; }
        public Measurement MShoulderToShoulderOverHead { get; private set; }
        public Measurement MHeadBackCurveRadius { get; private set; } //no ease needed
                

        public float GarmentBodiceLength { get { return MGarmentBodiceLength.Value; } set { MGarmentBodiceLength.Value = value; } } //length of garment from bottom to body neckline
        public float GarmentHoodLenghtFromTemple { get { return MGarmentHoodLenghtFromTemple.Value; } set { MGarmentHoodLenghtFromTemple.Value = value; } }//how far from temple should hood extend
        public float GarmentHoodZipEdge { get { return MGarmentHoodZipEdge.Value; } set { MGarmentHoodZipEdge.Value = value; } } //use with garmentHoodSetBackFromZip this can be used for a zip edge to cater for set zip lengths
        public float GarmentHoodSetBackFromZip { get { return MGarmentHoodSetBackFromZip.Value; } set { MGarmentHoodSetBackFromZip.Value = value; } } //can be made 0 for no zip edge
        public float GarmentHoodInsertWidth { get { return MGarmentHoodInsertWidth.Value; } set { MGarmentHoodInsertWidth.Value = value; } } //can be made 0 for no insert
        public float GarmentSeamAllowance { get { return MGarmentSeamAllowance.Value; } set { MGarmentSeamAllowance.Value = value; } }
        public float GarmentHemAllowance { get { return MGarmentHemAllowance.Value; } set { MGarmentHemAllowance.Value = value; } }

        public float WristToForeArm { get { return MWristToForeArm.Value; } set { MWristToForeArm.Value = value; } } //no ease needed unless we want a slighly longer sleeve
        public float ForeArmToBicep { get { return MForeArmToBicep.Value; } set { MForeArmToBicep.Value = value; } } //no ease needed
        public float BicepToUpperBicep { get { return MBicepToUpperBicep.Value; } set { MBicepToUpperBicep.Value = value; } }
        public float ArmLength { get { return MArmLength.Value; } set { MArmLength.Value = value; } }//no ease needed, for integrity check only

        public float ShoulderAngle { get { return MShoulderAngle.Value; } set { MShoulderAngle.Value = value; } }//no ease needed
        public float NeckCircumference { get { return MNeckCircumference.Value; } set { MNeckCircumference.Value = value; } }
        public float ShoulderLength { get { return MShoulderLength.Value; } set { MShoulderLength.Value = value; } }
        public float ChestCirc { get { return MChestCirc.Value; } set { MChestCirc.Value = value; } }
        public float ShoulderCirc { get { return MShoulderCirc.Value; } set { MShoulderCirc.Value = value; } }
        public float BicepCirc { get { return MBicepCirc.Value; } set { MBicepCirc.Value = value; } }
        public float ForeArmCirc { get { return MForeArmCirc.Value; } set { MForeArmCirc.Value = value; } }
        public float WristCirc { get { return MWristCirc.Value; } set { MWristCirc.Value = value; } }

        public float CircTempleToTemple { get { return MCircTempleToTemple.Value; } set { MCircTempleToTemple.Value = value; } }
        public float ShoulderToShoulderOverHead { get { return MShoulderToShoulderOverHead.Value; } set { MShoulderToShoulderOverHead.Value = value; } }
        public float HeadBackCurveRadius { get { return MHeadBackCurveRadius.Value; } set { MHeadBackCurveRadius.Value = value; } } //no ease needed

        public EaseMeasurements EaseMeasurements { get; set; }

        public Measurements(EaseMeasurements easeMeasurements)
        {
            this.EaseMeasurements = easeMeasurements;
            LoadBodyMediumDefaults();
            LoadGarmentMediumDefaults();
            LoadMinMaxValues();
        }
        
        public void StoreMeasurments()
        {
            Preferences.Set("GarmentBodiceLength", GarmentBodiceLength);
            Preferences.Set("GarmentHoodLenghtFromTemple", GarmentHoodLenghtFromTemple);
            Preferences.Set("GarmentHoodZipEdge", GarmentHoodZipEdge);
            Preferences.Set("GarmentHoodSetBackFromZip", GarmentHoodSetBackFromZip);
            Preferences.Set("GarmentHoodInsertWidth", GarmentHoodInsertWidth);
            Preferences.Set("GarmentHemAllowance", GarmentHemAllowance);
            Preferences.Set("GarmentSeamAllowance", GarmentSeamAllowance);
            Preferences.Set("NeckCircumference", NeckCircumference);
            Preferences.Set("ShoulderAngle", ShoulderAngle);
            Preferences.Set("ShoulderLength", ShoulderLength);
            Preferences.Set("ChestCirc", ChestCirc);
            Preferences.Set("WristToForeArm", WristToForeArm);
            Preferences.Set("ForeArmToBicep", ForeArmToBicep);
            Preferences.Set("BicepToUpperBicep", BicepToUpperBicep);
            Preferences.Set("ArmLength", ArmLength);
            Preferences.Set("WristCirc", WristCirc);
            Preferences.Set("ForeArmCirc", ForeArmCirc);
            Preferences.Set("BicepCirc", BicepCirc);
            Preferences.Set("ShoulderCirc", ShoulderCirc);
            Preferences.Set("HeadBackCurveRadius", HeadBackCurveRadius);
            Preferences.Set("CircTempleToTemple", CircTempleToTemple);
            Preferences.Set("ShoulderToShoulderOverHead", ShoulderToShoulderOverHead);
        }
        public void RetrieveMeasurments()
        {
            GarmentBodiceLength = Preferences.Get("GarmentBodiceLength", GarmentBodiceLength);
            GarmentHoodLenghtFromTemple = Preferences.Get("GarmentHoodLenghtFromTemple", GarmentHoodLenghtFromTemple);
            GarmentHoodZipEdge = Preferences.Get("GarmentHoodZipEdge", GarmentHoodZipEdge);
            GarmentHoodSetBackFromZip = Preferences.Get("GarmentHoodSetBackFromZip", GarmentHoodSetBackFromZip);
            GarmentHoodInsertWidth = Preferences.Get("GarmentHoodInsertWidth", GarmentHoodInsertWidth);
            GarmentHemAllowance = Preferences.Get("GarmentHemAllowance", GarmentHemAllowance);
            GarmentSeamAllowance = Preferences.Get("GarmentSeamAllowance", GarmentSeamAllowance);
            NeckCircumference = Preferences.Get("NeckCircumference", NeckCircumference);
            ShoulderAngle = Preferences.Get("ShoulderAngle", ShoulderAngle);
            ShoulderLength = Preferences.Get("ShoulderLength", ShoulderLength);
            ChestCirc = Preferences.Get("ChestCirc", ChestCirc);
            WristToForeArm = Preferences.Get("WristToForeArm", WristToForeArm);
            ForeArmToBicep = Preferences.Get("ForeArmToBicep", ForeArmToBicep);
            BicepToUpperBicep = Preferences.Get("BicepToUpperBicep", BicepToUpperBicep);
            ArmLength = Preferences.Get("ArmLength", ArmLength);
            WristCirc = Preferences.Get("WristCirc", WristCirc);
            ForeArmCirc = Preferences.Get("ForeArmCirc", ForeArmCirc);
            BicepCirc = Preferences.Get("BicepCirc", BicepCirc);
            ShoulderCirc = Preferences.Get("ShoulderCirc", ShoulderCirc);
            HeadBackCurveRadius = Preferences.Get("HeadBackCurveRadius", HeadBackCurveRadius);
            CircTempleToTemple = Preferences.Get("CircTempleToTemple", CircTempleToTemple);
            ShoulderToShoulderOverHead = Preferences.Get("ShoulderToShoulderOverHead", ShoulderToShoulderOverHead);
        }

        public Measurements ApplyEase()
        {
            NeckCircumference += EaseMeasurements.NeckCircumferenceEase;
            ShoulderLength -= EaseMeasurements.NeckCircumferenceEase * Utils.half;//remove ease around neck from this
            ChestCirc += EaseMeasurements.ChestCirEasec;
            ShoulderCirc += EaseMeasurements.ShoulderCircEase;
            BicepCirc += EaseMeasurements.BicepCircEase;
            ForeArmCirc += EaseMeasurements.ForeArmCircEase;
            WristCirc += EaseMeasurements.WristCircEase;
            CircTempleToTemple += EaseMeasurements.CircTempleToTempleEase;
            ShoulderToShoulderOverHead += EaseMeasurements.ShoulderToShoulderOverHeadEase;
            return this;
        }
        public Measurements UnEase()
        {
            NeckCircumference -= EaseMeasurements.NeckCircumferenceEase;
            ShoulderLength += EaseMeasurements.NeckCircumferenceEase * Utils.half;//remove ease around neck from this
            ChestCirc -= EaseMeasurements.ChestCirEasec;
            ShoulderCirc -= EaseMeasurements.ShoulderCircEase;
            BicepCirc -= EaseMeasurements.BicepCircEase;
            ForeArmCirc -= EaseMeasurements.ForeArmCircEase;
            WristCirc -= EaseMeasurements.WristCircEase;
            CircTempleToTemple -= EaseMeasurements.CircTempleToTempleEase;
            ShoulderToShoulderOverHead -= EaseMeasurements.ShoulderToShoulderOverHeadEase;
            return this;
        }

        public Measurements Clone()
        {
            return new Measurements(EaseMeasurements.Clone())
            {
                MGarmentBodiceLength = new Measurement(GarmentBodiceLength),
                MGarmentHoodLenghtFromTemple = new Measurement(GarmentHoodLenghtFromTemple),
                MGarmentHoodZipEdge = new Measurement(GarmentHoodZipEdge),
                MGarmentHoodSetBackFromZip = new Measurement(GarmentHoodSetBackFromZip),
                MGarmentHoodInsertWidth = new Measurement(GarmentHoodInsertWidth),
                MGarmentHemAllowance = new Measurement(GarmentHemAllowance),
                MGarmentSeamAllowance = new Measurement(GarmentSeamAllowance),
                MNeckCircumference = new Measurement(NeckCircumference),
                MShoulderAngle = new Measurement(ShoulderAngle),
                MShoulderLength = new Measurement(ShoulderLength),
                MChestCirc = new Measurement(ChestCirc),
                MWristToForeArm = new Measurement(WristToForeArm),
                MForeArmToBicep = new Measurement(ForeArmToBicep),
                MBicepToUpperBicep = new Measurement(BicepToUpperBicep),
                MArmLength = new Measurement(ArmLength),
                MWristCirc = new Measurement(WristCirc),
                MForeArmCirc = new Measurement(ForeArmCirc),
                MBicepCirc = new Measurement(BicepCirc),
                MShoulderCirc = new Measurement(ShoulderCirc),
                MHeadBackCurveRadius = new Measurement(HeadBackCurveRadius),
                MCircTempleToTemple = new Measurement(CircTempleToTemple),
                MShoulderToShoulderOverHead = new Measurement(ShoulderToShoulderOverHead)
            };
        }

        public void CopyBody(Measurements m)
        {
            NeckCircumference = m.NeckCircumference;
            ShoulderAngle = m.ShoulderAngle;
            ShoulderLength = m.ShoulderLength;
            ChestCirc = m.ChestCirc;
            WristToForeArm = m.WristToForeArm;
            ForeArmToBicep = m.ForeArmToBicep;
            BicepToUpperBicep = m.BicepToUpperBicep;
            ArmLength = m.ArmLength;
            WristCirc = m.WristCirc;
            ForeArmCirc = m.ForeArmCirc;
            BicepCirc = m.BicepCirc;
            ShoulderCirc = m.ShoulderCirc;
            HeadBackCurveRadius = m.HeadBackCurveRadius;
            CircTempleToTemple = m.CircTempleToTemple;
            ShoulderToShoulderOverHead = m.ShoulderToShoulderOverHead;
        }
        public void CopyGarment(Measurements m)
        {
            GarmentBodiceLength = m.GarmentBodiceLength;
            GarmentHoodLenghtFromTemple = m.GarmentHoodLenghtFromTemple;
            GarmentHoodZipEdge = m.GarmentHoodZipEdge;
            GarmentHoodSetBackFromZip = m.GarmentHoodSetBackFromZip;
            GarmentHoodInsertWidth = m.GarmentHoodInsertWidth;
            GarmentHemAllowance = m.GarmentHemAllowance;
            GarmentSeamAllowance = m.GarmentSeamAllowance;
        }

        public List<string> GetBodyMeasurementsList()
        {
            return new List<string>
            {
                "Neck Circumference = " + NeckCircumference,
                "Shoulder Angle = " + ShoulderAngle,
                "Shoulder Length = " + ShoulderLength,
                "Chest Circumference = " + ChestCirc,
                "Wrist To ForeArm = " + WristToForeArm,
                "Fore Arm To Bicep = " + ForeArmToBicep,
                "Bicep To Upper Bicep = " + BicepToUpperBicep,
                "Arm Length = " + ArmLength,
                "Wrist Circumference = " + WristCirc,
                "ForeArm Circumference = " + ForeArmCirc,
                "Bicep Circumference = " + BicepCirc,
                "Shoulder Circumference = " + ShoulderCirc,
                "Head Back Curve Radius = " + HeadBackCurveRadius,
                "Circ Temple To Temple = " + CircTempleToTemple,
                "Shoulder To Shoulder OverHead = " + ShoulderToShoulderOverHead,
            };
        }
        public List<string> GetGarmentMeasurementsList()
        {
            return new List<string>
            {
                "Garment Bodice Length = " + GarmentBodiceLength,
                "Garment Hood Lenght From Temple = " + GarmentHoodLenghtFromTemple,
                "Garment Hood Zip Edge = " + GarmentHoodZipEdge,
                "Garment Hood Set Back From Zip = " + GarmentHoodSetBackFromZip,
                "Garment Hood Insert Width = " + GarmentHoodInsertWidth,
                "Garment Hem Allowance = " + GarmentHemAllowance,
                "Garment Seam Allowance = " + GarmentSeamAllowance,
            };
        }

        private void LoadMinMaxValues()
        {
            MGarmentBodiceLength.SetMinMax(400,1200);

            MGarmentHoodLenghtFromTemple.SetMinMax(30,60);
            MGarmentHoodZipEdge.SetMinMax(10,150);
            MGarmentHoodSetBackFromZip.SetMinMax(0,50);
            MGarmentHoodInsertWidth.SetMinMax(0,100);

            MGarmentHemAllowance.SetMinMax(5,50);
            MGarmentSeamAllowance.SetMinMax(5,25);

            MNeckCircumference.SetMinMax(300, 500);
            MShoulderAngle.SetMinMax(10, 20);
            MShoulderLength.SetMinMax(50, 200);
            MChestCirc.SetMinMax(700, 1300);

            MWristToForeArm.SetMinMax(100, 400);
            MForeArmToBicep.SetMinMax(100, 400);
            MBicepToUpperBicep.SetMinMax(50, 200);
            MArmLength.SetMinMax(500, 1000);

            MWristCirc.SetMinMax(75, 300);
            MForeArmCirc.SetMinMax(100, 400);
            MBicepCirc.SetMinMax(250, 500);
            MShoulderCirc.SetMinMax(350, 600);

            MHeadBackCurveRadius.SetMinMax(100, 200);
            MCircTempleToTemple.SetMinMax(360, 600);
            MShoulderToShoulderOverHead.SetMinMax(400, 800);
        }

        public void LoadBodyMediumDefaults()
        {
            MNeckCircumference = new Measurement(420f);
            MShoulderAngle = new Measurement(17f);
            MShoulderLength = new Measurement(120f);
            MChestCirc = new Measurement(1110f);

            MWristToForeArm = new Measurement(240f);
            MForeArmToBicep = new Measurement(180f);
            MBicepToUpperBicep = new Measurement(100f);
            MArmLength = new Measurement(650f);

            MWristCirc = new Measurement(170f);
            MForeArmCirc = new Measurement(290f);
            MBicepCirc = new Measurement(350f);
            MShoulderCirc = new Measurement(470f);

            MHeadBackCurveRadius = new Measurement(150f);
            MCircTempleToTemple = new Measurement(470f);
            MShoulderToShoulderOverHead = new Measurement(650f);
        }
        public void LoadGarmentMediumDefaults()
        {
            MGarmentBodiceLength = new Measurement(680f);

            MGarmentHoodLenghtFromTemple = new Measurement(50f);
            MGarmentHoodZipEdge = new Measurement(70f);
            MGarmentHoodSetBackFromZip = new Measurement(45f);
            MGarmentHoodInsertWidth = new Measurement(90f);

            MGarmentHemAllowance = new Measurement(25f);
            MGarmentSeamAllowance = new Measurement(10f);
        }
    }
}
