﻿using SpiceSharp.Circuits;
using SpiceSharp.Components;

namespace SpiceSharp.Behaviours.DcLoad
{
    public class CurrentSourceLoadDcBehaviour : CircuitObjectBehaviorDcLoad
    {
        public override void Execute(Circuit ckt)
        {
            var currentSource = ComponentTyped<Currentsource>();

            var state = ckt.State;
            var rstate = state.Real;

            double value = 0.0;
            double time = 0.0;

            // Time domain analysis
            if (state.Domain == CircuitState.DomainTypes.Time)
            {
                if (ckt.Method != null)
                    time = ckt.Method.Time;

                // Use the waveform if possible
                if (currentSource.ISRCwaveform != null)
                    value = currentSource.ISRCwaveform.At(time);
                else
                    value = currentSource.ISRCdcValue * state.SrcFact;
            }
            else
            {
                // AC or DC analysis use the DC value
                value = currentSource.ISRCdcValue * state.SrcFact;
            }

            rstate.Rhs[currentSource.ISRCposNode] += value;
            rstate.Rhs[currentSource.ISRCnegNode] -= value;
            currentSource.Current = value;
        }
    }
}
