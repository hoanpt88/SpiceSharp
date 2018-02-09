﻿using System;
using SpiceSharp.NewSparse.Matrix;

namespace SpiceSharp.NewSparse
{
    /// <summary>
    /// Template for a solver
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Solver<T> where T : IFormattable
    {
        /// <summary>
        /// Number of fill-ins in the matrix generated by the solver
        /// </summary>
        public int Fillins { get; private set; }

        /// <summary>
        /// Gets or sets a flag that reordering is required
        /// </summary>
        public bool NeedsReordering { get; set; }

        /// <summary>
        /// Gets the matrix to work on
        /// </summary>
        public Matrix<T> Matrix { get; }

        /// <summary>
        /// Gets the right-hand side
        /// </summary>
        public Vector<T> Rhs
        {
            get
            {
                if (rhs.Length != Matrix.Size + 1)
                    rhs = new Vector<T>(Matrix.Size + 1);
                return rhs;
            }
        }
        Vector<T> rhs;

        /// <summary>
        /// Constructor
        /// </summary>
        public Solver()
        {
            Matrix = new Matrix<T>();
            rhs = new Vector<T>(1);
        }

        /// <summary>
        /// Solve
        /// </summary>
        /// <param name="solution">Solution vector</param>
        public abstract void Solve(Vector<T> solution);

        /// <summary>
        /// Solve the transposed problem
        /// </summary>
        /// <param name="solution">Solution vector</param>
        public abstract void SolveTransposed(Vector<T> solution);

        /// <summary>
        /// Factor the matrix
        /// </summary>
        public abstract void Factor();

        /// <summary>
        /// Order and factor the matrix
        /// </summary>
        public abstract void OrderAndFactor();

        /// <summary>
        /// Create a fillin
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="column">Column</param>
        /// <returns></returns>
        protected virtual Element<T> CreateFillin(int row, int column)
        {
            var result = Matrix.GetElement(row, column);
            Fillins++;
            return result;
        }
    }
}
