using System;
using System.Globalization;
using NSubstitute;
using Xunit;

namespace NGettext.Wpf.Tests
{
    public class CultureTrackerTest
    {
        private readonly CultureTracker _target;

        public CultureTrackerTest()
        {
            _target = new CultureTracker();
        }

        [Fact]
        public void Setting_CurrentCulture_Raise_CultureChanging_And_Then_CultureChanged()
        {
            var culture = new CultureInfo("en-US");
            var cultureChanging = Substitute.For<EventHandler<CultureEventArgs>>();
            _target.CultureChanging += cultureChanging;

            var cultureChanged = Substitute.For<EventHandler<CultureEventArgs>>();
            _target.CultureChanged += cultureChanged;

            _target.CurrentCulture = culture;

            Received.InOrder(() =>
            {
                cultureChanging.Invoke(Arg.Is(_target), Arg.Is<CultureEventArgs>(e => e.CultureInfo == culture));
                cultureChanged.Invoke(Arg.Is(_target), Arg.Is<CultureEventArgs>(e => e.CultureInfo == culture));
            });
        }

        [Fact]
        public void Setting_CurrentCulture_Raise_CultureChanged_After_CurrentCulture_Changed()
        {
            var culture = new CultureInfo("en-US");
            _target.CultureChanged += (s, e) => Assert.Same(culture, _target.CurrentCulture);

            _target.CurrentCulture = culture;
        }

        [Fact]
        public void Setting_CurrentCulture_Raise_CultureChanging_Before_CurrentCulture_Changed()
        {
            var culture = new CultureInfo("en-US");
            var oldCulture = _target.CurrentCulture;
            _target.CultureChanging += (s, e) => Assert.Same(oldCulture, _target.CurrentCulture);

            _target.CurrentCulture = culture;
        }

        [Fact]
        public void CurrentCulture_Is_Initially_CurrentUiCulture()
        {
            Assert.Same(CultureInfo.CurrentUICulture, _target.CurrentCulture);
        }

        [Fact]
        public void Setting_CurrentCulture_Notifies_Weak_Culture_Observers()
        {
            var cultureObserver = Substitute.For<IWeakCultureObserver>();
            _target.AddWeakCultureObserver(cultureObserver);

            var culture = new CultureInfo("en-US");

            _target.CurrentCulture = culture;

            cultureObserver.Received().HandleCultureChanged(Arg.Is(_target), Arg.Is<CultureEventArgs>(e => e.CultureInfo == culture));
        }

        [Fact]
        public void Weak_Culture_Observers_May_Be_Garbage_Collected()
        {
            var weakCultureObserver = GetWeakReferenceToObservingCultureObserver();
            GC.Collect();
            Assert.False(weakCultureObserver.TryGetTarget(out var _));
        }

        private WeakReference<IWeakCultureObserver> GetWeakReferenceToObservingCultureObserver()
        {
            var cultureObserver = Substitute.For<IWeakCultureObserver>();
            _target.AddWeakCultureObserver(cultureObserver);
            return new WeakReference<IWeakCultureObserver>(cultureObserver);
        }
    }
}