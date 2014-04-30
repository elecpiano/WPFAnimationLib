using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace Animation
{
    public class MoveAnimation : AnimationBase
    {
        #region Constructor

        public MoveAnimation(int indexOfTargetTransform)
        {
            Init(indexOfTargetTransform);
        }

        #endregion

        #region Properties

        private DoubleAnimationUsingKeyFrames _Animation_X = null;
        private DoubleAnimationUsingKeyFrames _Animation_Y = null;

        private EasingDoubleKeyFrame _KeyFrame_x_from = null;
        private EasingDoubleKeyFrame _KeyFrame_x_to = null;
        private EasingDoubleKeyFrame _KeyFrame_y_from = null;
        private EasingDoubleKeyFrame _KeyFrame_y_to = null;

        private double TargetX = 0;
        private double TargetY = 0;

        #endregion

        #region Animation

        private void Init(int indexOfTargetTransform)
        {
            _Storyboard = new Storyboard();
            _Storyboard.Completed += _Storyboard_Completed;

            /***animation x***/
            _Animation_X = new DoubleAnimationUsingKeyFrames();

            /*key frame x from*/
            _KeyFrame_x_from = new EasingDoubleKeyFrame();
            _KeyFrame_x_from.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            _KeyFrame_x_from.Value = 0;
            _Animation_X.KeyFrames.Add(_KeyFrame_x_from);

            /*key frame x to*/
            _KeyFrame_x_to = new EasingDoubleKeyFrame();
            _KeyFrame_x_to.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            _KeyFrame_x_to.Value = 1;
            _Animation_X.KeyFrames.Add(_KeyFrame_x_to);

            Storyboard.SetTargetProperty(_Animation_X, new PropertyPath(String.Format("(UIElement.RenderTransform).(TransformGroup.Children)[{0}].(TranslateTransform.X)", indexOfTargetTransform.ToString())));
            _Storyboard.Children.Add(_Animation_X);


            /***animation y***/
            _Animation_Y = new DoubleAnimationUsingKeyFrames();

            /*key frame 1*/
            _KeyFrame_y_from = new EasingDoubleKeyFrame();
            _KeyFrame_y_from.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            _KeyFrame_y_from.Value = 0;
            _Animation_Y.KeyFrames.Add(_KeyFrame_y_from);

            /*key frame 2*/
            _KeyFrame_y_to = new EasingDoubleKeyFrame();
            _KeyFrame_y_to.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1));
            _KeyFrame_y_to.Value = 1;
            _Animation_Y.KeyFrames.Add(_KeyFrame_y_to);

            Storyboard.SetTargetProperty(_Animation_Y, new PropertyPath(String.Format("(UIElement.RenderTransform).(TransformGroup.Children)[{0}].(TranslateTransform.Y)", indexOfTargetTransform.ToString())));
            _Storyboard.Children.Add(_Animation_Y);
        }

        public static MoveAnimation MoveTo(FrameworkElement cell, double x, double y, TimeSpan duration, IEasingFunction easing = null, Action<FrameworkElement> completed = null)
        {
            MoveAnimation animation = CreateNewAnimation(cell);
            animation.InstanceMoveTo(cell, x, y, duration, easing, completed);
            return animation;
        }

        public static MoveAnimation MoveBy(FrameworkElement cell, double x, double y, TimeSpan duration, IEasingFunction easing = null, Action<FrameworkElement> completed = null)
        {
            MoveAnimation animation = CreateNewAnimation(cell);
            animation.InstanceMoveBy(cell, x, y, duration, easing, completed);
            return animation;
        }

        public static MoveAnimation MoveFromTo(FrameworkElement cell,
            double from_x, double from_y,
            double to_x, double to_y,
            TimeSpan duration,
            IEasingFunction easing = null,
            Action<FrameworkElement> completed = null)
        {
            MoveAnimation animation = CreateNewAnimation(cell);
            animation.InstanceMoveFromTo(cell, from_x, from_y, to_x, to_y, duration, easing, completed);
            return animation;
        }

        private static MoveAnimation CreateNewAnimation(FrameworkElement cell)
        {
            int indexOfTargetTransform = 0;
            AnimationHelper.EnsureTransform(cell);

            TransformGroup transformGroup = cell.RenderTransform as TransformGroup;
            bool hasTranslateTransform = false;
            foreach (Transform item in transformGroup.Children)
            {

                if (item is TranslateTransform)
                {
                    hasTranslateTransform = true;
                    break;
                }
                indexOfTargetTransform++;
            }

            if (!hasTranslateTransform)
            {
                transformGroup.Children.Add(new TranslateTransform());
            }

            return new MoveAnimation(indexOfTargetTransform);
        }

        public void InstanceMoveBy(FrameworkElement cell, double x, double y, TimeSpan duration, IEasingFunction easing = null, Action<FrameworkElement> completed = null)
        {
            /*value*/
            TransformGroup transformGroup = cell.RenderTransform as TransformGroup;
            TranslateTransform translateTransform = null;

            foreach (Transform item in transformGroup.Children)
            {
                if (item is TranslateTransform)
                {
                    translateTransform = item as TranslateTransform;
                    break;
                }
            }

            if (translateTransform == null)
            {
                translateTransform = new TranslateTransform();
                transformGroup.Children.Add(translateTransform);
            }


            var fromX = translateTransform.X;
            var toX = fromX + x;
            var fromY = translateTransform.Y;
            var toY = fromY + y;

            this.Animate(cell, fromX, fromY, toX, toY, duration, easing, completed);
        }

        public void InstanceMoveFromTo(FrameworkElement cell,
            double from_x, double from_y,
            double to_x, double to_y,
            TimeSpan duration,
            IEasingFunction easing = null,
            Action<FrameworkElement> completed = null)
        {

            TransformGroup transformGroup = cell.RenderTransform as TransformGroup;
            TranslateTransform translateTransform = null;

            foreach (Transform item in transformGroup.Children)
            {
                if (item is TranslateTransform)
                {
                    translateTransform = item as TranslateTransform;
                    break;
                }
            }

            if (translateTransform == null)
            {
                translateTransform = new TranslateTransform();
                transformGroup.Children.Add(translateTransform);
            }

            this.Animate(cell, from_x, from_y, to_x, to_y, duration, easing, completed);
        }

        public void InstanceMoveTo(FrameworkElement cell, double x, double y, TimeSpan duration, IEasingFunction easing = null, Action<FrameworkElement> completed = null)
        {
            TransformGroup transformGroup = cell.RenderTransform as TransformGroup;
            TranslateTransform translateTransform = null;

            foreach (Transform item in transformGroup.Children)
            {
                if (item is TranslateTransform)
                {
                    translateTransform = item as TranslateTransform;
                    break;
                }
            }

            if (translateTransform == null)
            {
                translateTransform = new TranslateTransform();
                transformGroup.Children.Add(translateTransform);
            }

            this.Animate(cell, translateTransform.X, translateTransform.Y, x, y, duration, easing, completed);
        }

        private void Animate(FrameworkElement cell, double fromX, double fromY, double ToX, double ToY, TimeSpan duration, IEasingFunction easing, Action<FrameworkElement> completed)
        {
            AnimationTarget = cell;
            TargetX = ToX;
            TargetY = ToY;
            AnimationCompleted = completed;

            if (_Storyboard == null)
            {
                Init(0);
            }
            else
            {
                _Storyboard.Stop();
            }

            /*time*/
            _KeyFrame_x_to.KeyTime = KeyTime.FromTimeSpan(duration);
            _KeyFrame_y_to.KeyTime = KeyTime.FromTimeSpan(duration);

            /*value*/
            _KeyFrame_x_from.Value = fromX;
            _KeyFrame_x_to.Value = ToX;
            _KeyFrame_y_from.Value = fromY;
            _KeyFrame_y_to.Value = ToY;

            /*easing*/
            _KeyFrame_x_to.EasingFunction = easing;
            _KeyFrame_y_to.EasingFunction = easing;

            Storyboard.SetTarget(_Animation_X, AnimationTarget);
            Storyboard.SetTarget(_Animation_Y, AnimationTarget);

            _Storyboard.Begin();
        }

        private void _Storyboard_Completed(object sender, object e)
        {
            TransformGroup transformGroup = AnimationTarget.RenderTransform as TransformGroup;
            if (transformGroup != null)
            {
                foreach (Transform item in transformGroup.Children)
                {
                    if (item is TranslateTransform)
                    {
                        item.SetValue(TranslateTransform.XProperty, TargetX);
                        item.SetValue(TranslateTransform.YProperty, TargetY);
                        break;
                    }
                }
            }

            if (AnimationCompleted != null)
            {
                AnimationCompleted(AnimationTarget);
            }
        }

        #endregion

    }
}
