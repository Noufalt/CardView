using Foundation;
using PanCardView;
using PanCardView.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PanCardView.Enums;
using static Xamarin.Forms.Platform.iOS.Platform;

[assembly: ExportRenderer(typeof(CardsView), typeof(CardsViewRenderer))]
namespace PanCardView.iOS
{
    [Preserve(AllMembers = true)]
    public class CardsViewRenderer : VisualElementRenderer<CardsView>
    {
        private readonly UISwipeGestureRecognizer _leftSwipeGesture;
        private readonly UISwipeGestureRecognizer _rightSwipeGesture;
        private readonly UISwipeGestureRecognizer _upSwipeGesture;
        private readonly UISwipeGestureRecognizer _downSwipeGesture;

        public static void Preserve()
        => Preserver.Preserve();

        public CardsViewRenderer()
        {
            _leftSwipeGesture = new UISwipeGestureRecognizer(OnSwiped)
            {
                Direction = UISwipeGestureRecognizerDirection.Left
            };
            _rightSwipeGesture = new UISwipeGestureRecognizer(OnSwiped)
            {
                Direction = UISwipeGestureRecognizerDirection.Right
            };
            _upSwipeGesture = new UISwipeGestureRecognizer(OnSwiped)
            {
                Direction = UISwipeGestureRecognizerDirection.Up
            };
            _downSwipeGesture = new UISwipeGestureRecognizer(OnSwiped)
            {
                Direction = UISwipeGestureRecognizerDirection.Down
            };
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CardsView> e)
        {
            base.OnElementChanged(e);
            if(e.OldElement != null)
            {
                e.OldElement.PlatformSpecificSwipeHandled -= OnPlatformSpecificSwipeHandled;
            }
            if (e.NewElement != null)
            {
                e.NewElement.PlatformSpecificSwipeHandled += OnPlatformSpecificSwipeHandled;
                SetSwipeGestures();
            }
        }

        protected virtual void ResetSwipeGestureRecognizer(UISwipeGestureRecognizer swipeGestureRecognizer)
        {
            RemoveGestureRecognizer(swipeGestureRecognizer);
            AddGestureRecognizer(swipeGestureRecognizer);
        }

        protected void SetSwipeGestures()
        {
            ResetSwipeGestureRecognizer(_leftSwipeGesture);
            ResetSwipeGestureRecognizer(_rightSwipeGesture);
            ResetSwipeGestureRecognizer(_upSwipeGesture);
            ResetSwipeGestureRecognizer(_downSwipeGesture);
        }

        private void OnSwiped(UISwipeGestureRecognizer gesture)
        {
            var swipeDirection = gesture.Direction == UISwipeGestureRecognizerDirection.Left
                                ? ItemSwipeDirection.Left
                                : gesture.Direction == UISwipeGestureRecognizerDirection.Right
                                    ? ItemSwipeDirection.Right
                                    : gesture.Direction == UISwipeGestureRecognizerDirection.Up
                                        ? ItemSwipeDirection.Up
                                        : ItemSwipeDirection.Down;

            Element?.OnSwiped(swipeDirection);
        }

        private void OnPlatformSpecificSwipeHandled(bool isSideSwipe, bool isNextSwipe)
        {
            BeginAnimations(nameof(CardsView));
            SetAnimationDuration(PlatformConfiguration.iOSSpecific.CardsView.GetAutoAnimationDuration(Element).TotalSeconds);
            var transitionType = isSideSwipe
                ? isNextSwipe
                    ? UIViewAnimationTransition.FlipFromLeft
                    : UIViewAnimationTransition.FlipFromRight
                : isNextSwipe
                    ? UIViewAnimationTransition.CurlUp
                    : UIViewAnimationTransition.CurlDown;

            SetAnimationTransition(transitionType, this, true);
            CommitAnimations();
        }
    }
}
