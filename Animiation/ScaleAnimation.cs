using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animation
{
    public class ScaleAnimation : AnimationBase
    {
        #region Constructor

        public ScaleAnimation(int indexOfTragetTransform)
        {
            Init(indexOfTragetTransform);
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

        private void Init(int indexOfTragetTransform)
        {
            _Storyboard = new Storyboard();
            _Storyboard.Completed += _Storyboard_Completed;

            /***animation x***/
            _Animation_X = new DoubleAnimationUsingKeyFrames();

            /*key frame 1*/
            _KeyFrame_x_from = new EasingDoubleKeyFrame();
            _KeyFrame_x_from.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            _KeyFrame_x_from.Value = 0;
            _Animation_X.KeyFrames.Add(_KeyFrame_x_from);

            /*key frame 2*/
            _KeyFrame_x_to = new EasingDoubleKeyFrame();
            _KeyFrame_x_to.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1));
            _KeyFrame_x_to.Value = 1;
            _Animation_X.KeyFrames.Add(_KeyFrame_x_to);

            Storyboard.SetTargetProperty(_Animation_X, new PropertyPath(String.Format("(UIElement.RenderTransform).(TransformGroup.Children)[{0}].(ScaleTransform.ScaleX)", indexOfTragetTransform.ToString())));
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

            Storyboard.SetTargetProperty(_Animation_Y, new PropertyPath(String.Format("(UIElement.RenderTransform).(TransformGroup.Children)[{0}].(ScaleTransform.ScaleY)", indexOfTragetTransform.ToString())));
            _Storyboard.Children.Add(_Animation_Y);
        }

        public static ScaleAnimation ScaleFromTo(FrameworkElement cell,
                double from_x, double from_y,
                double to_x, double to_y,
                TimeSpan duration, Action<FrameworkElement> completed)
        {
            ScaleAnimation animation = CreateNewAnimation(cell);
            animation.InstanceScaleFromTo(cell, from_x, from_y, to_x, to_y, duration, completed);
            return animation;
        }

        public static ScaleAnimation ScaleTo(FrameworkElement cell, double targetX, double targetY, TimeSpan duration, Action<FrameworkElement> completed)
        {
            ScaleAnimation animation = CreateNewAnimation(cell);
            animation.InstanceScaleTo(cell, targetX, targetY, duration, completed);
            return animation;
        }

        private static ScaleAnimation CreateNewAnimation(FrameworkElement cell)
        {
            AnimationHelper.EnsureTransform(cell);

            int indexOfTragetTransform = 0;
            TransformGroup transformGroup = cell.RenderTransform as TransformGroup;

            bool hasScaleTransform = false;
            foreach (Transform item in transformGroup.Children)
            {
                if (item is ScaleTransform)
                {
                    hasScaleTransform = true;
                    break;
                }
                indexOfTragetTransform++;
            }

            if (!hasScaleTransform)
            {
                transformGroup.Children.Add(new ScaleTransform());
            }

            return new ScaleAnimation(indexOfTragetTransform);
        }

        public void InstanceScaleFromTo(FrameworkElement cell,
                double from_x, double from_y,
                double to_x, double to_y,
                TimeSpan duration, Action<FrameworkElement> completed)
        {
            this.Animate(cell, duration, from_x, from_y, to_x, to_y, completed);
        }

        public void InstanceScaleTo(FrameworkElement cell, double targetX, double targetY, TimeSpan duration, Action<FrameworkElement> completed)
        {
            TransformGroup transformGroup = cell.RenderTransform as TransformGroup;
            ScaleTransform scaleTransform = null;

            foreach (Transform item in transformGroup.Children)
            {
                if (item is ScaleTransform)
                {
                    scaleTransform = item as ScaleTransform;
                    break;
                }
            }

            if (scaleTransform == null)
            {
                scaleTransform = new ScaleTransform();
                transformGroup.Children.Add(scaleTransform);
            }

            this.Animate(cell, duration, scaleTransform.ScaleX, scaleTransform.ScaleY, targetX, targetY, completed);
        }

        private void Animate(FrameworkElement cell, TimeSpan duration, double fromX, double fromY, double targetX, double targetY, Action<FrameworkElement> completed)
        {
            AnimationTarget = cell;
            AnimationCompleted = completed;
            TargetX = targetX;
            TargetY = targetY;

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

            //*value*/
            _KeyFrame_x_from.Value = fromX;
            _KeyFrame_x_to.Value = targetX;
            _KeyFrame_y_from.Value = fromY;
            _KeyFrame_y_to.Value = targetY;

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
                    if (item is ScaleTransform)
                    {
                        item.SetValue(ScaleTransform.ScaleXProperty, TargetX);
                        item.SetValue(ScaleTransform.ScaleYProperty, TargetY);
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
