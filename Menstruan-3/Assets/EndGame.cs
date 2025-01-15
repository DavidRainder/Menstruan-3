using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    private Transform _comicPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CamMovements movement = new CamMovements();

        movement.lastPosition = _comicPosition.position;
        movement.onEnd.AddListener(End);
        movement.onStart.AddListener(AlEmpezar);

        CameraManager.Instance.MoveToSprite(movement);
    }
    private void AlEmpezar()
    {
        FMODEventEmitter.Instance.EmitEvent("BookOpening");
    }
    private void End()
    {
        // también deberías ponerle algún sonidito hehe

        GameManager.StopMusic(0.3f);
        FadeManager.Instance.FadeOut();
        SceneLoader.LoadScene("Intro");
        FMODEventEmitter.Instance.EmitEvent("BookClosing");
    }
}
