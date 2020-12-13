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

using YCYR.Layouts;
using YCYR.Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace YCYR.Model.Hoodie
{
    public class HoodiePatternRenderer
    {
        private readonly float scaleDraw = 0.25F;
        private readonly float scalePrint = 1F;
        private readonly Vector2 offsetOnSurface = new Vector2(250, 50);
        private readonly PlatformRendererBase platformRenderer;

        private List<Pattern> patternsToDraw;
        private List<Vector2> layoutPositions;
        private bool showBasePattern;

        public event EventHandler ProgressStateChanged;

        protected virtual void OnProgressStateChanged(EventArgs e)
        {
            EventHandler handler = ProgressStateChanged;
            handler?.Invoke(this, e);
        }


        public HoodiePatternRenderer()
        {
            platformRenderer = new SkiaPlatformRenderer();
            platformRenderer.CreateResources();
        }
        public SolutionFailureException BuildPattern(Measurements measurements, bool showBasePattern)
        {
            try
            {
                this.showBasePattern = showBasePattern;

                patternsToDraw = new List<Pattern>();
                if (showBasePattern)
                {
                    HoodiePattern hoodiePatternBlockBase = new HoodiePattern();
                    hoodiePatternBlockBase.ProgressStateChanged += HoodiePatternBlockBase_ProgressStateChanged;
                    hoodiePatternBlockBase.Build(measurements);
                    patternsToDraw.Add(hoodiePatternBlockBase);
                }
                HoodiePattern hoodiePatternBlock = new HoodiePattern();
                hoodiePatternBlock.ProgressStateChanged += HoodiePatternBlockBase_ProgressStateChanged;
                hoodiePatternBlock.Build(measurements.Clone().ApplyEase());
                patternsToDraw.Insert(0, hoodiePatternBlock);
                return null;
            }
            catch (SolutionFailureException ex)
            {
                return ex;
            }

            //TestPattern();
            //return null;
        }

        private void HoodiePatternBlockBase_ProgressStateChanged(object sender, EventArgs e)
        {
            OnProgressStateChanged(e);
        }

        public void TestPattern()
        {
            try
            {
                showBasePattern = true;


                patternsToDraw = new List<Pattern>();
                //for (int i = 0; i < 50; i++)
                //{
                EaseMeasurements e = new EaseMeasurements(false);
                e.LoadLargeEaseDefaults();
                Measurements m = new Measurements(e);

                m.GarmentBodiceLength = m.MGarmentBodiceLength.Max;

                m.GarmentHoodLenghtFromTemple = m.MGarmentHoodLenghtFromTemple.Max;
                m.GarmentHoodZipEdge = m.MGarmentHoodZipEdge.Max;
                m.GarmentHoodSetBackFromZip = m.MGarmentHoodSetBackFromZip.Max;
                m.GarmentHoodInsertWidth = m.MGarmentHoodInsertWidth.Max;

                m.GarmentHemAllowance = m.MGarmentHemAllowance.Max;
                m.GarmentSeamAllowance = m.MGarmentSeamAllowance.Max;

                m.ChestCirc = m.MChestCirc.Max;
                m.ShoulderLength = m.MShoulderLength.Max;
                m.NeckCircumference = m.MNeckCircumference.Max;
                m.ShoulderAngle = m.MShoulderAngle.Max;

                m.WristToForeArm = m.MWristToForeArm.Max;
                m.ForeArmToBicep = m.MForeArmToBicep.Max;
                m.BicepToUpperBicep = m.MBicepToUpperBicep.Max;
                m.ArmLength = m.MArmLength.Max;

                m.WristCirc = m.MWristCirc.Max;
                m.ForeArmCirc = m.MForeArmCirc.Max;
                m.BicepCirc = m.MBicepCirc.Max;
                m.ShoulderCirc = m.MShoulderCirc.Max;

                m.HeadBackCurveRadius = m.MHeadBackCurveRadius.Max;
                m.CircTempleToTemple = m.MCircTempleToTemple.Max;
                m.ShoulderToShoulderOverHead = m.MShoulderToShoulderOverHead.Max;

                m.ApplyEase();

                HoodiePattern patternMax = new HoodiePattern();
                patternMax.Build(m);
                patternsToDraw.Add(patternMax);

                e = new EaseMeasurements(false);
                m = new Measurements(e);

                m.GarmentBodiceLength = m.MGarmentBodiceLength.Min;

                m.GarmentHoodLenghtFromTemple = m.MGarmentHoodLenghtFromTemple.Min;
                m.GarmentHoodZipEdge = m.MGarmentHoodZipEdge.Min;
                m.GarmentHoodSetBackFromZip = m.MGarmentHoodSetBackFromZip.Min;
                m.GarmentHoodInsertWidth = 10;// m.MGarmentHoodInsertWidth.Min;

                m.GarmentHemAllowance = m.MGarmentHemAllowance.Min;
                m.GarmentSeamAllowance = m.MGarmentSeamAllowance.Min;

                m.ChestCirc = m.MChestCirc.Min;
                m.ShoulderLength = m.MShoulderLength.Min;
                m.NeckCircumference = m.MNeckCircumference.Min;
                m.ShoulderAngle = m.MShoulderAngle.Min;

                m.WristToForeArm = m.MWristToForeArm.Min;
                m.ForeArmToBicep = m.MForeArmToBicep.Min;
                m.BicepToUpperBicep = m.MBicepToUpperBicep.Min;
                m.ArmLength = m.MArmLength.Min;

                m.WristCirc = m.MWristCirc.Min;
                m.ForeArmCirc = m.MForeArmCirc.Min;
                m.BicepCirc = m.MBicepCirc.Min;
                m.ShoulderCirc = m.MShoulderCirc.Min;

                m.HeadBackCurveRadius = m.MHeadBackCurveRadius.Min;
                m.CircTempleToTemple = m.MCircTempleToTemple.Min;
                m.ShoulderToShoulderOverHead = m.MShoulderToShoulderOverHead.Min;

                m.ApplyEase();

                HoodiePattern patternMin = new HoodiePattern();
                patternMin.Build(m);
                patternsToDraw.Add(patternMin);
                //}
            }
            catch (SolutionFailureException ex)
            {
                throw ex;
            }
        }
        public void Paint(object canvas)
        {
            platformRenderer.SetSession(canvas);

            for(int i = 0; i < patternsToDraw.Count; i++)
            {
                Pattern pattern = patternsToDraw[i];
                pattern.Parts.ForEach(x =>
                {
                    x.ShowPath = false;
                    x.ShowAttributes = false;
                    x.ShowConstruction = false;
                    x.ShowSA = false;
                    x.ShowText = false;
                    x.FillColor = showBasePattern ? YCYRColors.colorFill1 : YCYRColors.colorTransparent;
                    x.PathColor = showBasePattern ? YCYRColors.colorPath1 : YCYRColors.colorTransparent;
                });
            }

            patternsToDraw.First().Parts.ForEach(x => {
                x.ShowPath = true;
                x.ShowAttributes = true;
                x.ShowConstruction = true;
                x.ShowSA = true;
                x.ShowText = true;
                x.FillColor = YCYRColors.colorFillMain;
                x.PathColor = YCYRColors.colorPathMain;
            });

            List<List<Vector2>> positions = MultiPatternLayout.CalcLayout(patternsToDraw, offsetOnSurface);
            for (int i = patternsToDraw.Count-1; i >= 0; i--)
                platformRenderer.DrawPattern(scaleDraw, patternsToDraw[i], positions[i]);

            //int spacing = 50;
            //layoutPositions = SinglePatternLayout.CalcLayout(patternsToDraw.First(), offsetOnSurface, PartLayoutEnum.LeftToRight, OriginPositionEnum.Center, spacing);
            //platformRenderer.DrawPattern(scaleDraw, patternsToDraw.First(), layoutPositions);
        }
        public string Print(string pathToRoot)
        {
            if (patternsToDraw.Count == 0)
                return "";

            string path = Path.Combine(EnsureTempDataDirectory(pathToRoot, "HoodieMaker"), $"{Guid.NewGuid().ToString("N")}.pdf");
            Vector2 padding = new Vector2(20, 20); //10 mm each side , will be converted to points in PrintPattern

            patternsToDraw.First().Parts.ForEach(x => {
                x.ShowPath = true;
                x.ShowAttributes = false;
                x.ShowConstruction = false;
                x.ShowSA = true;
                x.ShowText = true;
                x.FillColor = YCYRColors.colorFill1;
            });

            layoutPositions = SinglePatternLayoutForPrint.CalcLayout(patternsToDraw.First(), OriginPositionEnum.Center, padding);
            platformRenderer.PrintPattern(scalePrint, patternsToDraw.First(), layoutPositions, padding, path);
            return path;
        }
        private static string EnsureTempDataDirectory(string path, string name)
        {
            string tempDataPath = Path.Combine(path, name);
            if (!Directory.Exists(tempDataPath))
                Directory.CreateDirectory(tempDataPath);
            return tempDataPath;
        }
    }
}
