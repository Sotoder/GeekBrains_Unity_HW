using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltipes : MonoBehaviour
{
    private GUIStyle _guiStyle = new GUIStyle();
    private bool _isPause = false;
    public bool IsPause { set => _isPause = value; }

    private void OnGUI()
    {
        if (!_isPause)
        {
            GUI.Box(new Rect(5, 60, Screen.width / 5, 80), "Подсказки");
            _guiStyle.normal.textColor = Color.white;

            _guiStyle.fontStyle = FontStyle.Bold;
            GUI.Label(new Rect(10, 80, 1, 1), "P", _guiStyle);
            _guiStyle.fontStyle = FontStyle.Normal;
            GUI.Label(new Rect(22, 80, 1, 1), "- изменить цвет", _guiStyle);
            GUI.Label(new Rect(10, 90, 1, 1), "освещения", _guiStyle);

            _guiStyle.fontStyle = FontStyle.Bold;
            GUI.Label(new Rect(10, 110, 1, 1), "Q", _guiStyle);
            _guiStyle.fontStyle = FontStyle.Normal;
            GUI.Label(new Rect(22, 110, 1, 1), "- выбрать оружие", _guiStyle);
        }
    }
}
