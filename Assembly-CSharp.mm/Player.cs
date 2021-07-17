using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Cinemachine;
using UnityEngine.SceneManagement;

namespace Assembly_CSharp.mm
{
    [MonoMod.MonoModPatch("global::Player")]
    public class patch_Player : Player
    {
        public static bool first = false;

        bool noclip;
        float clipflyspeed;
        string clipflyspeedstr;
        bool centerCam;
        bool forceInvincible;

        string dbgLog;

        CinemachineBrain cinema;
        Camera mainCam;

        bool isFirst = false;

        public extern void orig_Start();
        public void Start()
        {
            if (first)
            {
                Destroy(gameObject);
                return;
            }
            first = true;
            isFirst = true;

            dbgLog = "";

            orig_Start();

            Player.isInvincible = true;

            // Disable volume for testing
            AudioListener.volume = 0f;

            windowRect = new Rect(20, 20, 0, 0);

            clipflyspeed = 10f;
            clipflyspeedstr = clipflyspeed.ToString();
            centerCam = false;

            forceInvincible = true;

            mainCam = Camera.main;
            cinema = mainCam.GetComponent<CinemachineBrain>();
        }

        void OnDestroy()
        {
            if(isFirst)
                first = false;
        }

        extern void orig_Update();
        void Update()
        {
            if (noclip)
            {
                float hor = CrossPlatformInputManager.GetAxisRaw("Horizontal");
                float ver = CrossPlatformInputManager.GetAxisRaw("Vertical");

                playerRigidbody.velocity = new Vector2(hor, ver) * clipflyspeed;
            }else
            {
                orig_Update();
            }

            CenterCam();

            if (Input.GetKeyDown(KeyCode.X))
            {
                ToggleNoClip();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                ToggleCenterCam();
            }

            if (centerCam)
            {
                mainCam.orthographicSize -= Input.mouseScrollDelta.y;
                mainCam.orthographicSize = Math.Max(mainCam.orthographicSize, 0.1f);
            }

            if (forceInvincible)
            {
                isInvincible = forceInvincible;
            }
        }

        [MonoMod.MonoModIgnore]
        private Rigidbody2D playerRigidbody;

        void ToggleNoClip()
        {
            noclip = !noclip;
            if (noclip)
            {
                playerRigidbody.isKinematic = true;
                playerRigidbody.velocity = Vector2.zero;
            }
            else
            {
                playerRigidbody.isKinematic = false;
            }
        }

        void ToggleCenterCam()
        {
            centerCam = !centerCam;
            if (centerCam)
            {
                cinema.enabled = false;
            
                CenterCam();
            }
            else
            {
                cinema.enabled = true;
            }
        }

        void CenterCam()
        {
            cinema.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
        }

        // GUI Stuff
        string enableWord(bool val)
        {
            return val ? "Disable" : "Enable"; 
        }

        Rect windowRect;

        void OnGUI()
        {
            // Register the window. Notice the 3rd parameter
            windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "Title Dbg");
        }

        // Make the contents of the window
        void DoMyWindow(int windowID)
        {
            // This button will size to fit the window
            if (GUILayout.Button($"{enableWord(noclip)} NoClip (x)"))
            {
                ToggleNoClip();
            }

            if (noclip)
            {
                GUILayout.Label("Flying Speed");

                string n_clipflyspeedstr = GUILayout.TextField(clipflyspeedstr);
                
                if(n_clipflyspeedstr != clipflyspeedstr)
                {
                    float.TryParse(n_clipflyspeedstr, out clipflyspeed);
                    clipflyspeedstr = n_clipflyspeedstr;
                }
            }

            if(GUILayout.Button($"{enableWord(centerCam)} Center Camera (c)"))
            {
                ToggleCenterCam();
            }

            if (centerCam)
            {
                GUILayout.Label("Scroll with the mouse wheel to zoom");

                Physics2D.alwaysShowColliders = true;
            }

            if(GUILayout.Button($"{enableWord(forceInvincible)} Invincibility")) {
                forceInvincible = !forceInvincible;
                isInvincible = forceInvincible;
            }

            if (dbgLog != "")
            {
                GUILayout.Label(dbgLog);
            }

            if(GUILayout.Button("Break all the things"))
            {
                int currInd = SceneManager.GetActiveScene().buildIndex;
                Log(currInd.ToString());

                for(int i = 1; i < 9; i++)
                {
                    if (i != currInd)
                    {
                        SceneManager.LoadScene(i, LoadSceneMode.Additive);
                    }
                }

                Invoke("KillCams", 1f);
            }

            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        void KillCams()
        {
            Camera[] cams = FindObjectsOfType<Camera>();

            for (int i = 0; i < cams.Length; i++)
            {
                if (cams[i] != mainCam)
                {
                    Destroy(cams[i].gameObject, 0f);

                    Log($"Destroying {cams[i].name}");
                }
            }
        }

        void Log(object l)
        {
            dbgLog += l.ToString() + "\n";
        }
    }
}
