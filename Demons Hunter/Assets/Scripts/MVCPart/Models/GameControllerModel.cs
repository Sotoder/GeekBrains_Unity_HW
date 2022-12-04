using System.Collections.Generic;

public class GameControllerModel
{
    private readonly List<IFixedUpdateble> _fixedControllers;
    private readonly List<IUpdateble> _updateControllers;
    public GameControllerModel()
    {
        _updateControllers = new List<IUpdateble>(8);
        _fixedControllers = new List<IFixedUpdateble>(8);
    }

    public List<IUpdateble> UpdateControllers => _updateControllers;
    public List<IFixedUpdateble> FixedControllers => _fixedControllers;
}
