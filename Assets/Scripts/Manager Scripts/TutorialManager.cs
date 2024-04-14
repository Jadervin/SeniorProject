using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance { get; private set; }

    [Header("Checkmarks")]
    //[SerializeField] private GameObject jumpCheckmark;
    [SerializeField] private List<GameObject> jumpCheckmarks;
    [SerializeField] private List<GameObject> shootCheckmarks;
    [SerializeField] private List<GameObject> optionCheckmarks;
    [SerializeField] private List<GameObject> sWSwitchCheckmarks;
    [SerializeField] private List<GameObject> specialShootCheckmarks;
    [SerializeField] private List<GameObject> counterCheckmarks;
    [SerializeField] private GameObject counterSuccessCheckmark;
    [SerializeField] private GameObject orbCheckmark;
    [SerializeField] private GameObject planetTotemCheckmark;
    [SerializeField] private GameObject healthCheckmark;
    [SerializeField] private List<GameObject> mapCheckmarks;

    [Header("Checkmark Booleans")]
    [SerializeField] private bool jumpCheckmarksComplete;
    [SerializeField] private bool shootCheckmarksComplete;
    [SerializeField] private bool optionCheckmarksComplete;
    [SerializeField] private bool sWSwitchCheckmarksComplete;
    [SerializeField] private bool specialShootCheckmarksComplete;
    [SerializeField] private bool counterCheckmarksComplete;
    [SerializeField] private bool counterSuccessCheckmarkComplete;
    [SerializeField] private bool orbCheckmarkComplete;
    [SerializeField] private bool planetTotemCheckmarkComplete;
    [SerializeField] private bool healthCheckmarkComplete;
    [SerializeField] private bool mapCheckmarksComplete;

    /*
    [Header("Tutorial Booleans")]
    [SerializeField] private bool tutorial2Complete;
    [SerializeField] private bool tutorial3Complete;
    [SerializeField] private bool tutorial4Complete;
    [SerializeField] private bool tutorial5Complete;
    [SerializeField] private bool tutorial6Complete;
    [SerializeField] private bool tutorial7Complete;
    [SerializeField] private bool tutorial10Complete;
    */


    [Header("Unlockable Areas")]
    [SerializeField] private GameObject tutorial2UnlockableArea;
    [SerializeField] private GameObject tutorial3UnlockableArea;
    [SerializeField] private GameObject tutorial4UnlockableArea;
    [SerializeField] private GameObject tutorial5UnlockableArea;
    [SerializeField] private GameObject tutorial6UnlockableArea;
    [SerializeField] private GameObject tutorial7UnlockableArea;
    [SerializeField] private GameObject tutorial10UnlockableArea;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera tutorial2Camera;
    [SerializeField] private CinemachineVirtualCamera tutorial3Camera;
    [SerializeField] private CinemachineVirtualCamera tutorial4Camera;
    [SerializeField] private CinemachineVirtualCamera tutorial5Camera;
    [SerializeField] private CinemachineVirtualCamera tutorial6Camera;
    [SerializeField] private CinemachineVirtualCamera tutorial7Camera;
    [SerializeField] private CinemachineVirtualCamera tutorial10Camera;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {




        GameInput.Instance.OnShootPressed += GameInput_OnShootPressed;

        GameInput.Instance.OnJumpPressed += GameInput_OnJumpPressed;

        GameInput.Instance.OnPausePressed += GameInput_OnPausePressed;
    }

    

    private void GameInput_OnShootPressed(object sender, System.EventArgs e)
    {
        if (CameraManager.instance.GetCurrentCamera() == tutorial2Camera)
        {
            foreach (GameObject obj in shootCheckmarks)
            {
                if (obj.activeSelf == false)
                {
                    obj.SetActive(true);
                }
            }
            shootCheckmarksComplete = true;
        }
    }


    private void GameInput_OnJumpPressed(object sender, System.EventArgs e)
    {
        if (CameraManager.instance.GetCurrentCamera() == tutorial2Camera)
        {
            foreach (GameObject obj in jumpCheckmarks)
            {
                if (obj.activeSelf == false)
                {
                    obj.SetActive(true);
                }
            }

            jumpCheckmarksComplete = true;
        }
    }


    private void GameInput_OnPausePressed(object sender, System.EventArgs e)
    {
        if (CameraManager.instance.GetCurrentCamera() == tutorial2Camera)
        {
            foreach (GameObject obj in optionCheckmarks)
            {
                if (obj.activeSelf == false)
                {
                    obj.SetActive(true);
                }
            }

            optionCheckmarksComplete = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(jumpCheckmarksComplete == true && shootCheckmarksComplete == true && optionCheckmarksComplete == true)
        {
            //tutorial2Complete = true;
            jumpCheckmarksComplete = false;
            shootCheckmarksComplete = false;
            optionCheckmarksComplete = false;

            tutorial2UnlockableArea.SetActive(false);
        }
    }
}
