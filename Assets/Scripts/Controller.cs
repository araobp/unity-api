using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using System.Collections;

public class Box__c
{
    public double id__c;
    public bool move__c;
}

public class Controller : RestClient
{
    [SerializeField]
    GameObject ConfigPanel;

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

        ConfigPanel.SetActive(false);

        StartCoroutine(poling());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ConfigPanel.SetActive(!ConfigPanel.activeSelf);
        }

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
            
            List<Box__c> boxes = JsonConvert.DeserializeObject<List<Box__c>>(text);
            boxes.ForEach(b => {
                Debug.Log($"{b.id__c}, {b.move__c}");
                switch(b.id__c)
                {
                    case 0:
                        if (checkIfIdle(0) && b.move__c)
                        {
                            animatorBox0.SetTrigger("Move");
                        }
                        break;
                    case 1:
                        if (checkIfIdle(1) && b.move__c)
                        {
                            animatorBox1.SetTrigger("Move");
                        }
                        break;
                    case 2:
                        if (checkIfIdle(2) && b.move__c)
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
