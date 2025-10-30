using UnityEngine;

public class MouseInputManager : MonoBehaviour
{
    [SerializeField] private Stage stage;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }
    }

    public void HandleMouseClick()
    {
        if (stage != null)
        {
            Vector3 mousePos = Input.mousePosition;
            int targetTileId = stage.ScreenPosToTileId(mousePos);
            stage.MovePlayerToTile(targetTileId);
        }
    }
}