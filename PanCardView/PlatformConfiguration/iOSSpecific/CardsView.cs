using System;
using System.Linq;
using PanCardView.Enums;
using PanCardView.Extensions;
using Xamarin.Forms;
using Control = PanCardView.CardsView;

namespace PanCardView.PlatformConfiguration.iOSSpecific
{
    public static class CardsView
    {
        public static readonly BindableProperty AutoAnimationTypeProperty
            = BindableProperty.CreateAttached(nameof(AutoAnimationTypeProperty), typeof(AutoAnimationType), typeof(CardsView), default(AutoAnimationType));

        public static readonly BindableProperty AutoAnimationDurationProperty
            = BindableProperty.CreateAttached(nameof(AutoAnimationDurationProperty), typeof(TimeSpan), typeof(CardsView), TimeSpan.FromMilliseconds(900));

        public static AutoAnimationType GetAutoAnimationType(BindableObject bindable)
            => (AutoAnimationType)bindable.GetValue(AutoAnimationTypeProperty);

        public static void SetAutoAnimationType(BindableObject bindable, AutoAnimationType value)
            => bindable.SetValue(AutoAnimationTypeProperty, value);

        public static TimeSpan GetAutoAnimationDuration(BindableObject bindable)
            => (TimeSpan)bindable.GetValue(AutoAnimationDurationProperty);

        public static void SetAutoAnimationDuration(BindableObject bindable, TimeSpan value)
            => bindable.SetValue(AutoAnimationDurationProperty, value);

        internal static bool HandleSwipe(Control control, ItemSwipeDirection direction)
        {
            var isSideSwipe = (int)direction < 2;
            var isNextSwipe = direction == ItemSwipeDirection.Left || direction == ItemSwipeDirection.Up;
            var swipeAnimationType = GetAutoAnimationType(control);
            if (Device.RuntimePlatform != Device.iOS ||
                swipeAnimationType == default(AutoAnimationType) ||
                (isSideSwipe && !swipeAnimationType.HasFlag(AutoAnimationType.Flip)) ||
                (!isSideSwipe && !swipeAnimationType.HasFlag(AutoAnimationType.Curl)) ||
                (isNextSwipe && !control.NextViews.Any()) || (!isNextSwipe && !control.PrevViews.Any()))
            {
                return false;
            }

            if (isSideSwipe && control.IsRightToLeftFlowDirectionEnabled)
            {
                isNextSwipe = !isNextSwipe;
            }

            control.IsSideSwipe = isSideSwipe;
            control.SelectedIndex = (control.SelectedIndex + (isNextSwipe ? 1 : -1)).ToCyclingIndex(control.ItemsCount);

            return true;
        }
    }
}
