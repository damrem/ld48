using UnityEngine;
using System.Collections.Generic;

namespace Damrem.Profiling {

    public class Timer {

        public struct Tick {
            public string Context;
            public string Label;
            public float Time;
            public override string ToString() {
                return $"{Context}: {Label}: {Time}";
            }
        }

        public List<Tick> Ticks { get; private set; }
        public string Context { get; private set; }
        public Timer(string context) {
            Ticks = new List<Tick>();
            Context = context;
        }

        float TickTime;
        public void DoTick(string label = "Unlabelled") {
            TickTime = Time.time;
            Ticks.Add(new Tick { Context = Context, Label = label, Time = TickTime });
        }

        public void Log() {
            foreach (var t in Ticks) Debug.Log(t);
        }
    }
}