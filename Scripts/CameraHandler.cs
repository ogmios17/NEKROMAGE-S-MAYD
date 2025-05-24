using Unity.VisualScripting;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private Vector3 pos;
    private Vector3 currentPos;
    private RaycastHit hit;
    public GameObject player;
    public PlayerController playerController;
    public float smoothness;
    private float usedSmoothness;
    public float downModifier;
    public float offsetz = -9;
    public float offsety = 12;
    public float offsetx = -30;
    private float previousPosition;
    private Vector3 velocity = Vector3.zero;
    int layerMask;
    public float ray;
    public float rayDown;
    private float targetY;

    private LineRenderer lineRendererForward; // ELIMINA DOPO IL DEBUG!
    private LineRenderer lineRendererBackward; // ELIMINA DOPO IL DEBUG!
    private LineRenderer lineRendererUp;  // ELIMINA DOPO IL DEBUG!

    void Start()
    {
        layerMask = ~(1 << LayerMask.NameToLayer("Player"));
        targetY = transform.position.y;
        pos = player.transform.position;
        // Crea oggetto figlio per il LineRenderer forward - ELIMINA DOPO IL DEBUG!
        GameObject forwardObj = new GameObject("ForwardLine"); // ELIMINA DOPO IL DEBUG!
        forwardObj.transform.parent = this.transform; // ELIMINA DOPO IL DEBUG!
        lineRendererForward = forwardObj.AddComponent<LineRenderer>(); // ELIMINA DOPO IL DEBUG!
        lineRendererForward.positionCount = 2; // ELIMINA DOPO IL DEBUG!
        lineRendererForward.startWidth = 0.5f; // ELIMINA DOPO IL DEBUG!
        lineRendererForward.endWidth = 0.5f; // ELIMINA DOPO IL DEBUG!
        lineRendererForward.material = new Material(Shader.Find("Sprites/Default")); // ELIMINA DOPO IL DEBUG!
        lineRendererForward.startColor = Color.green; // ELIMINA DOPO IL DEBUG!
        lineRendererForward.endColor = Color.green; // ELIMINA DOPO IL DEBUG!
        lineRendererForward.enabled = true; // ELIMINA DOPO IL DEBUG!

        // Crea oggetto figlio per il LineRenderer backward - ELIMINA DOPO IL DEBUG!
        GameObject backwardObj = new GameObject("BackwardLine"); // ELIMINA DOPO IL DEBUG!
        backwardObj.transform.parent = this.transform; // ELIMINA DOPO IL DEBUG!
        lineRendererBackward = backwardObj.AddComponent<LineRenderer>(); // ELIMINA DOPO IL DEBUG!
        lineRendererBackward.positionCount = 2; // ELIMINA DOPO IL DEBUG!
        lineRendererBackward.startWidth = 0.5f; // ELIMINA DOPO IL DEBUG!
        lineRendererBackward.endWidth = 0.5f; // ELIMINA DOPO IL DEBUG!
        lineRendererBackward.material = new Material(Shader.Find("Sprites/Default")); // ELIMINA DOPO IL DEBUG!
        lineRendererBackward.startColor = Color.red; // ELIMINA DOPO IL DEBUG!
        lineRendererBackward.endColor = Color.red; // ELIMINA DOPO IL DEBUG!
        lineRendererBackward.enabled = true; // ELIMINA DOPO IL DEBUG!

        GameObject downObj = new GameObject("DownLine"); // ELIMINA DOPO IL DEBUG!
        downObj.transform.parent = this.transform; // ELIMINA DOPO IL DEBUG!
        lineRendererUp = downObj.AddComponent<LineRenderer>(); // ELIMINA DOPO IL DEBUG!
        lineRendererUp.positionCount = 2; // ELIMINA DOPO IL DEBUG!
        lineRendererUp.startWidth = 0.5f; // ELIMINA DOPO IL DEBUG!
        lineRendererUp.endWidth = 0.5f; // ELIMINA DOPO IL DEBUG!
        lineRendererUp.material = new Material(Shader.Find("Sprites/Default")); // ELIMINA DOPO IL DEBUG!
        lineRendererUp.startColor = Color.blue; // ELIMINA DOPO IL DEBUG!
        lineRendererUp.endColor = Color.blue; // ELIMINA DOPO IL DEBUG!
        lineRendererUp.enabled = true; // ELIMINA DOPO IL DEBUG!
    }

    void Update()
    {
        currentPos = transform.position;
        pos.z = player.transform.position.z + offsetz;
        pos.x = player.transform.position.x + offsetx;      
        pos.y = CheckForPlatforms();

        transform.position = new Vector3(pos.x, currentPos.y, pos.z);
        transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smoothness);
    }

    float CheckForPlatforms()
    {
        Vector3 origindx = new Vector3(player.transform.position.x-0.1f, player.transform.position.y-downModifier , player.transform.position.z-0.1f);
        Vector3 originsx = new Vector3(player.transform.position.x+0.1f, player.transform.position.y-downModifier , player.transform.position.z+0.1f);
        Vector3 originup = new Vector3(player.transform.position.x, player.transform.position.y - 4, player.transform.position.z);

        // Aggiorna sempre le linee di debug - ELIMINA DOPO IL DEBUG!
        lineRendererForward.SetPosition(0, origindx); // ELIMINA DOPO IL DEBUG!
        lineRendererForward.SetPosition(1, origindx + playerController.getFrontDirection() * ray); // ELIMINA DOPO IL DEBUG!

        lineRendererBackward.SetPosition(0, originsx); // ELIMINA DOPO IL DEBUG!
        lineRendererBackward.SetPosition(1, originsx + playerController.getBackDirection() * ray); // ELIMINA DOPO IL DEBUG!

        lineRendererUp.SetPosition(0, originup); // ELIMINA DOPO IL DEBUG!
        lineRendererUp.SetPosition(1, originup + Vector3.down * rayDown); // ELIMINA DOPO IL DEBUG!


        if (Physics.Raycast(origindx, playerController.getFrontDirection(), out hit, ray,layerMask) || Physics.Raycast(originsx, playerController.getBackDirection(), out hit, ray, layerMask))
        {                
            targetY = hit.point.y + offsety;
            return hit.point.y + offsety;
                
        } else if(Physics.Raycast(originup, Vector3.down, out hit, rayDown, layerMask))
        {
            targetY = hit.point.y + offsety;
            return hit.point.y + offsety;
        }



            return targetY;
    }

    void SetOffsetx(float xmod)
    {
        offsetx = xmod;
        playerController.setInteractableState(true);
    }
    void SetOffsetz(float zmod)
    {
        offsetz = zmod;
        playerController.setInteractableState(true);
    }

    void MoveCameraInPlacex(float x)
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(x,transform.position.y,transform.position.z), ref velocity, 1000f);
    }
    void MoveCameraInPlacez(float z)
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, transform.position.y, z), ref velocity, 1000f);
    }
    void MoveCameraInPlacey(float y)
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, y, transform.position.z), ref velocity, 1000f);
    }
}
