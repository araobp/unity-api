using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public class Box
{
    public int id;
    public bool move;
}

public class Controller : RestClient
{
    [SerializeField]
    Animator animator;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            animator.SetTrigger("Move");
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            fetch();
        }
    }

    void fetch()
    {
        Get(ep, "/box", (err, text) => {
            
            List<Box> boxes = JsonConvert.DeserializeObject<List<Box>>(text);
            boxes.ForEach(b => {
                Debug.Log($"{b.id}, {b.move}");
            });
        });
    }
}
