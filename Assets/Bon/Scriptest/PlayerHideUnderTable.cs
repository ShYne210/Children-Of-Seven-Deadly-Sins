using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHideUnderTable : MonoBehaviour {
    public Transform[] hidePoints;         // nhiều điểm hide
    public GameObject playerModel;         
    public CharacterController controller; 
    public MonoBehaviour movementScript;   
    public Camera playerCamera;            
    public float hideRange = 2f;           

    private Vector3 savedPos;
    private Quaternion savedRot;
    private float normalHeight;
    private Vector3 normalCenter;
    private Vector3 normalCamLocalPos;
    private bool isHidden = false;
    private Transform currentHidePoint;

    void Start() {
        if (controller != null) {
            normalHeight = controller.height;
            normalCenter = controller.center;
        }
        if (playerCamera != null) {
            normalCamLocalPos = playerCamera.transform.localPosition;
        }
    }

    void Update() {
        // đổi sang phím Q
        if (Keyboard.current.qKey.wasPressedThisFrame) {
            if (!isHidden) {
                // tìm điểm hide gần nhất
                Transform nearest = null;
                float minDist = float.MaxValue;
                foreach (Transform point in hidePoints) {
                    float dist = Vector3.Distance(transform.position, point.position);
                    if (dist < minDist) {
                        minDist = dist;
                        nearest = point;
                    }
                }

                if (nearest != null && minDist <= hideRange) {
                    savedPos = transform.position;
                    savedRot = transform.rotation;
                    currentHidePoint = nearest;

                    transform.position = nearest.position;
                    transform.rotation = nearest.rotation;
                    if (playerModel != null) playerModel.SetActive(false);
                    if (controller != null) {
                        controller.height = 0.5f;
                        controller.center = new Vector3(0, 0.25f, 0);
                    }
                    if (movementScript != null) movementScript.enabled = false;
                    if (playerCamera != null) {
                        playerCamera.transform.position = nearest.position;
                        playerCamera.transform.rotation = nearest.rotation;
                    }
                    isHidden = true;
                    Debug.Log("Player is hiding at " + nearest.name);
                }
            } else {
                // Thoát hide
                transform.position = savedPos;
                transform.rotation = savedRot;
                if (playerModel != null) playerModel.SetActive(true);
                if (controller != null) {
                    controller.height = normalHeight;
                    controller.center = normalCenter;
                }
                if (movementScript != null) movementScript.enabled = true;
                if (playerCamera != null) {
                    playerCamera.transform.localPosition = normalCamLocalPos;
                }
                isHidden = false;
                currentHidePoint = null;
                Debug.Log("Player left hide spot!");
            }
        }
    }
}
