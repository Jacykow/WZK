using UniRx;

public static class UIExtensions
{
    public static void AddMenuButton(this FieldContainer fieldContainer)
    {
        fieldContainer.AddButton("Menu").OnClickAsObservable()
            .Subscribe(_ =>
            {
                ViewManager.main.CurrentView = ViewConfig.Views.Menu;
            })
            .AddTo(fieldContainer);
    }
}
