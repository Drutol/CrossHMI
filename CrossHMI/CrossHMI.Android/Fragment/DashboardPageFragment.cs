using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using CrossHMI.Shared.Devices;
using CrossHMI.Shared.ViewModels;
using GalaSoft.MvvmLight.Helpers;
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
                if(ViewModel.Boilers == null)
                    return;

                RecyclerView.SetAdapter(
                    new ObservableRecyclerAdapter<Boiler, ValueViewHolder>(ViewModel.Boilers, DataTemplate,
                            ItemTemplate)
                        {ApplyLayoutParams = true});
            }));


            RecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
        }

        private View ItemTemplate(int viewtype)
        {
            return LayoutInflater.Inflate(Resource.Layout.dashboard_boiler_item, null);
        }

        private void DataTemplate(Boiler item, ValueViewHolder holder, int position)
        {
            holder.BoilerName.Text = item.Repository;
            holder.Position.Text = $"Lat: {item.Lat}, Lon: {item.Lon}";
        }

        #region Views

        private RecyclerView _recyclerView;

        public RecyclerView RecyclerView => _recyclerView ?? (_recyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView));

        #endregion

        class ValueViewHolder : BindingViewHolderBase<Boiler>
        {
            private readonly View _view;

            public ValueViewHolder(View view) : base(view)
            {
                _view = view;
            }

            protected override void SetBindings()
            {
                Bindings.Add(this.SetBinding(() => ViewModel.CCX001_ControlOut).WhenSourceChanges(() =>
                {
                    ToggleValue.Text = ViewModel.CCX001_ControlOut.ToString();
                }));

                Bindings.Add(this.SetBinding(() => ViewModel.CCX001_Input2).WhenSourceChanges(() =>
                {
                    Value.Text = ViewModel.CCX001_Input2.ToString();
                }));
            }

            private TextView _boilerName;
            private TextView _position;
            private TextView _toggleValue;
            private TextView _value;

            public TextView BoilerName => _boilerName ?? (_boilerName = _view.FindViewById<TextView>(Resource.Id.BoilerName));

            public TextView Position => _position ?? (_position = _view.FindViewById<TextView>(Resource.Id.Position));

            public TextView ToggleValue => _toggleValue ?? (_toggleValue = _view.FindViewById<TextView>(Resource.Id.ToggleValue));

            public TextView Value => _value ?? (_value = _view.FindViewById<TextView>(Resource.Id.Value));
        }
    }
}