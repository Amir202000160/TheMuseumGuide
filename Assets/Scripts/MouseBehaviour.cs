using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Outline))]
public class MouseBehaviour : MonoBehaviour
{
     private  Outline outline;
     [SerializeField] GameObject Player;
     [SerializeField] GameObject UI;
     [SerializeField] GameObject[] Status;
    [SerializeField] float mouseSenstivity ;
    InputAction LookAction;
    InputAction LeftMouseAction;
    [SerializeField] LayerMask Statuslayer;
    [SerializeField] Camera fpscam;
    Ray ray;
    [HideInInspector]
    public float angle;
    float xRotate;

    void Start()
    {
    outline = GetComponent<Outline>();
    outline.enabled = false;
    UI.SetActive(false) ;
     LookAction = InputSystem.actions.FindAction("Mouse");
     LeftMouseAction = InputSystem.actions.FindAction("Attack");
        UnityEngine.Cursor.visible =false;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
    }
    void Update()
    {
        Vector2 LookValue = LookAction.ReadValue<Vector2>() *mouseSenstivity * Time.deltaTime;
        float xRotate = -LookValue.y;
        //xRotate = Mathf.Clamp(xRotate, -900f, 900f);
        transform.rotation *= Quaternion.Euler(xRotate, 0, 0);
        //transform.Rotate(Vector3.right * xRotate);
        //transform.localRotation = Quaternion.Euler(xRotate, 0f, 0f);
        Player.transform.Rotate(Vector3.up * LookValue.x);
        //transform.position = Player.transform.position;
        angle = Mathf.Atan2(LookValue.y, LookValue.x) * Mathf.Rad2Deg;
       // Debug.Log(angle);
         CamRay();
        
    }


    void CamRay()
    {
        ray = new Ray(fpscam.transform.position, fpscam.transform.forward);
        
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 10000f, Statuslayer))
        {
           
            hitInfo.collider.gameObject.GetComponent<Outline>().enabled = true;
            Debug.Log("Hit "+ hitInfo.collider.name);
            if(LeftMouseAction.IsPressed())
            {
                Debug.Log("Shoot "+ hitInfo.collider.name);
                 hitInfo.collider.gameObject.GetComponent<AudioSource>().Play();
                  UI.SetActive(true);
            }
            if(!hitInfo.collider.gameObject.GetComponent<AudioSource>().isPlaying)
                  {
                    UI.SetActive(false);
                  }
        }
       else 
        {
            foreach(GameObject status in Status)
            {
                status.GetComponent<Outline>().enabled = false;
            }
        }

    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(ray.origin,ray.direction* 10000f);
    }
    
}
