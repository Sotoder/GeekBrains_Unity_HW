public class WeaponController
{
    private IWeaponModel _currentWeaponModel;
    public IWeaponModel CurrentWeaponModel => _currentWeaponModel;

    public WeaponController(PlayerView playerView)
    {
    }

}
