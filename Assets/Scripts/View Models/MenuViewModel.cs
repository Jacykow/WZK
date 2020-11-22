using UniRx;
using UnityEngine;

public class MenuViewModel : MonoBehaviour
{
    public FieldContainer fieldContainer;

    private void Start()
    {
        fieldContainer.AddButton("Secret Trivial").OnClickAsObservable()
            .Subscribe(_ =>
            {
                ViewManager.main.CurrentView = ViewConfig.Views.SecretTrivial;
            })
            .AddTo(this);

        fieldContainer.AddButton("Secret Shamir").OnClickAsObservable()
            .Subscribe(_ =>
            {
                ViewManager.main.CurrentView = ViewConfig.Views.SecretShamir;
            })
            .AddTo(this);

        fieldContainer.AddButton("BBS").OnClickAsObservable()
            .Subscribe(_ =>
            {
                ViewManager.main.CurrentView = ViewConfig.Views.BBS;
            })
            .AddTo(this);
    }
}
