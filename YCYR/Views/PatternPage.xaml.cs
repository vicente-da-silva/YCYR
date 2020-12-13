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
using System.ComponentModel;
using Xamarin.Forms;
using YCYR.ViewModels;
using YCYR.Model.Hoodie;
using YCYR.Model.Common;
using YCYR.Model;
using SkiaSharp.Views.Forms;
using SkiaScene;
using SkiaScene.TouchManipulation;
using SkiaSharp;
using TouchTracking;
using System.Threading.Tasks;

namespace YCYR.Views
{
    [DesignTimeVisible(false)]
    public partial class PatternPage : ContentPage
    {
        private string pathForFiles;
        private HoodiePatternRenderer patternRenderer;

        private ISKScene _scene;
        private ITouchGestureRecognizer _touchGestureRecognizer;
        private ISceneGestureResponder _sceneGestureResponder;
        private Measurements measurements;
        private bool showBasePattern;
        private bool readyToDraw;

        public PatternPage(string pathForFiles, Measurements measurements)
        {
            InitializeComponent();
            this.SizeChanged += OnSizeChanged;

            this.measurements = measurements;
            this.pathForFiles = pathForFiles;
            showBasePattern = false;
            readyToDraw = false;

            BindingContext = new PatternViewModel();
            patternRenderer = new HoodiePatternRenderer();
            patternRenderer.ProgressStateChanged += PatternRenderer_ProgressStateChanged;
        }

        private void PatternRenderer_ProgressStateChanged(object sender, EventArgs e)
        {
            string progress = (e as ProgressStateChangedEventArgs).State;
            Device.BeginInvokeOnMainThread(() =>
            {
                activityLabel.Text = progress;
            });
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            RedrawPattern();
        }

        void ShowHideBasePattern_Clicked(object sender, EventArgs e)
        {
            if (showBasePattern)
                showBasePattern = false;
            else
                showBasePattern = true;

            RedrawPattern();
        }

        void RedrawPattern()
        {
            busyIndicator.IsRunning = true;
            stackActivity.IsVisible = true;
            Task.Run(() =>
            {
                readyToDraw = false;
                SolutionFailureException ex = patternRenderer.BuildPattern(measurements, showBasePattern);
                if (ex == null)
                {
                    readyToDraw = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        canvasView.InvalidateSurface();
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayDialog("No solution found", "Could not find pattern solution for provided measurements: " + ex.FailureReason);
                    });
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    busyIndicator.IsRunning = false;
                    stackActivity.IsVisible = false;
                });
            });
        }

        void CreatePDF_Clicked(object sender, EventArgs e)
        {
            DisplayDialog("PDF path", patternRenderer.Print(pathForFiles));
        }

        void Recentre_Clicked(object sender, EventArgs e)
        {
            _scene = null;
            canvasView.InvalidateSurface();
        }
                

        private void SKCanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            args.Surface.Canvas.Clear();

            if (!readyToDraw)
                return;

            if (_scene == null)
                InitSceneObjects();
            
            _scene.Render(args.Surface.Canvas);
        }

        private async void DisplayDialog(string title, string path)
        {
            await DisplayAlert(title, path, "ok");
        }

        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            SetSceneCenter();
        }
        
        private void SetSceneCenter()
        {
            if (_scene == null)
            {
                return;
            }
            var centerPoint = new SKPoint(canvasView.CanvasSize.Width / 2, canvasView.CanvasSize.Height / 2);
            _scene.ScreenCenter = centerPoint;
        }

        private void InitSceneObjects()
        {
            _scene = new SKScene(new SceneRenderer(patternRenderer))
            {
                MaxScale = 10,
                MinScale = 0.3f,
            };
            SetSceneCenter();
            _touchGestureRecognizer = new TouchGestureRecognizer();
            _sceneGestureResponder = new SceneGestureRenderingResponder(() => canvasView.InvalidateSurface(), _scene, _touchGestureRecognizer)
            {
                TouchManipulationMode = TouchManipulationMode.IsotropicScale,
                MaxFramesPerSecond = 30,
            };
            _sceneGestureResponder.StartResponding();
        }

        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            if (_touchGestureRecognizer == null)
                return;

            var viewPoint = args.Location;
            SKPoint point =
                new SKPoint((float)(canvasView.CanvasSize.Width * viewPoint.X / canvasView.Width),
                            (float)(canvasView.CanvasSize.Height * viewPoint.Y / canvasView.Height));

            var actionType = args.Type;
            _touchGestureRecognizer.ProcessTouchEvent(args.Id, actionType, point);
        }

       
    }
}