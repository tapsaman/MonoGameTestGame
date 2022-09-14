using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ZA6.Models;
using TapsasEngine;
using System.Linq;
using TapsasEngine.Utilities;

namespace ZA6.Managers
{
    public class ScenarioManager : StateMachine
    {
        public Scenario CurrentScenario => (Scenario)CurrentState;
        public static string[] RandomOrderScenarios
        {
            get
            {
                string[] randomOrderScenarios = new string[]
                {
                    "Crispy",
                    "Mushroom",
                    "Noise",
                    "Arrows",
                    "Scrambled",
                    "Tape"
                };

                if (IsMoogleMinigameDone)
                    randomOrderScenarios = randomOrderScenarios.Append("TripleA").ToArray();

                return randomOrderScenarios;
            }
        }

        public static string[] PlayedScenarios
        {
            get
            {
                string playedScenarios = Static.GameData.GetString("playedScenarios");
                
                if (playedScenarios == null)
                    return new string[0];
                
                return playedScenarios.Split();
            }
            private set
            {
                Static.GameData.Save("playedScenarios", String.Join(',', value));
            }
        }

        public static bool IsMoogleMinigameDone
        {
            get => Static.GameData.Get("minigame done");
            set => Static.GameData.Save("minigame done", value);

        }

        public ScenarioManager()
            : base(
                new Dictionary<string, State>
                {
                    { "None", new NoneScenario() },
                    { "Tape", new TapeScenario() },
                    { "Noise", new NoiseScenario() }
                },
                "None"
            )
        {}

        public void NextScenario()
        {
            string[] playedScenarios = PlayedScenarios.Append(CurrentStateKey).ToArray();
            string[] randomOrderScenarios = RandomOrderScenarios;
            string[] scenariosLeft = RandomOrderScenarios.Where(s => !playedScenarios.Contains(s)).ToArray();
            
            if (scenariosLeft.Length != 0)
            {
                TransitionTo(scenariosLeft[Utility.RandomBetween(0, scenariosLeft.Length - 1)]);
            }
            else
            {
                TransitionTo("End");
            }
            
            PlayedScenarios = playedScenarios;
        }

        public override void TransitionTo(string newStateName, StateArgs args = null)
        {
            base.TransitionTo(newStateName, args);

            CurrentScenario.Apply(Static.Scene);
        }
    }
}