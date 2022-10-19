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
            int[] occuredReferences = new int[records.Count];
            int referenceIndex = 0;
            // Validate records
            foreach(IRecord record in records)
            {
                // TODO: Use attributes in DefaultRecord so type checking is not neccessary
                if (!(record is DefaultRecord)) continue;
                DefaultRecord defaultRecord = (DefaultRecord)record;

                // Check for duplicate references
                if(occuredReferences.Contains(defaultRecord.Reference))
                {
                    // TODO: Find which index its a duplicate with (multiple if possible!)
                    // TODO: Proper report
                    Console.WriteLine($"Record#{referenceIndex}:{defaultRecord.Reference} has duplicate reference with Record#NA:NA");
                    valid = false;
                }

                occuredReferences[referenceIndex++] = defaultRecord.Reference;

                // Check for incorrect end balance
                if(defaultRecord.StartBalance + defaultRecord.Mutation != defaultRecord.EndBalance)
                {
                    // TODO: proper report
                    Console.WriteLine($"Record#{referenceIndex}:{defaultRecord.Reference} has incorrect end balance '{defaultRecord.EndBalance}'. Expected '{(defaultRecord.StartBalance + defaultRecord.Mutation)}' ({defaultRecord.StartBalance} + {defaultRecord.Mutation}).");
                    valid = false;
                }
            }

            return valid;
        }

    }
}
