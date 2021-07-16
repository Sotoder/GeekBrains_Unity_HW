using UnityEngine;

public class EndLevelDoorGUI : MonoBehaviour
{
    [SerializeField] private GameObject _endLevelDoor;

    private GUIStyle _guiStyle = new GUIStyle();

    private EndLevelDoor _door;

    private void Awake()
    {
        _door =  _endLevelDoor.GetComponent<EndLevelDoor>();
    }
    private void OnGUI()
    {
        if (_door.IsPlayerNear)
        {


            if (_door.IsAllLeversDown)
            {
                GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 100), "");
                _guiStyle.normal.textColor = Color.white;
                _guiStyle.fontStyle = FontStyle.Bold;
                _guiStyle.alignment = TextAnchor.MiddleCenter;
                _guiStyle.wordWrap = true;

                GUI.Label(new Rect(Screen.width / 2 - 95, Screen.height / 2 - 100, 190, 90), "Путь открыт!", _guiStyle);
            }
            else
            {

                GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 100), "");
                _guiStyle.normal.textColor = Color.white;
                _guiStyle.fontStyle = FontStyle.Bold;
                _guiStyle.alignment = TextAnchor.MiddleCenter;
                _guiStyle.wordWrap = true;

                GUI.Label(new Rect(Screen.width / 2 - 95, Screen.height / 2 - 90, 190, 10), "Дверь заблокирована!", _guiStyle);

                _guiStyle.fontStyle = FontStyle.Normal;
                GUI.Label(new Rect(Screen.width / 2 - 95, Screen.height / 2 - 80, 190, 80), "На востоке и западе ущелья расположены колонны." +
                    "Найди их. Они откроют тебе путь!", _guiStyle);
            }
            
        }
    }
}
