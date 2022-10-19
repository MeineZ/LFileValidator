using System;
using System.Collections.Generic;
using System.IO;

using LFV.Records;

namespace LFV.Parsers
{
    /// <summary>
    /// Allows for easy parsing of data files without the need to construct 
    /// instances like <see cref="Parser"/>.<br/>
    /// Use <see cref="ParserService.Parse(string)"/>.
    /// </summary>
    internal static class ParserService
    {
        // Parsers
        private static XMLParser xmlParser = new XMLParser();
        private static CSVParser csvParser = new CSVParser();

        /// <summary>
        /// Parses data in a file at the given path.
        /// </summary>
        /// <param name="path">The path to the file where data should be parsed from.</param>
        /// <returns>
        /// A list of <see cref="IRecord"/>s.
        /// This list is empty when the file is not formatted well.
        /// </returns>
        public static List<IRecord>? Parse(string path)
        {
            string fileExtension = Path.GetExtension(path);
            switch(fileExtension)
            {
                case ".csv": return csvParser!.Parse(path);
                case ".xml": return xmlParser!.Parse(path);
                default: 
                    return null;
            }
        }
    }
}
