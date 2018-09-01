﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using AoLibs.Utilities.Android;
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


            RecyclerView.SetLayoutManager(new GridLayoutManager(Activity, 2));
        }

        private BoilerViewHolder HolderFactory(ViewGroup parent, int viewtype, View view)
        {
            return new BoilerViewHolder( view);
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