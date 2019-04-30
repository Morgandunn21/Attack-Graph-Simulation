using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerGUI : MonoBehaviour {

    private string output1, output2;

    int[] cooldowns = new int[4];
    int[] reqPoints = new int[4];

    public int playerNum;
    public StringBuilder totalOutput;
    public GUIStyle myStyle;
    public float delay;

    private string[] names;

	// Use this for initialization
	void Start () {
        totalOutput = new StringBuilder("Running...");
        getNames();
        getCooldowns();
        getReqPoints();
	}
	
	/// <summary>
    /// Creates all gui objects on the canvas
    /// </summary>
	void OnGUI () {

        for(int i = 0; i < 4; i++)
        {

            setOutputs(i);

            GUI.Box(new Rect((Screen.width / 12) * ((i + 1) + ((playerNum-1) * 8)) - 75, Screen.height - 110,60, 50), output1);
            GUI.Box(new Rect((Screen.width / 12) * ((i + 1) + ((playerNum-1) * 8)) - 75, Screen.height - 55, 60, 50), output2);

        }

        initMyStyle();

        GUI.Box(new Rect((playerNum-1)*(2*(Screen.width/3) - 5) + 40, 20, Screen.width*2 / 7, Screen.height - 150), totalOutput.ToString(), myStyle);
	}

    /// <summary>
    /// initializes a custom GUI Style for the console output
    /// </summary>
    private void initMyStyle()
    {
        myStyle = new GUIStyle("box");
        myStyle.wordWrap = true;
        myStyle.fontSize = 20;
        myStyle.alignment = TextAnchor.UpperLeft;
        myStyle.normal.textColor = Color.green;
        myStyle.fontStyle = FontStyle.Bold;
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    private void Update()
    {
        getCooldowns();
        getReqPoints();
    }

    /// <summary>
    /// Pauses execution for a given number of seconds
    /// </summary>
    /// <returns></returns>
    public IEnumerator appendOutput(string newOutput)
    {
        totalOutput.Clear().Append("");
        while (newOutput.Length != 0)
        {
            totalOutput.Append(newOutput.Substring(0, newOutput.IndexOf("\n") + 1));
            newOutput = newOutput.Remove(0, newOutput.IndexOf("\n") + 1);
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(delay);
        totalOutput.Clear().Append("Running...");
    }

    /// <summary>
    /// Gets the cooldowns on each players abilities
    /// </summary>
    public void getCooldowns()
    {
        cooldowns = gameObject.GetComponent<Hacks>().getCooldowns();
    }

    /// <summary>
    /// Gets the points required for each players abilities
    /// </summary>
    public void getReqPoints()
    {
        reqPoints = gameObject.GetComponent<Hacks>().getReqScores();
    }

    public void getNames()
    {
        names = gameObject.GetComponent<Hacks>().getNames();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newOutput"></param>
    public void addToOutput(string newOutput)
    {
        StartCoroutine(appendOutput(newOutput));
    }

    /// <summary>
    /// Sets the outputs to each box at the bottom of the scree
    /// </summary>
    public void setOutputs(int i)
    {
        int score = gameObject.GetComponent<PointControl>().getScore();
        if (score < reqPoints[i])
        {
            output1 = "Points\n" + score + "/" + reqPoints[i];
        }
        else if (cooldowns[i] == 0)
        {
            output1 = "Ready";
        }
        else
        {
            output1 = cooldowns[i].ToString();
        }

        output2 = names[i];
    }
}
