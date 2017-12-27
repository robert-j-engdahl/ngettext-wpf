using System;
using System.Globalization;

namespace NGettext.Wpf
{
    public interface ICultureTracker
    {
        event EventHandler<CultureEventArgs> CultureChanged;
        event EventHandler<CultureEventArgs> CultureChanging;
        CultureInfo CurrentCulture { get; set; }
    }

    public class CultureTracker : ICultureTracker
    {
        private CultureInfo _currentCulture = CultureInfo.CurrentUICulture;
        public event EventHandler<CultureEventArgs> CultureChanged;

        public CultureInfo CurrentCulture
        {
            get { return _currentCulture; }
            set
            {
                CultureChanging?.Invoke(this, new CultureEventArgs(value));
                _currentCulture = value;
                RaiseCultureChanged();
            }
        }

        protected virtual void RaiseCultureChanged()
        {
            CultureChanged?.Invoke(this, new CultureEventArgs(CurrentCulture));
        }

        public event EventHandler<CultureEventArgs> CultureChanging;
    }
}