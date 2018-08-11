using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using CrossHMI.Shared.Devices;
using CrossHMI.Shared.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using ME.Itangqi.Waveloadingview;
using NavigationLib.Android.Navigation;

namespace CrossHMI.Android.Fragment
{
    public class DashboardPageFragment : FragmentBase<DashboardViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.dashboard_page;

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Boilers).WhenSourceChanges(() =>
            {
                if (ViewModel.Boilers == null)
                    return;

                RecyclerView.SetAdapter(
                    new ObservableRecyclerAdapter<Boiler, BoilerViewHolder>(ViewModel.Boilers, DataTemplate,
                            ItemTemplate,HolderFactory)
                        {ApplyLayoutParams = true});
            }));


            RecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
        }

        private BoilerViewHolder HolderFactory(ViewGroup parent, int viewtype, View view)
        {
            return new BoilerViewHolder(this, view);
        }

        private View ItemTemplate(int viewtype)
        {
            return LayoutInflater.Inflate(Resource.Layout.dashboard_boiler_item, null);
        }

        private void DataTemplate(Boiler item, BoilerViewHolder holder, int position)
        {
            holder.BoilerName.Text = item.Repository;
            holder.Position.Text = $"Lat: {item.Lat}, Lon: {item.Lon}";
        }

        #region Views

        private RecyclerView _recyclerView;

        public RecyclerView RecyclerView => _recyclerView ?? (_recyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView));

        #endregion

        class BoilerViewHolder : BindingViewHolderBase<Boiler>
        {
            private readonly DashboardPageFragment _parent;
            private readonly View _view;

            private Dictionary<string,TextView> _valueViewsDictionary = new Dictionary<string, TextView>();

            public BoilerViewHolder(DashboardPageFragment parent, View view) : base(view)
            {
                _parent = parent;
                _view = view;
            }

            protected override void SetBindings()
            {
                PropertiesListView.RemoveAllViews();

                foreach (var propertyInfo in typeof(Boiler).GetTypeInfo().GetProperties()
                    .Where(info => info.GetCustomAttribute<NetworkDeviceBase.ProcessVariableAttribute>() != null)
                    .OrderBy(info => info.Name))
                {
                    var view = _parent.LayoutInflater.Inflate(Resource.Layout.dashboard_boiler_item_property, null);
                    view.FindViewById<TextView>(Resource.Id.PropertyName).Text = propertyInfo.Name;
                    var valueView = view.FindViewById<TextView>(Resource.Id.PropertyValue);
                    PropertiesListView.AddView(view);

                    _valueViewsDictionary[propertyInfo.Name] = valueView;
                }

                Bindings.Add(this.SetBinding(() => ViewModel.CCX001_ControlOut).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.CCX001_ControlOut)].Text =
                        ViewModel.CCX001_ControlOut.ToString("N3");
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.CCX001_Input1).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.CCX001_Input1)].Text =
                        ViewModel.CCX001_Input1.ToString("N3");
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.CCX001_Input2).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.CCX001_Input2)].Text =
                        ViewModel.CCX001_Input2.ToString("N3");
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.CCX001_Input3).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.CCX001_Input3)].Text =
                        ViewModel.CCX001_Input3.ToString("N3");
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.DrumX001_LIX001_Output).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.DrumX001_LIX001_Output)].Text =
                        ViewModel.DrumX001_LIX001_Output.ToString("N3");
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.FCX001_ControlOut).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.FCX001_ControlOut)].Text =
                        ViewModel.FCX001_ControlOut.ToString("N3");
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.FCX001_Measurement).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.FCX001_Measurement)].Text =
                        ViewModel.FCX001_Measurement.ToString("N3");
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.FCX001_SetPoint).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.FCX001_SetPoint)].Text =
                        ViewModel.FCX001_SetPoint.ToString("N3");
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.LCX001_ControlOut).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.LCX001_ControlOut)].Text =
                        ViewModel.LCX001_ControlOut.ToString("N3");
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.LCX001_Measurement).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.LCX001_Measurement)].Text =
                        ViewModel.LCX001_Measurement.ToString("N3");
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.CCX001_ControlOut).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.CCX001_ControlOut)].Text =
                        ViewModel.CCX001_ControlOut.ToString("N3");
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.LCX001_SetPoint).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.LCX001_SetPoint)].Text =
                        ViewModel.LCX001_SetPoint.ToString("N3");
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.CCX001_Input1).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.CCX001_Input1)].Text =
                        ViewModel.CCX001_Input1.ToString("N3");
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.PipeX001_FTX001_Output ).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.PipeX001_FTX001_Output)].Text =
                        ViewModel.PipeX001_FTX001_Output.ToString("N3");
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.PipeX002_FTX002_Output).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.PipeX002_FTX002_Output)].Text =
                        ViewModel.PipeX002_FTX002_Output.ToString("N3");
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.PipeX001_ValveX001_Input).WhenSourceChanges(() =>
                {
                    _valueViewsDictionary[nameof(ViewModel.PipeX001_ValveX001_Input)].Text =
                        ViewModel.PipeX001_ValveX001_Input.ToString("N3");
                }));
            }


            private TextView _boilerName;
            private TextView _position;
            private WaveLoadingControl _waveView;
            private LinearLayout _propertiesListView;

            public TextView BoilerName => _boilerName ?? (_boilerName = _view.FindViewById<TextView>(Resource.Id.BoilerName));
            public TextView Position => _position ?? (_position = _view.FindViewById<TextView>(Resource.Id.Position));
            public WaveLoadingControl WaveView => _waveView ?? (_waveView = _view.FindViewById<WaveLoadingControl>(Resource.Id.WaveView));
            public LinearLayout PropertiesListView => _propertiesListView ?? (_propertiesListView = _view.FindViewById<LinearLayout>(Resource.Id.PropertiesListView));
        }

    }
}