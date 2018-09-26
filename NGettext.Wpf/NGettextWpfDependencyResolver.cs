namespace NGettext.Wpf
{
    public class NGettextWpfDependencyResolver
    {
        public virtual ICultureTracker ResolveCultureTracker()
        {
            return new CultureTracker();
        }
    }
}