using UnityEngine;
using UnityEngine.LightTransport;

public class MouseInputManager : MonoBehaviour
{
    private static MouseInputManager instance;
    public static MouseInputManager Instance => instance;

    [SerializeField] private Stage stage;
    [SerializeField] private Player player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }
    }

    public void HandleMouseClick()
    {
        Vector3 mousePos = Input.mousePosition;

        if (stage != null && player != null)
        {
            int targetTileId = stage.ScreenPosToTileId(mousePos);

            Debug.Log($"¸¶¿ì½º ½ºÅ©¸° ÁÂÇ¥: {mousePos}");
            Debug.Log($"Å¸°Ù Å¸ÀÏ ID: {targetTileId}");
            Debug.Log($"¸Ê Å©±â: {stage.mapWidth} x {stage.mapHeight} = {stage.mapWidth * stage.mapHeight}");

            player.MoveToTile(targetTileId);
        }
    }
}