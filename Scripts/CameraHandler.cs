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
    private float currentOffsetx;
    private float currentOffsetz;
    private float previousPosition;
    private Vector3 velocity = Vector3.zero;
    int layerMask;
    public float ray;
    public float rayDown;
    private float targetY;
    public float horizontalOffsetMoving;
    public float turningSmoothness;
    private float targetOffsetx;
    private float targetOffsetz;
    private float xOffsetVelocity;
    private float zOffsetVelocity;
    private float xRotationVelocity;
    private float yRotationVelocity;
    private float zRotationVelocity;
    private Quaternion targetRotation;
    private float currentRotationx;
    private float currentRotationy;
    private float currentRotationz;
    private float rotationSmoothness;

    private LineRenderer lineRendererForward; // ELIMINA DOPO IL DEBUG!
    private LineRenderer lineRendererBackward; // ELIMINA DOPO IL DEBUG!
    private LineRenderer lineRendererUp;  // ELIMINA DOPO IL DEBUG!
    private LineRenderer lineRendererUpsx;  // ELIMINA DOPO IL DEBUG!

    void Start()
    {

        targetRotation = transform.rotation;
        Vector3 targetEuler = transform.rotation.eulerAngles;
        currentRotationx = targetEuler.x;
        currentRotationy = targetEuler.y;
        currentRotationz = targetEuler.z;
        currentOffsetx = offsetx;
        currentOffsetz = offsetz;
        targetOffsetx = offsetx;
        targetOffsetz = offsetz;
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

        GameObject downObjsx = new GameObject("DownLine"); // ELIMINA DOPO IL DEBUG!
        downObjsx.transform.parent = this.transform; // ELIMINA DOPO IL DEBUG!
        lineRendererUpsx = downObjsx.AddComponent<LineRenderer>(); // ELIMINA DOPO IL DEBUG!
        lineRendererUpsx.positionCount = 2; // ELIMINA DOPO IL DEBUG!
        lineRendererUpsx.startWidth = 0.5f; // ELIMINA DOPO IL DEBUG!
        lineRendererUpsx.endWidth = 0.5f; // ELIMINA DOPO IL DEBUG!
        lineRendererUpsx.material = new Material(Shader.Find("Sprites/Default")); // ELIMINA DOPO IL DEBUG!
        lineRendererUpsx.startColor = Color.blue; // ELIMINA DOPO IL DEBUG!
        lineRendererUpsx.endColor = Color.blue; // ELIMINA DOPO IL DEBUG!
        lineRendererUpsx.enabled = true; // ELIMINA DOPO IL DEBUG!
    }

    void Update()
    {
        currentPos = transform.position;
        currentOffsetx = Mathf.SmoothDamp(currentOffsetx, targetOffsetx, ref xOffsetVelocity, turningSmoothness);
        currentOffsetz = Mathf.SmoothDamp(currentOffsetz, targetOffsetz, ref zOffsetVelocity, turningSmoothness);
        Vector3 targetEuler = targetRotation.eulerAngles;
        currentRotationx = Mathf.SmoothDamp(currentRotationx, targetEuler.x, ref xRotationVelocity, rotationSmoothness);
        currentRotationy = Mathf.SmoothDamp(currentRotationy, targetEuler.y, ref yRotationVelocity, rotationSmoothness);
        currentRotationz = Mathf.SmoothDamp(currentRotationz, targetEuler.z, ref zRotationVelocity, rotationSmoothness);

        pos.z = player.transform.position.z + currentOffsetz;
        pos.x = player.transform.position.x + currentOffsetx;      
        pos.y = CheckForPlatforms();
        Vector3 targetPos = new Vector3(player.transform.position.x + currentOffsetx, pos.y, player.transform.position.z + currentOffsetz);
        
        transform.position = new Vector3(pos.x, currentPos.y, pos.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothness);
        transform.rotation = Quaternion.Euler(currentRotationx, currentRotationy, currentRotationz);
        
        
    }

    float CheckForPlatforms()
    {
        Vector3 origindx = new Vector3(player.transform.position.x-0.1f, player.transform.position.y-downModifier , player.transform.position.z-0.1f);
        Vector3 originsx = new Vector3(player.transform.position.x+0.1f, player.transform.position.y-downModifier , player.transform.position.z+0.1f);
        Vector3 originup = new Vector3(player.transform.position.x, player.transform.position.y - 4, player.transform.position.z);
        Vector3 originupsx = new Vector3(player.transform.position.x, player.transform.position.y - 4, player.transform.position.z);

        // Aggiorna sempre le linee di debug - ELIMINA DOPO IL DEBUG!
        lineRendererForward.SetPosition(0, origindx); // ELIMINA DOPO IL DEBUG!
        lineRendererForward.SetPosition(1, origindx + playerController.getFrontDirection() * ray); // ELIMINA DOPO IL DEBUG!

        lineRendererBackward.SetPosition(0, originsx); // ELIMINA DOPO IL DEBUG!
        lineRendererBackward.SetPosition(1, originsx + playerController.getBackDirection() * ray); // ELIMINA DOPO IL DEBUG!

        lineRendererUp.SetPosition(0, originup); // ELIMINA DOPO IL DEBUG!
        lineRendererUp.SetPosition(1, originup + (playerController.getFrontDirection() + Vector3.down) * rayDown); // ELIMINA DOPO IL DEBUG!

        lineRendererUpsx.SetPosition(0, originupsx); // ELIMINA DOPO IL DEBUG!
        lineRendererUpsx.SetPosition(1, originupsx + (playerController.getBackDirection() + Vector3.down) * rayDown); // ELIMINA DOPO IL DEBUG!

        if (Physics.Raycast(origindx, playerController.getFrontDirection(), out hit, ray,layerMask) || Physics.Raycast(originsx, playerController.getBackDirection(), out hit, ray, layerMask))
        {                
            targetY = hit.point.y + offsety;
            return hit.point.y + offsety;
                
        } else if(Physics.Raycast(originup, playerController.getFrontDirection() + Vector3.down, out hit, rayDown, layerMask) || Physics.Raycast(originupsx, playerController.getBackDirection() + Vector3.down, out hit, rayDown, layerMask))
        {
            targetY = hit.point.y + offsety;
            return hit.point.y + offsety;
        }



            return targetY;
    }

    public void AdjustFront()
    {
        targetOffsetz = offsetz + playerController.getFrontDirection().z * horizontalOffsetMoving;
        targetOffsetx = offsetx + playerController.getFrontDirection().x * horizontalOffsetMoving;
        
    }

    public void AdjustBack()
    {
        targetOffsetz = offsetz + playerController.getBackDirection().z * horizontalOffsetMoving;
        targetOffsetx = offsetx + playerController.getBackDirection().x * horizontalOffsetMoving;
        
    }
    public void ResetOffsets()
    {
        targetOffsetz = offsetz;
        targetOffsetx = offsetx;
    }
    public void AdjustRotation(Quaternion rotation, float smoothness)
    {
        targetRotation = rotation;
        rotationSmoothness = smoothness;
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

    public void SetTargetOffset(float targetx, float targetz)
    {
        targetOffsetx = targetx;
        targetOffsetz = targetz;
    }
    public Vector3 GetCurrentOffset()
    {
        return new Vector3(currentOffsetx, offsety,currentOffsetz);   
    }

    public void setTurningSmoothness(float turningSmoothness)
    {
        this.turningSmoothness = turningSmoothness;
        Debug.Log("Set turningSmoothness to: " + turningSmoothness);
    }
    public float getTurningSmoothness()
    {
        return turningSmoothness;
    }
}
