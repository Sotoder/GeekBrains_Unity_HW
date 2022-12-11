public class UIAudioController
{
    private UIAudioControllerModel _uiAudioControllerModel;

    public UIAudioController(UIAudioControllerModel uiAudioControllerModel)
    {
        _uiAudioControllerModel= uiAudioControllerModel;
    }

    public void SiginButtons()
    {
        var buttonList = _uiAudioControllerModel.ButtonList;

        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].onClick.AddListener(PlayClick);
            buttonList[i].OnPointerEnterEvent += PlayPointerEnter;
        }
    }

    private void PlayPointerEnter()
    {
        _uiAudioControllerModel.MenuAudioSource.clip = _uiAudioControllerModel.OnSelectClip;
        _uiAudioControllerModel.MenuAudioSource.Play();
    }

    private void PlayClick()
    {
        _uiAudioControllerModel.MenuAudioSource.clip = _uiAudioControllerModel.OnClickClip;
        _uiAudioControllerModel.MenuAudioSource.Play();
    }

    public void Clear()
    {
        var buttonList = _uiAudioControllerModel.ButtonList;

        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].onClick.RemoveListener(PlayClick);
            buttonList[i].OnPointerEnterEvent -= PlayPointerEnter;
        }
    }
}
