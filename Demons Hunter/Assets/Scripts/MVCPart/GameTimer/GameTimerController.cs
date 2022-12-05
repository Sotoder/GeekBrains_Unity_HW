using PlayFab.ClientModels;
using PlayFab;
using System;
using System.Collections.Generic;

public class GameTimerController: IUpdateble, IClearable
{
    private TimerModel _timerModel;
    private float _timeSpent;
    private float _finalTime;
    private float _bestTime;
    private string _displayName;

    public GameTimerController (TimerModel timerModel)
    {
        _timerModel = timerModel;
        _timerModel.Boss.OnBossDead += LevelEnd;
        _timerModel.WinGamePanel.LeadersNameText.text = "";
        _timerModel.WinGamePanel.SelfPosition.text = "";
        _timerModel.WinGamePanel.LeadersTimeText.text = "";

        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest(), OnGetPlayerProfile, OnError);
    }

    private void OnGetPlayerProfile(GetPlayerProfileResult result)
    {
        _displayName = result.PlayerProfile.DisplayName;
    }

    private void LevelEnd()
    {
        _finalTime = _timeSpent;

        PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = PlayFabConstants.BEST_TIME_ON_LEVEL_SPENT,
            MaxResultsCount = 1
        }, OnSelfPositionLoaded, OnError);

    }

    private void OnSelfPositionLoaded(GetLeaderboardAroundPlayerResult result)
    {
        var leaderboard = result.Leaderboard;
        _bestTime = (float)leaderboard[0].StatValue / -100;
        var bestSpan = TimeSpan.FromSeconds(_bestTime);
        var currentSpan = TimeSpan.FromSeconds(_finalTime);

        var selfString = "Current time:" + string.Format("{0:00}:{1:00}:{2:00}", (int)currentSpan.TotalHours, currentSpan.Minutes, currentSpan.Seconds) + "\n";

        if (leaderboard[0].StatValue != 0)
        {
            selfString += "Past best result: " + string.Format("{0:00}:{1:00}:{2:00}", (int)bestSpan.TotalHours, bestSpan.Minutes, bestSpan.Seconds);
        }
        else
        {
            selfString += "It's your first time!";
        }

        _timerModel.WinGamePanel.SelfPosition.text = selfString;

        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = PlayFabConstants.BEST_TIME_ON_LEVEL_SPENT, Value = (int)(_finalTime * -100)}
            }
        }, OnStatisticsUpdated, OnError);
    }

    private void OnStatisticsUpdated(UpdatePlayerStatisticsResult result)
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = PlayFabConstants.BEST_TIME_ON_LEVEL_SPENT,
            MaxResultsCount = 10

        }, OnLeaderBoardLoaded, OnError);
    }

    private void OnLeaderBoardLoaded(GetLeaderboardResult result)
    {
        var leaderboard = result.Leaderboard;
        var positionShift = 1;
        var isSelfResultsAdded = false;

        for (int i = 0; i < leaderboard.Count; i++)
        {
            int finalInt = (int)(_finalTime * 100);
            int bestInt = (int)(_bestTime * 100);
            int leaderInt = -leaderboard[i].StatValue;
            float leaderTime = (float)leaderboard[i].StatValue / -100;
            var leadNameString = "";
            var leadTimeString = "";

            if (finalInt < bestInt && leaderInt >= finalInt && !isSelfResultsAdded)
            {
                var currentSpan = TimeSpan.FromSeconds(_finalTime);
                leadNameString = $"{leaderboard[i].Position + positionShift}. {_displayName}:\n";
                leadTimeString = string.Format("{0:00}:{1:00}:{2:00}", (int)currentSpan.TotalHours,
                    currentSpan.Minutes, currentSpan.Seconds) + "\n";
                positionShift++;
                isSelfResultsAdded = true;
                _timerModel.WinGamePanel.LeadersNameText.text += leadNameString;
                _timerModel.WinGamePanel.LeadersTimeText.text += leadTimeString;
            }

            if (finalInt < bestInt && _displayName == leaderboard[i].DisplayName)
            {
                positionShift--;
                continue;
            }

            var span = TimeSpan.FromSeconds(leaderTime);
            leadNameString = $"{leaderboard[i].Position + positionShift}. {leaderboard[i].DisplayName}:\n";
            leadTimeString = string.Format("{0:00}:{1:00}:{2:00}", (int)span.TotalHours,
                span.Minutes, span.Seconds) + "\n";
            _timerModel.WinGamePanel.LeadersNameText.text += leadNameString;
            _timerModel.WinGamePanel.LeadersTimeText.text += leadTimeString;
        }

        _timerModel.WinGamePanel.Show();
    }

    public void Update(float deltaTime)
    {
        _timeSpent += deltaTime;
        var span = TimeSpan.FromSeconds(_timeSpent);
        _timerModel.TimerText.text = string.Format("{0:00}:{1:00}:{2:00}", (int)span.TotalHours, span.Minutes, span.Seconds);
    }

    private void OnError(PlayFabError error)
    {
    }


    public void Clear()
    {
        _timerModel.Boss.OnBossDead -= LevelEnd;
    }
}
