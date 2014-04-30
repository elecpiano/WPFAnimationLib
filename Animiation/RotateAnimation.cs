using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Animation
{
    public class RotateAnimation : AnimationBase
    {
        #region Constructor

        public RotateAnimation(int indexOfTragetTransform)
        {
            Init(indexOfTragetTransform);
        }

        #endregion

        #region Properties

        private DoubleAnimationUsingKeyFrames _Animation_Angle = null;
        private EasingDoubleKeyFrame _keyFrame_from = null;
        private EasingDoubleKeyFrame _keyFrame_to = null;
        private double TargetAngle = 0;

        #endregion

        #region Animation

        private void Init(int indexOfTragetTransform)
        {
            _Storyboard = new Storyboard();
            _Storyboard.Completed += _Storyboard_Completed;

            /***animation***/
            _Animation_Angle = new DoubleAnimationUsingKeyFrames();

            /*key frame*/
            _keyFrame_from = new EasingDoubleKeyFrame();
            _keyFrame_from.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            _keyFrame_from.Value = 0;
            _Animation_Angle.KeyFrames.Add(_keyFrame_from);

            _keyFrame_to = new EasingDoubleKeyFrame();
            _keyFrame_to.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1));
            _keyFrame_to.Value = 0;
            _Animation_Angle.KeyFrames.Add(_keyFrame_to);

            Storyboard.SetTargetProperty(_Animation_Angle, new PropertyPath(String.Format("(UIElement.RenderTransform).(TransformGroup.Children)[{0}].(RotateTransform.Angle)", indexOfTragetTransform.ToString())));
            _Storyboard.Children.Add(_Animation_Angle);
        }

        public static RotateAnimation RotateFromTo(FrameworkElement cell,
                double from, double to, TimeSpan duration, Action<FrameworkElement> completed)
        {
            RotateAnimation animation = CreateNewAnimation(cell);
            animation.InstanceRotateFromTo(cell, from, to, duration, completed);
            return animation;
        }

        public static RotateAnimation RotateTo(FrameworkElement cell, double to, TimeSpan duration, Action<FrameworkElement> completed)
        {
            RotateAnimation animation = CreateNewAnimation(cell);
            animation.InstanceRotateTo(cell, to, duration, completed);
            return animation;
        }

        private static RotateAnimation CreateNewAnimation(FrameworkElement cell)
        {
            AnimationHelper.EnsureTransform(cell);

            int indexOfTragetTransform = 0;
            TransformGroup transformGroup = cell.RenderTransform as TransformGroup;

            bool hasRotateTransform = false;
            foreach (Transform item in transformGroup.Children)
            {
                if (item is RotateTransform)
                {
                    hasRotateTransform = true;
                    break;
                }
                indexOfTragetTransform++;
            }

            if (!hasRotateTransform)
            {
                transformGroup.Children.Add(new RotateTransform());
            }

            return new RotateAnimation(indexOfTragetTransform);
        }

        public void InstanceRotateFromTo(FrameworkElement cell,
                double from, double to, TimeSpan duration, Action<FrameworkElement> completed)
        {
            this.Animate(cell, duration, from, to, completed);
        }

        public void InstanceRotateTo(FrameworkElement cell, double to, TimeSpan duration, Action<FrameworkElement> completed)
        {
            TransformGroup transformGroup = cell.RenderTransform as TransformGroup;
            RotateTransform rotateTransform = null;

            foreach (Transform item in transformGroup.Children)
            {
                if (item is RotateTransform)
                {
                    rotateTransform = item as RotateTransform;
                    break;
                }
            }

            if (rotateTransform == null)
            {
                rotateTransform = new RotateTransform();
                transformGroup.Children.Add(rotateTransform);
            }

            this.Animate(cell, duration, rotateTransform.Angle, to, completed);
        }

        private void Animate(FrameworkElement cell, TimeSpan duration, double from, double to, Action<FrameworkElement> completed)
        {
            AnimationTarget = cell;
            AnimationCompleted = completed;
            TargetAngle = to;

            if (_Storyboard == null)
            {
                Init(0);
            }
            else
            {
                _Storyboard.Stop();
            }

            /*time*/
            _keyFrame_from.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            _keyFrame_to.KeyTime = KeyTime.FromTimeSpan(duration);
            
            //*value*/
            _keyFrame_from.Value = from;
            _keyFrame_to.Value = to;

            Storyboard.SetTarget(_Animation_Angle, AnimationTarget);

            _Storyboard.Begin();
        }

        private void _Storyboard_Completed(object sender, object e)
        {
            TransformGroup transformGroup = AnimationTarget.RenderTransform as TransformGroup;
            if (transformGroup != null)
            {
                foreach (Transform item in transformGroup.Children)
                {
                    if (item is RotateTransform)
                    {
                        item.SetValue(RotateTransform.AngleProperty, TargetAngle);
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
