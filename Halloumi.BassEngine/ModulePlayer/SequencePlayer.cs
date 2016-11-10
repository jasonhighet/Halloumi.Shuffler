using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.AudioEngine.Players;

namespace Halloumi.Shuffler.AudioEngine.ModulePlayer
{
    public class SequencePlayer
    {
        public enum ActionType
        {
            PlaySolo,
            Pause,
            Play,
            PauseAll,
            FadeIn,
            FadeOut
        }

        private DateTime _lastEvent = DateTime.Now;
        private double _loopLength;

        private DateTime _nextStep = DateTime.Now;

        private bool _stop;

        public SequencePlayer()
        {
            Actions = new List<Action>();

            StepsPerLoop = 32;
            SetBpm(100);
        }

        public int StepsPerLoop { get; set; }

        public int CurrentStep { get; internal set; }

        public int StepCount { get;  set; }

        public bool IsLooped { get; set; }

        public decimal Bpm { get; set; }

        public double Interval { get; internal set; }

        public List<Action> Actions { get; internal set; }


        private void ProcessStep()
        {
            var currentEvent = DateTime.Now;
            var timeSpan = currentEvent - _lastEvent;
            var secondsSinceLastEvent = Math.Round(timeSpan.TotalSeconds, 4);
            _lastEvent = currentEvent;

            if ((secondsSinceLastEvent - Interval)*1000 > 10)
                DebugHelper.WriteLine("STEP DELAY");

            DebugHelper.WriteLine("OUT:" + Math.Round(secondsSinceLastEvent - Interval, 4));

            ProcessStepActions(CurrentStep);

            CurrentStep++;
            if (CurrentStep != StepCount) return;

            CurrentStep = 0;
            if (!IsLooped) Stop();
        }

        private void ProcessStepActions(int currentStep)
        {
            var actions = Actions.Where(x => x.Step == currentStep).ToList();
            var players = actions.Select(x => x.Player).Distinct();

            ParallelHelper.ForEach(players, player =>
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                var playerActions = actions.Where(x => x.Player == player);
                foreach (var action in playerActions)
                {
                    AudioStream stream;
                    switch (action.ActionType)
                    {
                        case ActionType.PlaySolo:
                            player.Pause();
                            player.QueueSection(action.StreamKey, action.SectionKey);
                            player.Play(action.StreamKey);
                            break;
                        case ActionType.Play:
                            player.QueueSection(action.StreamKey, action.SectionKey);
                            player.Play(action.StreamKey);
                            break;
                        case ActionType.PauseAll:
                            player.Pause();
                            break;
                        case ActionType.Pause:
                            player.Pause(action.StreamKey);
                            break;
                        case ActionType.FadeIn:
                            stream = player.GetAudioStream(action.StreamKey);
                            AudioStreamHelper.SetVolumeSlide(stream, 0f, 1f, action.Length);
                            break;
                        case ActionType.FadeOut:
                            stream = player.GetAudioStream(action.StreamKey);
                            AudioStreamHelper.SetVolumeSlide(stream, 1f, 0f, action.Length);
                            break;
                        default:
                            throw new Exception("Invalid Event Type");
                    }
                }

                stopWatch.Stop();
                DebugHelper.WriteLine("ActionElapsed:" + stopWatch.ElapsedMilliseconds);
            });
        }

        public void Play()
        {
            _loopLength = BpmHelper.GetDefaultLoopLength(Bpm);
            Interval = Math.Round(_loopLength/StepsPerLoop, 4);

            CurrentStep = 0;
            _stop = false;
            _nextStep = DateTime.Now.AddMilliseconds(Interval*1000);

            Task.Run(() => ProcessStep());
            Task.Run(() => PlayLoop());
        }

        private void PlayLoop()
        {
            while (!_stop)
            {
                while ((_nextStep - DateTime.Now).TotalMilliseconds > 0)
                {
                    var sleep = Convert.ToInt32((_nextStep - DateTime.Now).TotalMilliseconds) - 50;
                    if (sleep > 50)
                        Thread.Sleep(sleep);
                }
                _nextStep = DateTime.Now.AddMilliseconds(Interval*1000);
                if (!_stop)
                    Task.Run(() => ProcessStep());
            }
        }

        public void Stop()
        {
            _stop = true;
        }

        public void SetBpm(decimal bpm)
        {
            Bpm = bpm;
        }

        public class Action
        {
            public int Step { get; set; }

            public AudioPlayer Player { get; set; }

            public ActionType ActionType { get; set; }

            public string StreamKey { get; set; }

            public string SectionKey { get; set; }

            public double Length { get; set; }
        }
    }
}