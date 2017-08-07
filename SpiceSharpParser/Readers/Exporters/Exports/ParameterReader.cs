﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceSharp.Simulations;

namespace SpiceSharp.Parser.Readers.Exports
{
    /// <summary>
    /// This class can read device parameters (eg. "@M1[gm]")
    /// </summary>
    public class ParameterReader : IReader
    {
        /// <summary>
        /// Get the last generated object
        /// </summary>
        public object Generated { get; private set; }

        /// <summary>
        /// Read
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="parameters">Parameters</param>
        /// <param name="netlist">Netlist</param>
        /// <returns></returns>
        public bool Read(Token name, List<object> parameters, Netlist netlist)
        {
            string component;
            if (!name.TryReadReference(out component) || parameters.Count != 1)
                return false;

            // Get the name of the component
            string parameter = parameters[0].ReadIdentifier();

            var pe = new ParameterExport(component, parameter);
            netlist.Exports.Add(pe);
            Generated = pe;
            return true;
        }
    }

    /// <summary>
    /// A class that can export a device parameter
    /// </summary>
    public class ParameterExport : Export
    {
        /// <summary>
        /// The component
        /// </summary>
        public string Component { get; }

        /// <summary>
        /// The parameter name
        /// </summary>
        public string Parameter { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="component">The component name</param>
        /// <param name="parameter">The parameter name</param>
        public ParameterExport(string component, string parameter)
        {
            Component = component;
            Parameter = parameter;
        }

        /// <summary>
        /// Get the type name
        /// </summary>
        public override string TypeName => "unknown";

        /// <summary>
        /// The display name of the export
        /// </summary>
        public override string Name => "@" + Component + "[" + Parameter + "]";

        /// <summary>
        /// Extract the data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override object Extract(SimulationData data)
        {
            var c = data.GetComponent(Component);
            return c?.Ask(Parameter, data.Circuit);
        }
    }
}
