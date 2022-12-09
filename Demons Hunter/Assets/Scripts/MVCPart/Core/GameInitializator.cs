public class GameInitializator
{

    public GameInitializator(GameInitalizatorModel gameInitalizatorModel, GameController gameController)
    {
        var timerController = new GameTimerController(gameInitalizatorModel.TimerModel);
        var uiAudioController = new UIAudioController(gameInitalizatorModel.UIAudioControllerModel);

        uiAudioController.SiginButtons();
        gameController.Add(timerController);
    }
}