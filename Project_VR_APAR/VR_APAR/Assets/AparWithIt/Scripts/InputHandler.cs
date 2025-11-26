using UnityEngine.SceneManagement;
using UnityEngine;

public class InputHandler: MonoBehaviour
{
    private MouseTrigger _mouseTrigger;
    
    void Update()
    {
        MouseInput();
        RestartScene();
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if(hitInfo.collider.CompareTag("MouseTrigger"))
                {
                    _mouseTrigger = hitInfo.collider.GetComponent<MouseTrigger>();
                    _mouseTrigger.onMouseDown?.Invoke();
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(_mouseTrigger != null)
            {
                _mouseTrigger.onMouseUp?.Invoke();
                _mouseTrigger = null;
            }
        }
    }

    private void RestartScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene);
        }
    }
}
