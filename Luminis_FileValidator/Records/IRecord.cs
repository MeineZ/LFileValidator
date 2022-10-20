
namespace LFV.Records
{
    /// <summary>
    /// Interface to identify a record of any type.
    /// </summary>
    interface IRecord
    {
        /// <summary>
        /// The reference number of this record.
        /// </summary>
        int Reference { get; set; }

        /// <summary>
        /// Converts record into a string including the given index.
        /// </summary>
        /// <param name="index">The index of the row that this record is found in.</param>
        /// <returns>Stringified record (e.g. "Record#1:23456 'Description'")</returns>
        string ToString(int index);
    }
}
