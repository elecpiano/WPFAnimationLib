using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace Animation
{
    public abstract class AnimationBase
    {
        #region Constructor

        public AnimationBase()
        {
        }

        #endregion

        #region Properties

        protected Storyboard _Storyboard = null;
        protected FrameworkElement AnimationTarget = null;

        #endregion

        #region Animation

        public virtual void Stop()
        {
            this._Storyboard.Stop();
        }

        #endregion

        #region Events

        protected Action<FrameworkElement> AnimationCompleted = null;

        #endregion

    }

    public class AnimationHelper
    {
        private static object obj = new object();
        public static void EnsureTransform(FrameworkElement cell)
        {
            lock (obj)
            {
                TransformGroup transformGroup = cell.RenderTransform as TransformGroup;
                if (transformGroup == null)
                {
                    cell.RenderTransform = transformGroup = new TransformGroup();
                    transformGroup.Children.Add(new TranslateTransform());
                    transformGroup.Children.Add(new ScaleTransform());
                    transformGroup.Children.Add(new RotateTransform());
                    transformGroup.Children.Add(new SkewTransform());
                    
                }

                if (cell.RenderTransformOrigin == null)
                {
                    cell.RenderTransformOrigin = new Point(0.5d, 0.5d);
                }
            }
        }
    }
}
