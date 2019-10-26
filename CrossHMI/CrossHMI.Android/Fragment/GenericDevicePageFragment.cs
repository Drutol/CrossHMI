using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using AoLibs.Navigation.Android.Navigation;
using CrossHMI.Shared.Devices;
using CrossHMI.Shared.NavArgs;
using CrossHMI.Shared.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using Org.Apache.Http.Entity;

namespace CrossHMI.Android.Fragment
{
    public class GenericDevicePageFragment : FragmentBase<GenericDetailsViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.generic_device_details_page;

        public override void NavigatedTo()
        {
            ViewModel.NavigatedTo(NavigationArguments as GenericDetailsNavArgs);
        }

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Device).WhenSourceChanges(() =>
            {
                if (ViewModel.Device == null)
                    return;


                BoilerName.Text = ViewModel.Device.Repository;

                PropertiesRecyclerView.SetAdapter(
                    new ObservableRecyclerAdapter<string, PropertyViewHolder>(ViewModel.Device.PropertiesNames,
                        DataTemplate,
                        ItemTemplate, HolderFactory) {StretchContentHorizonatally = true});
                PropertiesRecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            }));
        }

        private PropertyViewHolder HolderFactory(ViewGroup parent, int viewtype, View view)
        {
            return new PropertyViewHolder(this, view);
        }

        private View ItemTemplate(int viewtype)
        {
            return LayoutInflater.Inflate(Resource.Layout.boiler_property, null);
        }

        private void DataTemplate(string item, PropertyViewHolder holder, int position)
        {
            holder.PropertyName.Text = item;
        }

        class PropertyViewHolder : BindingViewHolderBase<string>
        {
            private readonly GenericDevicePageFragment _parent;
            private readonly View _view;
            private PropertyChangedEventHandler _handler;
            private GenericDevice _previousDevice;

            public PropertyViewHolder(GenericDevicePageFragment parent, View view) : base(view)
            {
                _parent = parent;
                _view = view;
            }

            protected override void SetBindings()
            {
                var property = ViewModel;
                _handler = (sender, args) =>
                {
                    if (args.PropertyName == property)
                    {
                        PropertyValue.Text = _parent.ViewModel.Device.Values[property].ToString();
                    }
                };
                if (_parent.ViewModel.Device.Values.ContainsKey(property))
                {
                    PropertyValue.Text = _parent.ViewModel.Device.Values[property].ToString();
                }
                _previousDevice = _parent.ViewModel.Device;
                _previousDevice.PropertyChanged += _handler;
            }

            public override void DetachBindings()
            {
                _previousDevice.PropertyChanged -= _handler;
            }

            private TextView _propertyName;
            private ImageView _iconWarning;
            private TextView _propertyValue;

            public TextView PropertyName => _propertyName ?? (_propertyName = _view.FindViewById<TextView>(Resource.Id.PropertyName));
            public ImageView IconWarning => _iconWarning ?? (_iconWarning = _view.FindViewById<ImageView>(Resource.Id.IconWarning));
            public TextView PropertyValue => _propertyValue ?? (_propertyValue = _view.FindViewById<TextView>(Resource.Id.PropertyValue));
        }


        #region Views

        private TextView _boilerName;
        private TextView _position;
        private RecyclerView _propertiesRecyclerView;
        private ScrollView _scrollView;

        public TextView BoilerName => _boilerName ?? (_boilerName = FindViewById<TextView>(Resource.Id.BoilerName));
        public TextView Position => _position ?? (_position = FindViewById<TextView>(Resource.Id.Position));
        public RecyclerView PropertiesRecyclerView => _propertiesRecyclerView ?? (_propertiesRecyclerView = FindViewById<RecyclerView>(Resource.Id.PropertiesRecyclerView));
        public ScrollView ScrollView => _scrollView ?? (_scrollView = FindViewById<ScrollView>(Resource.Id.ScrollView));

        #endregion
    }
}