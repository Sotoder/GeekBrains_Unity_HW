public class GameInitializator
{

    public GameInitializator(GameInitalizatorModel gameInitalizatorModel, GameController gameController)
    {
        var timerController = new GameTimerController(gameInitalizatorModel.TimerModel);

        gameController.Add(timerController);
    }
}