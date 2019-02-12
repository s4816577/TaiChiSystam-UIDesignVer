using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaybackState
{
    void Update();

    void Play();

    void Pause();

    void Restart();

    void SpeedUp();

    void SpeedDown();

    void Next();

    void Last();

    void NextMovement();

    void LastMovement();

    void NextAction();

    void LastAction();

	void SetRestartInd(int Ind);

    bool CanPlayActionAudio();
}
