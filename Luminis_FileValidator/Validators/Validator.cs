using System;
using System.Collections.Generic;
using System.Linq;

using LFV.Records;

namespace LFV.Validators
{

    /// <summary>
    /// This class allows you to validate a collection of <see cref="IRecord"/>s using
    /// <see cref="Validate(IEnumerable{IRecord})"/>.
    /// </summary>
    internal class Validator
    {
        // TODO: Allow for easy multi-type validation (a list may consist of multiple types, question is.. Is the collection still valid in that case? Should all data be in the same format?).


        /// <summary>
        /// Validates the given collection of <see cref="IRecord"/>s.<br/>
        /// 
        /// Currently limited to a collection of <see cref="DefaultRecord"/>s.<br/>
        /// Checks for duplicate references and incorrect end balances and logs invalid entries to the console.<br/>
        /// </summary>
        /// <param name="records">The collection of <see cref="IRecord"/>s to validate.</param>
        /// <returns><c>true</c> if collection is completely valid.</returns>
        public bool Validate(List<IRecord>? records)
        {
            bool valid = true;
            if (!(records?.Any() ?? false)) return valid;

            // Keep track of references in this collection to easily check for duplicates.
            List<int> occuredReferences = new List<int>(records.Count);

            // Validate records
            for(int i = 0; i < records.Count; ++i)
            {
                // TODO: Use attributes in DefaultRecord so type checking is not neccessary
                if (!(records[i] is DefaultRecord)) continue;
                DefaultRecord currentRecord = (DefaultRecord)records[i];

                // Check for duplicate references
                // TODO: Find all references instead of just one.
                int foundIndex = occuredReferences.FindIndex(reference => reference == currentRecord.Reference);
                if (foundIndex >= 0)
                {
                    ReportInvalidation(currentRecord, i, $"Duplicate reference with {records[foundIndex].ToString(foundIndex)}");
                    valid = false;
                }

                occuredReferences.Add(currentRecord.Reference);

                // Check for incorrect end balance
                if(currentRecord.StartBalance + currentRecord.Mutation != currentRecord.EndBalance)
                {
                    ReportInvalidation(currentRecord, i, $"Incorrect end balance '{currentRecord.EndBalance}'. Expected '{(currentRecord.StartBalance + currentRecord.Mutation)}' ({currentRecord.StartBalance} + {currentRecord.Mutation}).");
                    valid = false;
                }
            }

            return valid;
        }

        /// <summary>
        /// Allows for consistent invalidation reporting.
        /// Prefixes the given description with the record info and prints additional info on a new line.
        /// </summary>
        /// <param name="record">The record that has been invalidated.</param>
        /// <param name="index">The index of the record that has been invalidated.</param>
        /// <param name="desc">Additional information about the invalidation.</param>
        private void ReportInvalidation(IRecord record, int index, string desc)
        {
            // TODO: Needs proper reporting system
            if (!(record is DefaultRecord)) return;

            Console.WriteLine($"{record.ToString(index)} '{((DefaultRecord)record).Description}' is invalid:\n\t {desc} \n");
        }
    }
}
