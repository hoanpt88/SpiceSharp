﻿using System.Collections.Generic;
using SpiceSharp.Components;

namespace SpiceSharp.Parser.Readers
{
    /// <summary>
    /// This class can read current-controlled current sources
    /// </summary>
    public class CurrentControlledCurrentsourceReader : IReader
    {
        /// <summary>
        /// The last generated object
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
            if (name.image[0] != 'f' && name.image[0] != 'F')
                return false;

            CurrentControlledCurrentsource cccs = new CurrentControlledCurrentsource(name.ReadWord());
            cccs.ReadNodes(parameters, 2);
            switch (parameters.Count)
            {
                case 2: throw new ParseException(parameters[1], "Voltage source expected");
                case 3: throw new ParseException(parameters[2], "Value expected");
            }

            cccs.Set("control", parameters[2].ReadWord());
            cccs.Set("gain", parameters[3].ReadValue());

            Generated = cccs;
            netlist.Circuit.Components.Add(cccs);
            return true;
        }
    }
}
