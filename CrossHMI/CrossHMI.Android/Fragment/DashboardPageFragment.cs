using CrossHMI.Shared.ViewModels;
using NavigationLib.Android.Navigation;

namespace CrossHMI.Android.Fragment
{
    public class DashboardPageFragment : FragmentBase<DashboardViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.dashboard_page;

        protected override void InitBindings()
        {
        }
    }
}