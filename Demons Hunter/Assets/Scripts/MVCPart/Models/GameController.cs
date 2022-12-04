public class GameController : IController
{
    private GameControllerModel _model;

    public GameController()
    {
        _model = new GameControllerModel();
    }

    public void Add(IController controller)
    {
        if (controller is IUpdateble executeController)
        {
            _model.UpdateControllers.Add(executeController);
        }

        if (controller is IFixedUpdateble fixedController)
        {
            _model.FixedControllers.Add(fixedController);
        }
    }

    public void Update(float deltaTime)
    {
        for (var element = 0; element < _model.UpdateControllers.Count; ++element)
        {
            _model.UpdateControllers[element].Update(deltaTime);
        }
    }

    public void FixedUpdate(float fixedDeltaTime)
    {
        for (var element = 0; element < _model.FixedControllers.Count; ++element)
        {
            _model.FixedControllers[element].FixedUpdate(fixedDeltaTime);
        }
    }
}
