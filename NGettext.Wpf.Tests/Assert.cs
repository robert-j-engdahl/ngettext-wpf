using System;
using System.ComponentModel;
using System.Linq;
using NSubstitute;

namespace Xunit
{
    public partial class Assert
    {
        public static void DependsOn(string paramName, Action targetInvocation)
        {
            var e = Throws<ArgumentNullException>(targetInvocation);
            Equal(paramName, e.ParamName);
        }

        public static void DoesNotThrow(Action action)
        {
            var e = Record.Exception(action);
            Null(e);
        }

        public static TInnerException ThrowsArgumentException<TInnerException>(string paramName, Action action)
        {
            var e = Throws<ArgumentException>(action);
            Equal(paramName, e.ParamName);
            return IsAssignableFrom<TInnerException>(e.InnerException);
        }

        public static void NotifiesDataError(string propertyName, INotifyDataErrorInfo target, Action action,
            Predicate<object> validationErrorPredicate)
        {
            var listener = Substitute.For<EventHandler<DataErrorsChangedEventArgs>>();
            listener.WhenForAnyArgs(l => l.Invoke(null, null)).Do(
                callInfo =>
                {
                    if (callInfo.Arg<DataErrorsChangedEventArgs>().PropertyName == propertyName)
                    {
                        Contains(
                            callInfo.Arg<INotifyDataErrorInfo>().GetErrors(propertyName).Cast<object>(),
                            validationErrorPredicate);
                    }
                });
            target.ErrorsChanged += listener;
            action.Invoke();
            True(target.HasErrors);
            target.ErrorsChanged -= listener;
            listener.Received()
                .Invoke(Arg.Is(target), Arg.Is<DataErrorsChangedEventArgs>(args => args.PropertyName == propertyName));
        }

        public static void NoDataError(string propertyName, INotifyDataErrorInfo target, Action action)
        {
            var listener = Substitute.For<EventHandler<DataErrorsChangedEventArgs>>();
            target.ErrorsChanged += listener;
            action.Invoke();
            target.ErrorsChanged -= listener;
            listener.DidNotReceive()
                .Invoke(Arg.Is(target), Arg.Is<DataErrorsChangedEventArgs>(args => args.PropertyName == propertyName));
            Empty(target.GetErrors(propertyName));
        }

        public static void NotifiesNoDataError(string propertyName, INotifyDataErrorInfo target, Action action)
        {
            var listener = Substitute.For<EventHandler<DataErrorsChangedEventArgs>>();
            target.ErrorsChanged += listener;
            action.Invoke();
            target.ErrorsChanged -= listener;
            listener.Received()
                .Invoke(Arg.Is(target), Arg.Is<DataErrorsChangedEventArgs>(args => args.PropertyName == propertyName));
            Empty(target.GetErrors(propertyName));
        }

        public static void NotifiesPropertyChanged(INotifyPropertyChanged target, string propertyName, Action action)
        {
            var listener = Substitute.For<PropertyChangedEventHandler>();
            target.PropertyChanged += listener;
            try
            {
                action.Invoke();
            }
            finally
            {
                target.PropertyChanged -= listener;
            }
            listener.Received()
                .Invoke(Arg.Is(target), Arg.Is<PropertyChangedEventArgs>(e => e.PropertyName == propertyName));
        }
    }
}