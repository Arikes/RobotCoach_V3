using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ms_video_ctrol : MonoBehaviour {
    public MediaPlayer _mediaPlayer;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickPlay()
    {
        if (_mediaPlayer)
        {
            _mediaPlayer.Control.Play();
        }
    }
    public void OnClickStop()
    {
        if (_mediaPlayer)
        {
            _mediaPlayer.Control.Pause();
        }
    }
    public void OnClickReStart()
    {
        if (_mediaPlayer)
        {
            _mediaPlayer.Control.Rewind();
            _mediaPlayer.Control.Play();
        }
    }
}
