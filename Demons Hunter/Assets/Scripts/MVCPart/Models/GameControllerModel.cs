using System.Collections.Generic;

public class GameControllerModel
{
    private readonly List<IFixedUpdateble> _fixedControllers;
    private readonly List<IUpdateble> _updateControllers;
    private readonly List<IClearable> _clearableControllers;
    public GameControllerModel()
    {
        _updateControllers = new List<IUpdateble>(8);
        _fixedControllers = new List<IFixedUpdateble>(8);
        _clearableControllers = new List<IClearable>(8);
    }

    public List<IUpdateble> UpdateControllers => _updateControllers;
    public List<IFixedUpdateble> FixedControllers => _fixedControllers;
    public List<IClearable> ClearableControllers => _clearableControllers;
}
