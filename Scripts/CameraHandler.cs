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
    private float targetY;

    private LineRenderer lineRendererForward; // ELIMINA DOPO IL DEBUG!
    private LineRenderer lineRendererBackward; // ELIMINA DOPO IL DEBUG!

    void Start()
    {
        layerMask = ~(1 << LayerMask.NameToLayer("Player"));
        targetY = transform.position.y;
        pos = transform.position;
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
        Vector3 origindx = new Vector3(player.transform.position.x, player.transform.position.y-4 , player.transform.position.z);
        Vector3 originsx = new Vector3(player.transform.position.x, player.transform.position.y-4 , player.transform.position.z -3);

        // Aggiorna sempre le linee di debug - ELIMINA DOPO IL DEBUG!
        lineRendererForward.SetPosition(0, origindx); // ELIMINA DOPO IL DEBUG!
        lineRendererForward.SetPosition(1, origindx + Vector3.forward * ray); // ELIMINA DOPO IL DEBUG!

        lineRendererBackward.SetPosition(0, originsx); // ELIMINA DOPO IL DEBUG!
        lineRendererBackward.SetPosition(1, originsx + Vector3.back * ray); // ELIMINA DOPO IL DEBUG!

        if (!playerController.GetGroundedState())
        {
            if (Physics.Raycast(origindx, Vector3.forward, out hit, ray,layerMask) || Physics.Raycast(originsx, Vector3.back, out hit, ray, layerMask))
            {                
                targetY = hit.point.y + offsety;
                return hit.point.y + offsety;
                
            }
            
        }

        return targetY;
    }
}
