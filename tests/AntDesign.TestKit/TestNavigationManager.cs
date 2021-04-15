using Microsoft.AspNetCore.Components;

namespace AntDesign.Tests
{
    public class TestNavigationManager : NavigationManager
    {
        public delegate void NavigatedCallback(string uri, bool forceLoad);
        
        public TestNavigationManager()
        {
            Initialize("http://localhost/", "http://localhost/");
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            Navigated?.Invoke(uri, forceLoad);
        }

        public event NavigatedCallback Navigated;
    }
}
