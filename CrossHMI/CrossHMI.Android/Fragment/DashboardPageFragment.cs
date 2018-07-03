using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using CrossHMI.Shared.Variables;
using CrossHMI.Shared.ViewModels;
using NavigationLib.Android.Navigation;

namespace CrossHMI.Android.Fragment
{
    public class DashboardPageFragment : FragmentBase<DashboardViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.dashboard_page;

        protected override void InitBindings()
        {
            RecyclerView.SetAdapter(
                new ObservableRecyclerAdapter<VariableUpdatedEntry, ValueViewHolder>(ViewModel.Updates, DataTemplate,
                    ItemTemplate) {ApplyLayoutParams = true});
            RecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
        }

        private View ItemTemplate(int viewtype)
        {
            return LayoutInflater.Inflate(Resource.Layout.dashboard_received_data_item, null);
        }

        private void DataTemplate(VariableUpdatedEntry item, ValueViewHolder holder, int position)
        {
            holder.Value.Text = item.Value;
            holder.VariableName.Text = item.Name;
        }

        #region Views

        private RecyclerView _recyclerView;

        public RecyclerView RecyclerView => _recyclerView ?? (_recyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView));

        #endregion

        class ValueViewHolder : RecyclerView.ViewHolder
        {
            private readonly View _view;

            public ValueViewHolder(View view) : base(view)
            {
                _view = view;
            }

            private TextView _variableName;
            private TextView _value;

            public TextView VariableName => _variableName ?? (_variableName = _view.FindViewById<TextView>(Resource.Id.VariableName));

            public TextView Value => _value ?? (_value = _view.FindViewById<TextView>(Resource.Id.Value));
        }
    }
}