using System.Collections.Generic;
using System.IO;

using LFV.Records;

namespace LFV.Parsers
{
    /// <summary>
    /// Parses a certain data file into a collection of <see cref="IRecord"/>s.
    /// </summary>
    internal abstract class Parser
    {
        public Parser()
        { }

        /// <summary>
        /// Parse a document into a collection of <see cref="IRecord"/>s.<br/>
        /// First validates the path to make sure we are working with a proper path.
        /// Then uses <see cref="ParseDocument(string)"/> to parse the document in the correct way.
        /// </summary>
        /// <param name="path">The path of the document to parse.</param>
        /// <returns>
        /// A <see cref="List{T}"/> of parsed <see cref="IRecord"/>s. 
        /// It's empty when no data was found or a parsing error occured.
        /// </returns>
        public List<IRecord> Parse(string path)
        {
            // Make sure we have a path 
            if (!ValidatePath(path)) return new List<IRecord>();

            // Process document
            return ParseDocument(path);
        }

        /// <summary>
        /// Validate the given path by checking properties like:<br/>
        ///   - Null or empty<br/>
        ///   - Whether it has an extension
        /// </summary>
        /// <param name="path">The path to validate.</param>
        /// <returns><c>true</c> if the path is valid. <c>false</c> if it isn't.</returns>
        private bool ValidatePath(string path)
        {
            if(string.IsNullOrEmpty(path)) return false;
            if (!Path.HasExtension(path)) return false;

            return true;
        }

        /// <summary>
        /// Allows custom parsing for custom document types.
        /// </summary>
        /// <param name="path">A validated path to the document. Guaranteed to be valid.</param>
        /// <returns>
        /// A <see cref="List{T}"/> of parsed <see cref="IRecord"/>s. 
        /// It's empty when no data was found or a parsing error occured.
        /// </returns>
        protected abstract List<IRecord> ParseDocument(string path);

    }
}
