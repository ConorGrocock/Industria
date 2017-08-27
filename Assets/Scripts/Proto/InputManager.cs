using UnityEngine;

public class InputManager : MonoBehaviourSingleton<InputManager>
{
    [Header("Camera Settings")]
    public float cameraMoveSpeed = 1.0f;
    public float cameraZoomSpeed = 0.5f;
    public float minimumZoomSize = 1.0f;
    public float maximumZoomSize = 8.0f;

    [Header("Time Settings")]
    public float changeTimeSpeed = 0.5f;

    /// <summary>
    /// Minimum game time scale
    /// </summary>
    [Range(0.0f, 100.0f)]
    public float minTimeSpeed = 0.0f;

    /// <summary>
    /// Maximum game time scale
    /// </summary>
    [Range(0.0f, 100.0f)]
    public float maxTimeSpeed = 100.0f;

    [Header("Input Keys")]
    public KeyCode cameraMoveUpKey = KeyCode.UpArrow;
    public KeyCode cameraMoveDownKey = KeyCode.DownArrow;
    public KeyCode cameraMoveLeftKey = KeyCode.LeftArrow;
    public KeyCode cameraMoveRightKey = KeyCode.RightArrow;
    public KeyCode cameraZoomOutKey = KeyCode.PageDown;
    public KeyCode cameraZoomInKey = KeyCode.PageUp;
    public KeyCode increaseGameSpeedKey = KeyCode.KeypadPlus;
    public KeyCode decreaseGameSpeedKey = KeyCode.KeypadMinus;

    [Header("Hotkeys")]
    public KeyCode millHotkey = KeyCode.Q;
    public KeyCode mineHotkey = KeyCode.W;
    public KeyCode powerPlantHotkey = KeyCode.E;
    public KeyCode labHotkey = KeyCode.R;

    void Update()
    {
        if (Manager._instance.isGameOver || Manager._instance.isMainMenu || Manager._instance.isPaused) return;

        if (Input.GetKey(cameraMoveUpKey)) if (UtilityManager._instance.GetOrthographicBounds().max.y < (GenWorld._instance.worldHeight + 2) * 1.28) Camera.main.transform.Translate(new Vector3(0, cameraMoveSpeed * Time.deltaTime));
        if (Input.GetKey(cameraMoveDownKey)) if (UtilityManager._instance.GetOrthographicBounds().min.y > -2 * 1.28) Camera.main.transform.Translate(new Vector3(0, -cameraMoveSpeed * Time.deltaTime));
        if (Input.GetKey(cameraMoveLeftKey)) if (UtilityManager._instance.GetOrthographicBounds().min.x > -2 * 1.28) Camera.main.transform.Translate(new Vector3(-cameraMoveSpeed * Time.deltaTime, 0));
        if (Input.GetKey(cameraMoveRightKey)) if (UtilityManager._instance.GetOrthographicBounds().max.x < (GenWorld._instance.worldWidth + 2) * 1.28) Camera.main.transform.Translate(new Vector3(cameraMoveSpeed * Time.deltaTime, 0));
        if (Input.GetKey(cameraZoomOutKey)) Camera.main.orthographicSize += cameraZoomSpeed * Time.deltaTime;
        if (Input.GetKey(cameraZoomInKey)) Camera.main.orthographicSize -= cameraZoomSpeed * Time.deltaTime;

        if (Input.GetKey(increaseGameSpeedKey))
        {
            if ((Time.timeScale + changeTimeSpeed) > maxTimeSpeed) Time.timeScale = maxTimeSpeed;
            else Time.timeScale += changeTimeSpeed;
        }

        if (Input.GetKey(decreaseGameSpeedKey))
        {
            if ((Time.timeScale - changeTimeSpeed) < minTimeSpeed) Time.timeScale = minTimeSpeed;
            else Time.timeScale -= changeTimeSpeed;
        }

        if (Time.timeScale < minTimeSpeed) Time.timeScale = minTimeSpeed;
        if (Time.timeScale > maxTimeSpeed) Time.timeScale = maxTimeSpeed;

        if (Camera.main.orthographicSize < minimumZoomSize)
        {
            Camera.main.orthographicSize = minimumZoomSize;
        }

        if (Camera.main.orthographicSize > maximumZoomSize)
        {
            Camera.main.orthographicSize = maximumZoomSize;
        }
    }
}
