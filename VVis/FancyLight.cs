using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace VVis
{
    public sealed class FancyLight : XamlLight
    {
        // Register an attached proeprty that enables apps to set a UIElement or Brush as a target for this light type in markup.
        public static readonly DependencyProperty IsTargetProperty =
            DependencyProperty.RegisterAttached(
            "IsTarget",
            typeof(Boolean),
            typeof(FancyLight),
            new PropertyMetadata(null, OnIsTargetChanged)
        );

        public static void SetIsTarget(DependencyObject target, Boolean value)
        {
            target.SetValue(IsTargetProperty, value);
        }
        public static Boolean GetIsTarget(DependencyObject target)
        {
            return (Boolean)target.GetValue(IsTargetProperty);
        }

        // Handle attached property changed to automatically target and untarget UIElements and Brushes.    
        private static void OnIsTargetChanged(DependencyObject obj,
                                                DependencyPropertyChangedEventArgs e)
        {
            var isAdding = (Boolean)e.NewValue;

            if (isAdding)
            {
                if (obj is UIElement)
                {
                    XamlLight.AddTargetElement(GetIdStatic(), obj as UIElement);
                }
                else if (obj is Brush)
                {
                    XamlLight.AddTargetBrush(GetIdStatic(), obj as Brush);
                }
            }
            else
            {
                if (obj is UIElement)
                {
                    XamlLight.RemoveTargetElement(GetIdStatic(), obj as UIElement);
                }
                else if (obj is Brush)
                {
                    XamlLight.RemoveTargetBrush(GetIdStatic(), obj as Brush);
                }
            }
        }

        protected override void OnConnected(UIElement newElement)
        {
            // OnConnected is called when the first target UIElement is shown on the screen. This enables delaying composition object creation until it's actually necessary.
            var light = Window.Current.Compositor.CreateAmbientLight();
            light.Color = Colors.Yellow;
            CompositionLight = light;
        }

        protected override void OnDisconnected(UIElement oldElement)
        {
            // OnDisconnected is called when there are no more target UIElements on the screen. The CompositionLight should be disposed when no longer required.
            CompositionLight.Dispose();
            CompositionLight = null;
        }

        protected override string GetId()
        {
            return GetIdStatic();
        }

        private static string GetIdStatic()
        {
            // This specifies the unique name of the light. In most cases you should use the type's FullName.
            return typeof(FancyLight).FullName;
        }
    }
}
