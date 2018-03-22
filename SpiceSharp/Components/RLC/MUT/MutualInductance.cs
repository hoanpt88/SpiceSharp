﻿using System;
using SpiceSharp.Attributes;
using SpiceSharp.Behaviors;
using SpiceSharp.Components.MutualInductanceBehaviors;
using SpiceSharp.Simulations;

namespace SpiceSharp.Components
{
    /// <summary>
    /// A mutual inductance
    /// </summary>
    public class MutualInductance : Component
    {
        /// <summary>
        /// Parameters
        /// </summary>
        [ParameterName("inductor1"), ParameterInfo("First coupled inductor")]
        public Identifier InductorName1 { get; set; }
        [ParameterName("inductor2"), ParameterInfo("Second coupled inductor")]
        public Identifier InductorName2 { get; set; }

        /// <summary>
        /// Private variables
        /// </summary>
        public Inductor Inductor1 { get; private set; }
        public Inductor Inductor2 { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the mutual inductance</param>
        public MutualInductance(Identifier name) : base(name, 0)
        {
            // Make sure mutual inductances are evaluated AFTER inductors
            Priority = -1;

            // Add parameters
            ParameterSets.Add(new BaseParameters());

            // Add factories
            Behaviors.Add(typeof(TransientBehavior), () => new TransientBehavior(Name));
            Behaviors.Add(typeof(FrequencyBehavior), () => new FrequencyBehavior(Name));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="inductorName1">Inductor 1</param>
        /// <param name="inductorName2">Inductor 2</param>
        /// <param name="coupling">Mutual inductance</param>
        public MutualInductance(Identifier name, Identifier inductorName1, Identifier inductorName2, double coupling)
            : base(name, 0)
        {
            // Make sure mutual inductances are evaluated AFTER inductors
            Priority = -1;

            // Add parameters
            ParameterSets.Add(new BaseParameters(coupling));

            // Add factories
            Behaviors.Add(typeof(TransientBehavior), () => new TransientBehavior(Name));
            Behaviors.Add(typeof(FrequencyBehavior), () => new FrequencyBehavior(Name));

            // Connect
            InductorName1 = inductorName1;
            InductorName2 = inductorName2;
        }

        /// <summary>
        /// Setup the mutual inductance
        /// </summary>
        /// <param name="simulation">Simulation</param>
        public override void Setup(Simulation simulation)
        {
            if (simulation == null)
                throw new ArgumentNullException(nameof(simulation));

            // Get the inductors for the mutual inductance
            Inductor1 = simulation.Circuit.Objects[InductorName1] as Inductor ?? throw new CircuitException("{0}: Could not find inductor '{1}'".FormatString(Name, InductorName1));
            Inductor2 = simulation.Circuit.Objects[InductorName2] as Inductor ?? throw new CircuitException("{0}: Could not find inductor '{1}'".FormatString(Name, InductorName2));
        }

        /// <summary>
        /// Add inductances to the data provider for setting up behaviors
        /// </summary>
        /// <returns></returns>
        protected override SetupDataProvider BuildSetupDataProvider(ParameterPool parameters, BehaviorPool behaviors)
        {
            // Base execution (will add entity behaviors and parameters for this mutual inductance)
            var data = base.BuildSetupDataProvider(parameters, behaviors);

            // Register inductor 1
            data.Add("inductor1", parameters.GetEntityParameters(InductorName1));
            data.Add("inductor1", behaviors.GetEntityBehaviors(InductorName1));

            // Register inductor 2
            data.Add("inductor2", parameters.GetEntityParameters(InductorName2));
            data.Add("inductor2", behaviors.GetEntityBehaviors(InductorName2));

            return data;
        }
    }
}
