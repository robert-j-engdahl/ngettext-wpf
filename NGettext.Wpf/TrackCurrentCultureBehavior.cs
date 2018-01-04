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
    public class TrackCurrentCultureBehavior : Behavior<FrameworkElement>
    {
        private bool _isTrackingCurrentCulture;

        /// <summary>
        /// This property is subject to static property injection and must be set before instances 
        /// are attached to any FrameworkElement.
        /// </summary>
        public static ICultureTracker CultureTracker { get; set; }

        protected override void OnAttached()
        {
            if (!DesignerProperties.GetIsInDesignMode(AssociatedObject) && CultureTracker != null)
            {
                AllowGarbageCollectionWhileUnloaded();
                TrackCurrentCulture();
            }

            base.OnAttached();
        }

        private void AllowGarbageCollectionWhileUnloaded()
        {
            // Two objects may hold a reference to this behavior - the AssociatedObject and the 
            // CultureTracker.  The intended lifetime scope of the CultureTracker is singleton, 
            // but the lifetime of the AssociatedObject might be shorter.  Therefore we need to
            // make sure the CultureTracker is not preventing this behavior and therethrough
            // the AssociatedObject from being garbage collected.  The solution is to allow
            // garbage collection while the AssociatedObject is unloaded.
            AssociatedObject.Unloaded += IgnoreCurrentCulture;
            AssociatedObject.Loaded += TrackCurrentCulture;
        }

        private void TrackCurrentCulture(object sender = null, RoutedEventArgs e = null)
        {
            if (_isTrackingCurrentCulture) return;
            _isTrackingCurrentCulture = true;
            CultureTracker.CultureChanged += UpdateAssociatedObjectCulture;
            UpdateAssociatedObjectCulture();
        }

        private void IgnoreCurrentCulture(object sender, EventArgs e)
        {
            if (!_isTrackingCurrentCulture) return;
            _isTrackingCurrentCulture = false;
            CultureTracker.CultureChanged -= UpdateAssociatedObjectCulture;
        }

        private void UpdateAssociatedObjectCulture(object sender = null, EventArgs e = null)
        {
            AssociatedObject.Language = XmlLanguage.GetLanguage(CultureTracker.CurrentCulture.IetfLanguageTag);
        }
    }
}