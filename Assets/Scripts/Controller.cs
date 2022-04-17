using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using System.Collections;

public class Box
{
    public int id;
    public bool move;
}

public class Controller : RestClient
{
    [SerializeField]
    Animator animatorBox0;

    [SerializeField]
    Animator animatorBox1;

    [SerializeField]
    Animator animatorBox2;

    [SerializeField]
    TMP_InputField InputURL;

    [SerializeField]
    TMP_InputField InputUsername;

    [SerializeField]
    TMP_InputField InputPassword;

    EndPoint ep;

    // Start is called before the first frame update
    void Start()
    {
        InputURL.text = PlayerPrefs.GetString("url");
        InputUsername.text = PlayerPrefs.GetString("username");
        InputPassword.text = PlayerPrefs.GetString("password");

        ep = new EndPoint();
        ep.baseUrl = InputURL.text;
        ep.login = InputUsername.text;
        ep.password = InputPassword.text;
        ep.authType = EndPoint.AuthType.BASIC;

        InputURL.onEndEdit.AddListener(text =>
        {
            PlayerPrefs.SetString("url", text);
            ep.baseUrl = text;
        });

        InputUsername.onEndEdit.AddListener(text =>
        {
            PlayerPrefs.SetString("username", text);
            ep.login = text;
        });

        InputPassword.onEndEdit.AddListener(text =>
        {
            PlayerPrefs.SetString("password", text);
            ep.password = text;
        });

        StartCoroutine(poling());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            animatorBox0.SetTrigger("Move");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            animatorBox1.SetTrigger("Move");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            animatorBox2.SetTrigger("Move");
        }
        /*
        else if (Input.GetKeyDown(KeyCode.F))
        {
            fetch();
        }
        */
    }

    IEnumerator poling() {
        while (true)
        {
            Debug.Log("poling");
            if (!isFetching) fetch();
            yield return new WaitForSeconds(3);
        }
    }

    bool checkIfIdle(int id)
    {
        bool isIdle = true;
        switch(id)
        {
            case 0:
                isIdle = animatorBox0.GetCurrentAnimatorStateInfo(0).IsName("Idle");
                break;
            case 1:
                isIdle = animatorBox1.GetCurrentAnimatorStateInfo(0).IsName("Idle");
                break;
            case 2:
                isIdle = animatorBox2.GetCurrentAnimatorStateInfo(0).IsName("Idle");
                break;
            default:
                break;
        }
        return isIdle;
    }

    bool isFetching = false;

    void fetch()
    {
        isFetching = true;
        Get(ep, "/box", (err, text) => {
            
            List<Box> boxes = JsonConvert.DeserializeObject<List<Box>>(text);
            boxes.ForEach(b => {
                Debug.Log($"{b.id}, {b.move}");
                switch(b.id)
                {
                    case 0:
                        if (checkIfIdle(0) && b.move)
                        {
                            animatorBox0.SetTrigger("Move");
                        }
                        break;
                    case 1:
                        if (checkIfIdle(1) && b.move)
                        {
                            animatorBox1.SetTrigger("Move");
                        }
                        break;
                    case 2:
                        if (checkIfIdle(2) && b.move)
                        {
                            animatorBox2.SetTrigger("Move");
                        }
                        break;
                    default:
                        break;
                }
                isFetching = false;
            });
        });
    }
}
