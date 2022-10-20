using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFV.Records
{
    // TODO: Use attributes to specify what must be validated and what header names the properties are bound to.

    /// <summary>
    /// The data structure of a default record.
    /// </summary>
    internal struct DefaultRecord : IRecord
    {
        public int Reference { get; set; }
        public string AccountNumber;
        public string Description;
        public decimal StartBalance;
        public decimal Mutation;
        public decimal EndBalance;

        /// <summary>
        /// Converts record into a string including the given index.
        /// </summary>
        /// <param name="index">The index of the row that this record is found in.</param>
        /// <returns>Stringified record (e.g. "Record#1:23456 'Description'")</returns>
        public string ToString(int index)
        {
            return $"Record#{index}:{Reference}";
        }
    }
}
