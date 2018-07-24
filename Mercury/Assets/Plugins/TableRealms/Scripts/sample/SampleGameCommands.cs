using UnityEngine;

public class SampleGameCommands : ActionBehaviour {

    private long seconds = 0;

    public void Update() {
        long secondsNow = (long)Time.realtimeSinceStartup;
        if (seconds != secondsNow) {
            seconds = secondsNow;
            TableRealmsModel.instance.SetData("timeString", "" + seconds);
            TableRealmsModel.instance.SetData("timeLong", seconds);
            TableRealmsModel.instance.SetData("timeFloat", Time.realtimeSinceStartup);

        }
    }

    public void StartGame() {
        TableRealmsModel.instance.SetData("page", "Controller");
    }
}
