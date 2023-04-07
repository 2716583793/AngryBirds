using UnityEngine;

/// <summary>
/// 鼠标监听
/// </summary>
public class CursorChange : MonoBehaviour
{
    public Texture2D cursorUp;
    public Texture2D cursorDown;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // print("down");
            Cursor.SetCursor(cursorDown, Vector2.zero, CursorMode.Auto);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            // print("up");
            Cursor.SetCursor(cursorUp, Vector2.zero, CursorMode.Auto);
        }
    }
}
