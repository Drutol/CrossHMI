using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CrossHMI.Shared.NavArgs;
using CrossHMI.Shared.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using ME.Itangqi.Waveloadingview;
using NavigationLib.Android.Navigation;

namespace CrossHMI.Android.Fragment
{
    public class BoilderDetailsPageFragment : FragmentBase<BoilderDetailsViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.boiler_details_page;
        private readonly List<Binding> _propertyBindings = new List<Binding>();

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
            Bindings.Add(this.SetBinding(() => ViewModel.Boiler).WhenSourceChanges(() =>
            {
                if(ViewModel.Boiler == null)
                    return;

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

        }

        private void Callback(LinearLayout propertyView, Func<double> value)
        {
            propertyView.FindViewById<TextView>(Resource.Id.PropertyValue).Text = value().ToString("N3");
        }

        #region Views

        private TextView _boilerName;
        private TextView _position;
        private TextView _fSetPoint;
        private TextView _lSetPoint;
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

        public TextView BoilerName => _boilerName ?? (_boilerName = FindViewById<TextView>(Resource.Id.BoilerName));

        public TextView Position => _position ?? (_position = FindViewById<TextView>(Resource.Id.Position));

        public TextView FSetPoint => _fSetPoint ?? (_fSetPoint = FindViewById<TextView>(Resource.Id.FSetPoint));

        public TextView LSetPoint => _lSetPoint ?? (_lSetPoint = FindViewById<TextView>(Resource.Id.LSetPoint));

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

        #endregion
    }
}