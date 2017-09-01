﻿using System;

namespace SpiceSharp.Components
{
    /// <summary>
    /// Attribute when a voltage is applied by a component
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class VoltageDriver : Attribute
    {
        /// <summary>
        /// The pin connected to the positive side of the voltage source
        /// </summary>
        public int Positive { get; }

        /// <summary>
        /// The pin connected to the negative side of the voltage source
        /// </summary>
        public int Negative { get; }

        /// <summary>
        /// Constructor
        /// This attribute specifies that two nodes are connected by a voltage source. It can
        /// be used for checking voltage loops.
        /// </summary>
        /// <param name="pos">The positive output pin index</param>
        /// <param name="neg">The negative output pin index</param>
        public VoltageDriver(int pos, int neg)
        {
            Positive = pos;
            Negative = neg;
        }
    }
}
