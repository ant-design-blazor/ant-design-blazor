namespace AntDesign
{
    public partial class ActionColumn : ColumnBase
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (IsHeader)
            {
                Context.HeaderColumnInitialed(this);
            }
        }
    }
}
