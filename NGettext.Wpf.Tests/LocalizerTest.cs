using System;
using System.Globalization;
using NSubstitute;
using Xunit;

namespace NGettext.Wpf.Tests
{
    public class LocalizerTest
    {
        private readonly ICultureTracker _cultureTracker = Substitute.For<ICultureTracker>();
        private readonly Localizer _target;
        private readonly CultureInfo _initialCulture = new CultureInfo("da-DK");
        private readonly CultureInfo _changedCulture = new CultureInfo("en-US");

        public LocalizerTest()
        {
            _cultureTracker.CurrentCulture.Returns(_initialCulture);
            _target = new Localizer(_cultureTracker, "Locale", "some domain");
        }

        [Fact]
        public void Depends_On_CultureTracker()
        {
            Assert.DependsOn("cultureTracker", () => new Localizer(null, "Locale", "some domain"));
        }

        [Fact]
        public void Catalog_Is_Initialized_From_CultureTracker_CurrentCulture()
        {
            Assert.Same(_cultureTracker.CurrentCulture, Assert.IsAssignableFrom<Catalog>(_target.Catalog).CultureInfo);
        }

        [Fact]
        public void Catalog_Is_Reset_When_CultureTracker_CultureChanging()
        {
            _cultureTracker.CultureChanging +=
                Raise.Event<EventHandler<CultureEventArgs>>(new CultureEventArgs(_changedCulture));
            Assert.Same(_changedCulture, Assert.IsAssignableFrom<Catalog>(_target.Catalog).CultureInfo);
        }

        [Fact]
        public void Is_Disposable()
        {
            Assert.IsAssignableFrom<IDisposable>(_target);
        }

        [Fact]
        public void Catalog_Is_Not_Reset_When_CultureTracker_CultureChanging_After_Disposal()
        {
            _target.Dispose();
            _cultureTracker.CultureChanging +=
                Raise.Event<EventHandler<CultureEventArgs>>(new CultureEventArgs(_changedCulture));
            Assert.Same(_initialCulture, Assert.IsAssignableFrom<Catalog>(_target.Catalog).CultureInfo);
        }

        [Fact]
        public void CultureTracker_Is_Injected()
        {
            Assert.Same(_cultureTracker, _target.CultureTracker);
        }
    }
}