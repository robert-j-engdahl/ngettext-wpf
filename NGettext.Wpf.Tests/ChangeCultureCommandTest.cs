using System.Windows.Input;
using NSubstitute;
using Xunit;

namespace NGettext.Wpf.Tests
{
    public class ChangeCultureCommandTest
    {
        private readonly ChangeCultureCommand _target;
        private readonly ICultureTracker _cultureTracker = Substitute.For<ICultureTracker>();

        public ChangeCultureCommandTest()
        {
            _target = new ChangeCultureCommand();
            ChangeCultureCommand.CultureTracker = _cultureTracker;
        }

        [Fact]
        public void Implements_ICommand()
        {
            Assert.IsAssignableFrom<ICommand>(_target);
        }

        [Theory, InlineData("da-DK"), InlineData("en-US")]
        public void CanExecute_If_Parameter_Is_Supported_Culture(string culture)
        {
            Assert.True(_target.CanExecute(culture));
        }

        [Theory, InlineData("bad culture")]
        public void Cannot_Execute_If_Parameter_Is_Not_Supported_Culture(string culture)
        {
            Assert.False(_target.CanExecute(culture));
        }

        [Theory, InlineData("da-DK"), InlineData("en-US")]
        public void Execute_Sets_CurrentCulture_From_Parameter(string culture)
        {
            _target.Execute(culture);
            Assert.Equal(culture, _cultureTracker.CurrentCulture.Name);
        }
    }
}