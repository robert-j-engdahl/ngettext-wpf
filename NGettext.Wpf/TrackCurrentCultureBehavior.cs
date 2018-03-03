using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using EventArgs = System.EventArgs;
using System.Windows.Interactivity;

namespace NGettext.Wpf
{
    /// <summary>
    /// Makes sure that the CultureInfo used for all binding operations inside the associated
    /// FrameworkElement follows the CurrentCulture of the CultureTracker injected to the static
    /// CultureTracker property.
    ///
    /// For instance, dates and numbers bound with a culture specific StringFormat will be formatted
    /// according to the tracked culture and even reformatted on culture changed.
    /// </summary>
    public class TrackCurrentCultureBehavior : Behavior<FrameworkElement>, IWeakCultureObserver
    {
        /// <summary>
        /// This property is subject to static property injection and must be set before instances
        /// are attached to any FrameworkElement.
        /// </summary>
        public static ICultureTracker CultureTracker { get; set; }

        protected override void OnAttached()
        {
            if (!DesignerProperties.GetIsInDesignMode(AssociatedObject))
            {
                if (CultureTracker is null)
                {
                    throw new Exception("TrackCurrentCultureBehavior.CultureTracker must be initialized");
                }
                CultureTracker.AddWeakCultureObserver(this);
                UpdateAssociatedObjectCulture();
            }

            base.OnAttached();
        }

        private void UpdateAssociatedObjectCulture()
        {
            AssociatedObject.Language = XmlLanguage.GetLanguage(CultureTracker.CurrentCulture.IetfLanguageTag);
        }

        public void HandleCultureChanged(ICultureTracker sender, CultureEventArgs eventArgs)
        {
            UpdateAssociatedObjectCulture();
        }
    }
}