using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Android;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using AoLibs.Navigation.Android.Navigation;
using AoLibs.Utilities.Android;
using CrossHMI.Shared.Devices;
using CrossHMI.Shared.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using ME.Itangqi.Waveloadingview;

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

            //RecyclerView.SetAdapter(
            //    new ObservableRecyclerAdapter<Boiler, BoilerViewHolder>(ViewModel.Boilers, DataTemplate,
            //            ItemTemplate,HolderFactory)
            //        {ApplyLayoutParams = true});
            //
            RecyclerView.SetAdapter(
                new ObservableRecyclerAdapterWithMultipleViewTypes<NetworkDeviceBase, RecyclerView.ViewHolder>(
                    new Dictionary<Type, ObservableRecyclerAdapterWithMultipleViewTypes<NetworkDeviceBase,
                        RecyclerView.ViewHolder>.IItemEntry>
                    {
                        {
                            typeof(Boiler),
                            new ObservableRecyclerAdapterWithMultipleViewTypes<NetworkDeviceBase, RecyclerView.ViewHolder>
                                .SpecializedItemEntry<Boiler, BoilerViewHolder>
                                {
                                    SpecializedDataTemplate = DataTemplate,
                                    SpecializedHolderFactory = BoilerHolderFactory,
                                    ItemTemplate = ItemTemplate
                                }
                        },
                        {
                            typeof(GenericDevice),
                            new ObservableRecyclerAdapterWithMultipleViewTypes<NetworkDeviceBase, RecyclerView.ViewHolder>
                                .SpecializedItemEntry<GenericDevice, DeviceViewHolder>
                                {
                                    SpecializedDataTemplate = DataTemplate,
                                    SpecializedHolderFactory = DeviceHolderFactory,
                                    ItemTemplate = ItemTemplate
                                }
                        }
                    }, ViewModel.Boilers));
            }));


            RecyclerView.SetLayoutManager(new GridLayoutManager(Activity, 2));
        }

        private void DataTemplate(GenericDevice item, DeviceViewHolder holder, int position)
        {
            holder.Title.Text = $"Custom boiler {item.Repository}"; // $"Lat: {item.Lat}, Lon: {item.Lon}";

            holder.CardView.SetOnClickCommand(ViewModel.NavigateToGenericDeviceDetailsCommand, item);
        }

        private DeviceViewHolder DeviceHolderFactory(ViewGroup parent, int viewtype, View view)
        {
            return new DeviceViewHolder(view);
        }

        private BoilerViewHolder BoilerHolderFactory(ViewGroup parent, int viewtype, View view)
        {
            return new BoilerViewHolder(view);
        }

        private View ItemTemplate(int viewtype)
        {
            return LayoutInflater.Inflate(Resource.Layout.dashboard_boiler_item, null);
        }

        private void DataTemplate(Boiler item, BoilerViewHolder holder, int position)
        {
            holder.Title.Text = item.Repository; // $"Lat: {item.Lat}, Lon: {item.Lon}";

            holder.CardView.SetOnClickCommand(ViewModel.NavigateToBoilerDetailsCommand, item);
        }

        #region Views

        private RecyclerView _recyclerView;

        public RecyclerView RecyclerView => _recyclerView ?? (_recyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView));

        #endregion

        class BoilerViewHolder : BindingViewHolderBase<Boiler>
        {
            private readonly View _view;

            public BoilerViewHolder(View view) : base(view)
            {
                _view = view;
            }

            protected override void SetBindings()
            {
                Bindings.Add(this.SetBinding(() => ViewModel.IsAnyValueThresholdExeeded).WhenSourceChanges(() =>
                {
                    if (ViewModel.IsAnyValueThresholdExeeded)
                    {
                        Status.Text = "Warning";
                        Status.SetTextColor(Color.OrangeRed);
                    }
                    else
                    {
                        Status.Text = "All OK";
                        Status.SetTextColor(Color.ParseColor("#98c926"));
                    }
                }));
            }

            private ImageView _image;
            private TextView _title;
            private TextView _status;
            private CardView _cardView;

            public ImageView Image => _image ?? (_image = _view.FindViewById<ImageView>(Resource.Id.Image));

            public TextView Title => _title ?? (_title = _view.FindViewById<TextView>(Resource.Id.Title));

            public TextView Status => _status ?? (_status = _view.FindViewById<TextView>(Resource.Id.Status));

            public CardView CardView => _cardView ?? (_cardView = _view.FindViewById<CardView>(Resource.Id.CardView));
        }

        class DeviceViewHolder : BindingViewHolderBase<GenericDevice>
        {
            private readonly View _view;

            public DeviceViewHolder(View view) : base(view)
            {
                _view = view;
            }

            protected override void SetBindings()
            {

            }

            private ImageView _image;
            private TextView _title;
            private TextView _status;
            private CardView _cardView;

            public ImageView Image => _image ?? (_image = _view.FindViewById<ImageView>(Resource.Id.Image));

            public TextView Title => _title ?? (_title = _view.FindViewById<TextView>(Resource.Id.Title));

            public TextView Status => _status ?? (_status = _view.FindViewById<TextView>(Resource.Id.Status));

            public CardView CardView => _cardView ?? (_cardView = _view.FindViewById<CardView>(Resource.Id.CardView));
        }
    }
}