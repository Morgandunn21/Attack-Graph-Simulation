using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hacks : MonoBehaviour {

    public const int WSTART = 0;
    public const int BSTART = 1;
    public const int TSTART = 2;
    public const int TOFF = 3;
    public const int BOFF = 4;
    public const int WOFF = 5;

    public static string[] stmts = { "Wifi Start\n", "Beagle Bone Start\n", "Teensy Start\n", "Teensy Off\n", "Beagle Bone Off\n", "Wifi Off\n" };

    public GameObject opponent;
    public GameObject player;
    public GameObject fire;
    public PlayerGUI gui;
    public ScoreBoard sb;
    public string b1, b2, b3, b4;

    private string[] buttons = new string[4];
    private const int numHacks = 8;

    private Hack[] hacks = new Hack[numHacks];
    private Reset reset;


    // Use this for initialization
    void Awake() {
        gui = GetComponent<PlayerGUI>();

        buttons[0] = b1;
        buttons[1] = b2;
        buttons[2] = b3;
        buttons[3] = b4;

        Debug.Log("Instantiating Hacks");

        PointControl points = player.GetComponent<PointControl>();
        PlayerMovement control = player.GetComponent<PlayerMovement>();

        hacks[4] = new flip(points);
        hacks[5] = new freeze(points);
        hacks[6] = new reverse(points);
        hacks[0] = new dropPoint(points);
        hacks[1] = new slowDown(points);
        hacks[2] = new Delay(points);
        hacks[7] = new Kill(points, sb, fire);
        hacks[3] = new Stuck(points);
        reset = new Reset(control);

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            hacks[i].update();

            if (player.GetComponent<PointControl>().playerNum == 1)
            {
                if(Input.GetKeyDown("space"))
                {
                    reset.run(opponent, gui);
                }
            }
            else
            {
                if(Input.GetKeyDown("return"))
                {
                    reset.run(opponent, gui);
                }
            }

            if (Input.GetKeyDown(buttons[i]))
            {
                hacks[i].run(opponent, gui);
            }
        }
    }

    public int[] getCooldowns()
    {
        int[] cooldowns = new int[4];

        for (int i = 0; i < 4; i++)
        {
            if (hacks[i] == null)
            {
                Debug.Log("Hack[" + i + "] is null");
            }
            else
            {
                cooldowns[i] = hacks[i].getCurrentCooldown();
            }
        }

        return cooldowns;
    }

    public int[] getReqScores()
    {
        int[] reqScores = { hacks[0].getPointsReq(), hacks[1].getPointsReq(), hacks[2].getPointsReq(), hacks[3].getPointsReq() };
        return reqScores;
    }

    public string[] getNames()
    {
        string[] names = new string[numHacks];

        for(int i = 0; i < buttons.Length; i++)
        {
            names[i] = buttons[i] + "\n" + hacks[i].ToString();
        }

        return names;
    }

    public abstract class Hack
    {

        protected float cooldownTime, currentCooldown;
        protected int pointsRequired;
        protected string name;
        protected string output;
        protected int[] instr;
        protected PointControl points;
        protected int resetCooldown = 3;

        public Hack(int cT, int pr, string n)
        {
            cooldownTime = cT + resetCooldown;
            pointsRequired = pr;
            name = n;
            currentCooldown = 0;
        }

        public void update()
        {
            if (currentCooldown <= 0)
            {
                return;
            }
            else
            {
                currentCooldown -= Time.deltaTime;
            }
        }

        public abstract void run(GameObject opponent, PlayerGUI gui);
    //Getters
        public int getCurrentCooldown()
        {
            return (int)currentCooldown;
        }

        public int getPointsReq()
        {
            return pointsRequired;
        }

        public override string ToString()
        {
            return name;
        }

        public void showOutput(PlayerGUI gui)
        {
            gui.addToOutput(output);
        }
    }
    //possibly not possible
    public class flip : Hack
    {
        public flip(PointControl p) : base(5, 0, "Flip")
        {
            points = p;

            output = "Flip.exe\n" +
                "Accessing Oppponent Wifi...\n" +
                "Accessing Direction.log...\n" +
                "Flipping Horizontal Input...\n" +
                "Hack Successful\n";
        }

        public override void run(GameObject opponent, PlayerGUI gui)
        {
            if (base.currentCooldown <= 0 && points.getScore() > pointsRequired - 1)
            {
                showOutput(gui);
                opponent.GetComponent<PlayerMovement>().setRotationSpeed(-opponent.GetComponent<PlayerMovement>().getRotationSpeed());
                base.currentCooldown = base.cooldownTime;
            }
        }

    }
    //implemented
    public class freeze : Hack
    {
        public freeze(PointControl p) : base(10, 2, "Freeze")
        {
            points = p;

            output = "Freeze.exe/n" +
                "Accessing Car Wifi.../n" +
                "Accessing BeagleBone Terminal.../n" +
                "Running 'shutdown' in Terminal.../n" +
                "Restarting Beaglebone.../n" +
                "Hack Successful/n";
        }

        public override void run(GameObject opponent, PlayerGUI gui)
        {
            if (base.currentCooldown <= 0 && points.getScore() >= pointsRequired)
            {
                showOutput(gui);
                opponent.GetComponent<PlayerMovement>().setMovementSpeed(0);
                base.currentCooldown = base.cooldownTime;
            }
        }
    }
    //maybe not possible
    public class reverse : Hack
    {
        public reverse(PointControl p) : base(15, 4, "Reverse")
        {
            points = p;

            output = "Reverse.exe\n" +
                "Accessing Car Wifi...\n" +
                "Accessing Direction.log...\n" +
                "Flipping Vertical Input...\n" +
                "Hack Successful\n";
        }

        public override void run(GameObject opponent, PlayerGUI gui)
        {
            if (base.currentCooldown <= 0 && points.getScore() >= pointsRequired)
            {
                showOutput(gui);
                opponent.GetComponent<PlayerMovement>().setMovementSpeed(-opponent.GetComponent<PlayerMovement>().getMovementSpeed());
                base.currentCooldown = base.cooldownTime;
            }
        }
    }
    //implementable
    public class dropPoint : Hack
    {
        public dropPoint(PointControl p) : base(20, 6, "Drop Point")
        {
            points = p;

            output = "DropPoint.exe\n" +
                "Accessing Car Wifi...\n" +
                "Accessing Inventory.py...\n" +
                "Set hasPoint to False...\n" +
                "Hack Succesful\n";
        }

        public override void run(GameObject opponent, PlayerGUI gui)
        {
            if (base.currentCooldown <= 0 && points.getScore() >= pointsRequired)
            {
                showOutput(gui);
                opponent.GetComponent<PointControl>().setHasPoint(false);
                base.currentCooldown = base.cooldownTime;
            }
        }
    }

    public class slowDown : Hack
    {
        public slowDown(PointControl p) : base(5, 1, "Slow")
        {
            points = p;

            output = "SlowDown.exe\n" +
                "Accessing Car Wifi...\n" +
                "Accessing Movement.py...\n" +
                "Speed = Speed/2...\n" +
                "Hack Succesful\n";
        }

        public override void run(GameObject opponent, PlayerGUI gui)
        {
            if(base.currentCooldown <= 0 && points.getScore() >= pointsRequired)
            {
                showOutput(gui);
                opponent.GetComponent<PlayerMovement>().setMovementSpeed(opponent.GetComponent<PlayerMovement>().getMovementSpeed() / 2);
                base.currentCooldown = base.cooldownTime;
            }
        }
    }

    public class Kill : Hack
    {
        ScoreBoard sb;
        GameObject fire;
        public Kill(PointControl p, ScoreBoard sb, GameObject fire) : base(100, 1, "Kill")
        {
            this.sb = sb;
            this.fire = fire;

            points = p;

            output = "Kill.exe\n" +
                "Accessing Car Wifi...\n" +
                "Accessing Data.log...\n" +
                "Increasing Temperature Variable...\n" +
                "Beaglebone Quits Due to High Temp...\n" +
                "Hack Successful\n";
        }

        public override void run(GameObject opponent, PlayerGUI gui)
        {
            if (base.currentCooldown <= 0 && points.getScore() >= pointsRequired)
            {
                showOutput(gui);
                fire.SetActive(true);
                sb.FinalScreen(points.playerNum, 4f);
                base.currentCooldown = base.cooldownTime;
            }
        }
    }

    public class Delay : Hack
    {
        public Delay(PointControl p) : base(8, 1, "Delay")
        {
            points = p;
            //update
            output = "Kill.exe\n" +
                "Accessing Car Wifi...\n" +
                "Accessing Data.log...\n" +
                "Increasing Temperature Variable...\n" +
                "Beaglebone Quits Due to High Temp...\n" +
                "Hack Successful\n";
        }

        public override void run(GameObject opponent, PlayerGUI gui)
        {
            if (base.currentCooldown <= 0 && points.getScore() >= pointsRequired)
            {
                showOutput(gui);
                opponent.GetComponent<PlayerMovement>().setDelay(opponent.GetComponent<PlayerMovement>().getDelay() + 0.1f);
                base.currentCooldown = base.cooldownTime;
            }
        }
    }

    public class Stuck : Hack
    {
        public Stuck(PointControl p) : base(12, 1, "Stuck")
        {
            points = p;
            //update
            output = "Kill.exe\n" +
                "Accessing Car Wifi...\n" +
                "Accessing Data.log...\n" +
                "Increasing Temperature Variable...\n" +
                "Beaglebone Quits Due to High Temp...\n" +
                "Hack Successful\n";
        }

        public override void run(GameObject opponent, PlayerGUI gui)
        {
            if (base.currentCooldown <= 0 && points.getScore() >= pointsRequired)
            {
                showOutput(gui);
                opponent.GetComponent<PlayerMovement>().setDelay(100f);
                base.currentCooldown = base.cooldownTime;
            }
        }
    }

    public class Reset : Hack
    {
        PlayerMovement control;

        public Reset(PlayerMovement control) : base(0, 0, "Reset")
        {
            this.control = control;

            output = "Hacking Detected...\n" +
                "Reset.exe\n" +
                "Reseting All Default Values...\n" +
                "Restarting BeagleBone...\n" +
                "...\n" +
                "Reset Successful\n";
        }

        public override void run(GameObject opponent, PlayerGUI gui)
        {
            if (base.currentCooldown <= 0)
            {
                showOutput(gui);
                control.resetDefaults();
                base.currentCooldown = base.cooldownTime;
            }
        }
    }

    public void setOpponent(GameObject opp)
    {
        opponent = opp;
    }
}
