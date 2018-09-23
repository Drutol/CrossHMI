using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CrossHMI.Android.Views;
using CrossHMI.Shared.Devices;
using CrossHMI.Shared.NavArgs;
using CrossHMI.Shared.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using ME.Itangqi.Waveloadingview;
using NavigationLib.Android.Navigation;

namespace CrossHMI.Android.Fragment
{
    public class BoilderDetailsPageFragment : FragmentBase<BoilderDetailsViewModel>, IOnMapReadyCallback
    {
        private readonly List<Binding> _propertyBindings = new List<Binding>();
        private readonly TaskCompletionSource<GoogleMap> _mapTaskCompletionSource = new TaskCompletionSource<GoogleMap>();
        private Boiler _previouslyDisplayedBoiler;

        private Marker _marker;

        public override int LayoutResourceId { get; } = Resource.Layout.boiler_details_page;

        public override void NavigatedTo()
        {
            ViewModel.NavigatedTo(NavigationArguments as BoilderDetailsNavArgs);
            base.NavigatedTo();
        }

        public override void NavigatedFrom()
        {
            ClearPropertyBindings();
            base.NavigatedFrom();
        }

        private void ClearPropertyBindings()
        {
            foreach (var binding in _propertyBindings)
                binding.Detach();

            _propertyBindings.Clear();
        }

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Boiler).WhenSourceChanges(async () =>
            {
                if(ViewModel.Boiler == null)
                    return;

                if (_previouslyDisplayedBoiler != null)
                {
                    _previouslyDisplayedBoiler.PropertyThresholdStatusChanged -= PreviouslyDisplayedBoilerOnPropertyThresholdStatusChanged;
                }

                _previouslyDisplayedBoiler = ViewModel.Boiler;
                _previouslyDisplayedBoiler.PropertyThresholdStatusChanged +=
                    PreviouslyDisplayedBoilerOnPropertyThresholdStatusChanged;

                ClearPropertyBindings();
                            
                #region ValueBindings

                _propertyBindings.Add(this.SetBinding(() => ViewModel.Boiler.PipeX001_FTX001_Output).WhenSourceChanges(() =>
               Callback(PipeX001_FTX001_Output, () => ViewModel.Boiler.PipeX001_FTX001_Output)));

                _propertyBindings.Add(this.SetBinding(() => ViewModel.Boiler.PipeX001_ValveX001_Input).WhenSourceChanges(() =>
                    Callback(PipeX001_ValveX001_Input, () => ViewModel.Boiler.PipeX001_ValveX001_Input)));

                _propertyBindings.Add(this.SetBinding(() => ViewModel.Boiler.DrumX001_LIX001_Output).WhenSourceChanges(() =>
                    Callback(DrumX001_LIX001_Output, () => ViewModel.Boiler.DrumX001_LIX001_Output)));

                _propertyBindings.Add(this.SetBinding(() => ViewModel.Boiler.PipeX002_FTX002_Output).WhenSourceChanges(() =>
                    Callback(PipeX002_FTX002_Output, () => ViewModel.Boiler.PipeX002_FTX002_Output)));

                _propertyBindings.Add(this.SetBinding(() => ViewModel.Boiler.FCX001_ControlOut).WhenSourceChanges(() =>
                    Callback(FCX001_ControlOut, () => ViewModel.Boiler.FCX001_ControlOut)));

                _propertyBindings.Add(this.SetBinding(() => ViewModel.Boiler.FCX001_SetPoint).WhenSourceChanges(() =>
                    Callback(FCX001_SetPoint, () => ViewModel.Boiler.FCX001_SetPoint)));

                _propertyBindings.Add(this.SetBinding(() => ViewModel.Boiler.FCX001_Measurement).WhenSourceChanges(() =>
                    Callback(FCX001_Measurement, () => ViewModel.Boiler.FCX001_Measurement)));

                _propertyBindings.Add(this.SetBinding(() => ViewModel.Boiler.LCX001_ControlOut).WhenSourceChanges(() =>
                    Callback(LCX001_ControlOut, () => ViewModel.Boiler.LCX001_ControlOut)));

                _propertyBindings.Add(this.SetBinding(() => ViewModel.Boiler.LCX001_Measurement).WhenSourceChanges(() =>
                    Callback(LCX001_Measurement, () => ViewModel.Boiler.LCX001_Measurement)));

                _propertyBindings.Add(this.SetBinding(() => ViewModel.Boiler.LCX001_SetPoint).WhenSourceChanges(() =>
                    Callback(LCX001_SetPoint, () => ViewModel.Boiler.LCX001_SetPoint)));

                _propertyBindings.Add(this.SetBinding(() => ViewModel.Boiler.CCX001_ControlOut).WhenSourceChanges(() =>
                    Callback(CCX001_ControlOut, () => ViewModel.Boiler.CCX001_ControlOut)));

                _propertyBindings.Add(this.SetBinding(() => ViewModel.Boiler.CCX001_Input1).WhenSourceChanges(() =>
                    Callback(CCX001_Input1, () => ViewModel.Boiler.CCX001_Input1)));

                _propertyBindings.Add(this.SetBinding(() => ViewModel.Boiler.CCX001_Input2).WhenSourceChanges(() =>
                    Callback(CCX001_Input2, () => ViewModel.Boiler.CCX001_Input2)));

                _propertyBindings.Add(this.SetBinding(() => ViewModel.Boiler.CCX001_Input3).WhenSourceChanges(() =>
                    Callback(CCX001_Input3, () => ViewModel.Boiler.CCX001_Input3)));

                #endregion

                #region ImageValuesBindings

                Bindings.Add(
                    this.SetBinding(() => ViewModel.Boiler.FCX001_SetPoint,
                        () => FSetPoint.Text).ConvertSourceToTarget(ConvertPropertyValue));
                Bindings.Add(
                    this.SetBinding(() => ViewModel.Boiler.LCX001_SetPoint,
                        () => LSetPoint.Text).ConvertSourceToTarget(ConvertPropertyValue));

                Bindings.Add(
                    this.SetBinding(() => ViewModel.Boiler.LCX001_Measurement,
                        () => WaveView.ProgressValue).ConvertSourceToTarget(d => (int)d));

                Bindings.Add(
                    this.SetBinding(() => ViewModel.Boiler.PipeX001_ValveX001_Input,
                        () => ValveInput.Text).ConvertSourceToTarget(d => $"ValveInput = {d:N2}"));

                Bindings.Add(
                    this.SetBinding(() => ViewModel.Boiler.PipeX002_FTX002_Output,
                        () => FTMeasurement.Text).ConvertSourceToTarget(ConvertPropertyValue));

                #endregion

                #region UpperDashboardBindings

                Bindings.Add(
                    this.SetBinding(() => ViewModel.Boiler.LCX001_Measurement,
                        () => WaveViewUpper.ProgressValue).ConvertSourceToTarget(d => (int)d));

                Bindings.Add(
                    this.SetBinding(() => ViewModel.Boiler.PipeX001_ValveX001_Input,
                        () => InputPipeProgressBar.Progress).ConvertSourceToTarget(d => (int)d));

                Bindings.Add(
                    this.SetBinding(() => ViewModel.Boiler.PipeX002_FTX002_Output,
                        () => OutputPipeProgressBar.Progress).ConvertSourceToTarget(d => (int)d*1000));

                #endregion

                BoilerName.Text = ViewModel.Boiler.Repository;           

                var map = await _mapTaskCompletionSource.Task;

                _marker?.Remove();
                var pos = new LatLng(ViewModel.Boiler.Lat, ViewModel.Boiler.Lon);
                _marker = map.AddMarker(new MarkerOptions().SetPosition(pos).SetTitle(ViewModel.Boiler.Repository));
                map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(pos, 14f));
            }));


            #region PropertyLabelInitialization

            PipeX001_FTX001_Output.FindViewById<TextView>(Resource.Id.PropertyName).Text = nameof(PipeX001_FTX001_Output);
            PipeX001_ValveX001_Input.FindViewById<TextView>(Resource.Id.PropertyName).Text = nameof(PipeX001_ValveX001_Input);
            DrumX001_LIX001_Output.FindViewById<TextView>(Resource.Id.PropertyName).Text = nameof(DrumX001_LIX001_Output);
            PipeX002_FTX002_Output.FindViewById<TextView>(Resource.Id.PropertyName).Text = nameof(PipeX002_FTX002_Output);
            FCX001_ControlOut.FindViewById<TextView>(Resource.Id.PropertyName).Text = nameof(FCX001_ControlOut);
            FCX001_Measurement.FindViewById<TextView>(Resource.Id.PropertyName).Text = nameof(FCX001_Measurement);
            FCX001_SetPoint.FindViewById<TextView>(Resource.Id.PropertyName).Text = nameof(FCX001_SetPoint);
            LCX001_ControlOut.FindViewById<TextView>(Resource.Id.PropertyName).Text = nameof(LCX001_ControlOut);
            LCX001_Measurement.FindViewById<TextView>(Resource.Id.PropertyName).Text = nameof(LCX001_Measurement);
            LCX001_SetPoint.FindViewById<TextView>(Resource.Id.PropertyName).Text = nameof(LCX001_SetPoint);
            CCX001_ControlOut.FindViewById<TextView>(Resource.Id.PropertyName).Text = nameof(CCX001_ControlOut);
            CCX001_Input1.FindViewById<TextView>(Resource.Id.PropertyName).Text = nameof(CCX001_Input1);
            CCX001_Input2.FindViewById<TextView>(Resource.Id.PropertyName).Text = nameof(CCX001_Input2);
            CCX001_Input3.FindViewById<TextView>(Resource.Id.PropertyName).Text = nameof(CCX001_Input3);

            #endregion

            MapView.OnCreate(null);
            MapView.GetMapAsync(this);
        }

        private void PreviouslyDisplayedBoilerOnPropertyThresholdStatusChanged(object sender, (string Property, bool ExceedsThreshold) e)
        {
            if (e.Property == "DrumX001_LIX001_Output")
            {
                DrumX001_LIX001_Output.FindViewById<TextView>(Resource.Id.PropertyValue)
                    .SetTextColor(e.ExceedsThreshold ? Color.OrangeRed : Color.Black);
            }
        }


        private string ConvertPropertyValue(double arg)
        {
            return arg.ToString("N2");
        }

        private void Callback(LinearLayout propertyView, Func<double> value)
        {
            propertyView.FindViewById<TextView>(Resource.Id.PropertyValue).Text = value().ToString("N3");
        }

        #region MapLifecycle

        public override void OnPause()
        {
            base.OnPause();
            try
            {
                MapView?.OnPause();
            }
            catch (Exception)
            {
                //maps already paused
            }
        }

        public override void OnResume()
        {
            base.OnResume();
            try
            {
                MapView?.OnResume();
            }
            catch (Exception)
            {
                //maps already running
            }
        }

        #endregion

        #region Views

        private TextView _boilerName;
        private TextView _position;
        private ProgressBar _inputPipeProgressBar;
        private WaveLoadingControl _waveViewUpper;
        private ProgressBar _outputPipeProgressBar;
        private TextView _fSetPoint;
        private TextView _lSetPoint;
        private TextView _valveInput;
        private TextView _fTMeasurement;
        private WaveLoadingControl _waveView;
        private LinearLayout _pipeX001_FTX001_Output;
        private LinearLayout _pipeX001_ValveX001_Input;
        private LinearLayout _drumX001_LIX001_Output;
        private LinearLayout _pipeX002_FTX002_Output;
        private LinearLayout _fCX001_ControlOut;
        private LinearLayout _fCX001_Measurement;
        private LinearLayout _fCX001_SetPoint;
        private LinearLayout _lCX001_ControlOut;
        private LinearLayout _lCX001_Measurement;
        private LinearLayout _lCX001_SetPoint;
        private LinearLayout _cCX001_ControlOut;
        private LinearLayout _cCX001_Input1;
        private LinearLayout _cCX001_Input2;
        private LinearLayout _cCX001_Input3;
        private CustomMap _mapView;

        public TextView BoilerName => _boilerName ?? (_boilerName = FindViewById<TextView>(Resource.Id.BoilerName));
        public TextView Position => _position ?? (_position = FindViewById<TextView>(Resource.Id.Position));
        public ProgressBar InputPipeProgressBar => _inputPipeProgressBar ?? (_inputPipeProgressBar = FindViewById<ProgressBar>(Resource.Id.InputPipeProgressBar));
        public WaveLoadingControl WaveViewUpper => _waveViewUpper ?? (_waveViewUpper = FindViewById<WaveLoadingControl>(Resource.Id.WaveViewUpper));
        public ProgressBar OutputPipeProgressBar => _outputPipeProgressBar ?? (_outputPipeProgressBar = FindViewById<ProgressBar>(Resource.Id.OutputPipeProgressBar));
        public TextView FSetPoint => _fSetPoint ?? (_fSetPoint = FindViewById<TextView>(Resource.Id.FSetPoint));
        public TextView LSetPoint => _lSetPoint ?? (_lSetPoint = FindViewById<TextView>(Resource.Id.LSetPoint));
        public TextView ValveInput => _valveInput ?? (_valveInput = FindViewById<TextView>(Resource.Id.ValveInput));
        public TextView FTMeasurement => _fTMeasurement ?? (_fTMeasurement = FindViewById<TextView>(Resource.Id.FTMeasurement));
        public WaveLoadingControl WaveView => _waveView ?? (_waveView = FindViewById<WaveLoadingControl>(Resource.Id.WaveView));
        public LinearLayout PipeX001_FTX001_Output => _pipeX001_FTX001_Output ?? (_pipeX001_FTX001_Output = FindViewById<LinearLayout>(Resource.Id.PipeX001_FTX001_Output));
        public LinearLayout PipeX001_ValveX001_Input => _pipeX001_ValveX001_Input ?? (_pipeX001_ValveX001_Input = FindViewById<LinearLayout>(Resource.Id.PipeX001_ValveX001_Input));
        public LinearLayout DrumX001_LIX001_Output => _drumX001_LIX001_Output ?? (_drumX001_LIX001_Output = FindViewById<LinearLayout>(Resource.Id.DrumX001_LIX001_Output));
        public LinearLayout PipeX002_FTX002_Output => _pipeX002_FTX002_Output ?? (_pipeX002_FTX002_Output = FindViewById<LinearLayout>(Resource.Id.PipeX002_FTX002_Output));
        public LinearLayout FCX001_ControlOut => _fCX001_ControlOut ?? (_fCX001_ControlOut = FindViewById<LinearLayout>(Resource.Id.FCX001_ControlOut));
        public LinearLayout FCX001_Measurement => _fCX001_Measurement ?? (_fCX001_Measurement = FindViewById<LinearLayout>(Resource.Id.FCX001_Measurement));
        public LinearLayout FCX001_SetPoint => _fCX001_SetPoint ?? (_fCX001_SetPoint = FindViewById<LinearLayout>(Resource.Id.FCX001_SetPoint));
        public LinearLayout LCX001_ControlOut => _lCX001_ControlOut ?? (_lCX001_ControlOut = FindViewById<LinearLayout>(Resource.Id.LCX001_ControlOut));
        public LinearLayout LCX001_Measurement => _lCX001_Measurement ?? (_lCX001_Measurement = FindViewById<LinearLayout>(Resource.Id.LCX001_Measurement));
        public LinearLayout LCX001_SetPoint => _lCX001_SetPoint ?? (_lCX001_SetPoint = FindViewById<LinearLayout>(Resource.Id.LCX001_SetPoint));
        public LinearLayout CCX001_ControlOut => _cCX001_ControlOut ?? (_cCX001_ControlOut = FindViewById<LinearLayout>(Resource.Id.CCX001_ControlOut));
        public LinearLayout CCX001_Input1 => _cCX001_Input1 ?? (_cCX001_Input1 = FindViewById<LinearLayout>(Resource.Id.CCX001_Input1));
        public LinearLayout CCX001_Input2 => _cCX001_Input2 ?? (_cCX001_Input2 = FindViewById<LinearLayout>(Resource.Id.CCX001_Input2));
        public LinearLayout CCX001_Input3 => _cCX001_Input3 ?? (_cCX001_Input3 = FindViewById<LinearLayout>(Resource.Id.CCX001_Input3));
        public CustomMap MapView => _mapView ?? (_mapView = FindViewById<CustomMap>(Resource.Id.MapView));

        #endregion

        public void OnMapReady(GoogleMap googleMap)
        {
            _mapTaskCompletionSource.SetResult(googleMap);
            try
            {
                googleMap.MyLocationEnabled = true;
            }
            catch (Exception)
            {
                //location permission not granted
            }

        }
    }
}