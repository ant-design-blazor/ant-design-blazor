namespace AntDesign.TestApp.Maui.Pages.Form
{
    public partial class StepForm
    {
        private int _current;

        public void Next()
        {
            // todo: Not re-rendered
            _current += 1;
            StateHasChanged();
        }

        public void Prev()
        {
            // todo: Not re-rendered
            if (_current <= 0) return;
            _current -= 1;
            StateHasChanged();
        }
    }
}