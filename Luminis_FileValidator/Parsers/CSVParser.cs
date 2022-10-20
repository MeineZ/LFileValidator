using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using LFV.Records;

namespace LFV.Parsers
{
    /// <summary>
    /// Allows for an XML data file to be parsed into a collection of <see cref="IRecord"/>s.
    /// </summary>
    internal class CSVParser : Parser
    {
        // Define constants
        char[] newline = Environment.NewLine.ToCharArray();
        char comma = ',';

        /// <summary>
        /// Parses the document into a list of <see cref="IRecord"/>s by loading the CSV document 
        /// and looping through its records.
        /// </summary>
        /// <param name="path">A validated path to the document. Guaranteed to be valid.</param>
        /// <returns>
        /// A <see cref="List{T}"/> of parsed <see cref="IRecord"/>s. 
        /// <c>null</c> when no data was found or a parsing error occured.
        /// </returns>
        protected override List<IRecord> ParseDocument(string path)
        {
            List<IRecord> records = new List<IRecord>();

            // Load csv file as text
            string data = LoadDocumentContent(path);
            if (string.IsNullOrEmpty(data))
                return records;

            // Split file into lines
            // Assumes lines are divided by THIS environment newline character code(s)
            string[] lines = data.Split(newline);

            // Parse header
            bool succesfulHeaderParse = ParseDocumentHeader(lines[0], out string[] headerCells, out Dictionary<string, List<string>> rawRecordData);
            if (!succesfulHeaderParse)
            {
                // TODO: Proper report
                Console.WriteLine($"Header row is empty in '{path}'.");
                return records;
            }


            // Parse data
            bool gatherRawDataSuccess = GatherRawRecordData(lines, headerCells, rawRecordData);
            if (!gatherRawDataSuccess)
            {
                // TODO: Proper report
                Console.WriteLine($"Gathering data failed in '{path}'.");
                return records;
            }

            // Parse gathered data into actual record instances
            bool parseRawDataSuccess = ParseRawData(rawRecordData, records);
            if (!parseRawDataSuccess)
            {
                // TODO: Proper report
                Console.WriteLine($"Parsing raw data failed in '{path}'.");
                return records;
            }

            return records;
        }

        /// <summary>
        /// Loads the csv content as text.
        /// Logs exception to console if anything went wrong.
        /// </summary>
        /// <param name="path">The path to the csv file.</param>
        /// <returns>
        /// String containing the file contents as text.<br/>
        /// Empty if the document failed to load or if the document is empty.
        /// </returns>
        private string LoadDocumentContent(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            // All expection type handling is the same in this case.
            catch (Exception ex)
            {
                // TODO: Proper report
                Console.WriteLine(ex);
            }

            return string.Empty;
        }

        /// <summary>
        /// Parses all cells from the given header row.
        /// Then it constructs a dictionary by using the header cells as keys.
        /// </summary>
        /// <param name="headerLine">The text on the first line of the document that is supposedly the header.</param>
        /// <param name="headerCells">[out] All cells in the header that present the header of each column.</param>
        /// <param name="rawRecordData">[out] A dictionary constructed from the header cells with empty lists.</param>
        /// <returns>
        /// <c>false</c> if the header row is empty.<br/>
        /// <c>true</c> when parse was succesful.
        /// </returns>
        private bool ParseDocumentHeader(string headerLine, out string[] headerCells, out Dictionary<string, List<string>> rawRecordData)
        {
            // Set up out parameters before the first return
            headerCells = string.IsNullOrEmpty(headerLine.Trim())
                ? new string[0]
                : headerLine!.Split(comma);
            rawRecordData = new Dictionary<string, List<string>>();

            // Validate header
            if (headerCells.Length <= 0) return false;

            // Construct dictionary by adding header cells as keys.
            foreach (string headerCell in headerCells)
            {
                if (!rawRecordData.ContainsKey(headerCell))
                {
                    rawRecordData.Add(headerCell, new List<string>());
                }
            }
            return true;
        }

        /// <summary>
        /// Gathers the data of each row and moves it into the corresponding list inside the dictionary.
        /// </summary>
        /// <param name="lines">All lines/rows from the loaded file, including the header row.</param>
        /// <param name="headerCells">The header cells to identify data with the correct column.</param>
        /// <param name="rawRecordData">The dictionary with header cells as keys, to fill with data.</param>
        /// <returns>
        /// <c>false</c> if any of the parameters are null or empty.<br/>
        /// <c>true</c> after all rows have been processed.
        /// </returns>
        private bool GatherRawRecordData(string[] lines, string[] headerCells, Dictionary<string, List<string>> rawRecordData)
        {
            // Validate parameters
            if (!(lines?.Any() ?? false)) return false;
            if (!(headerCells?.Any() ?? false)) return false;
            if (rawRecordData == null) return false;

            // Process through all rows
            foreach (string line in lines.Skip(1))
            {
                if(string.IsNullOrEmpty(line)) continue;

                string[] rawRecordCells = line.Split(comma);
                for (int i = 0; i < rawRecordCells.Length; ++i)
                {
                    // For each cell in the row, check if there is a corresponding header cell (not out of bounds)
                    if (i >= headerCells.Length) break;
                    // TODO: Vulnerable to duplicate header names
                    rawRecordData[headerCells[i]].Add(rawRecordCells[i]);
                }
            }

            return true;
        }

        /// <summary>
        /// Goes through the given raw record data and converts it all to <see cref="DefaultRecord"/>s.
        /// </summary>
        /// <param name="rawRecordData">The raw record data that has been parsed from the CSV file.</param>
        /// <param name="records">The list to push records to.</param>
        /// <returns>
        /// <c>false</c> if any parameter is null or empty (records may be empty).<br/>
        /// <c>true</c> after all data has been processed.
        /// </returns>
        private bool ParseRawData(Dictionary<string, List<string>> rawRecordData, List<IRecord> records)
        {
            // Validate parameters
            if (!(rawRecordData?.Any() ?? false)) return false;
            if (records == null) return false;

            // TODO: Find better way for looping through entries (while gathering raw data perhaps?)
            for (int i = 0; i < rawRecordData.First().Value.Count; ++i)
            {
                try
                {
                    // TODO: Generic type should be supported here (perhaps in combination with attributes).
                    DefaultRecord newRecord = new DefaultRecord()
                    {
                        Reference = Int32.Parse(rawRecordData["Reference"][i]),
                        AccountNumber = rawRecordData["Account Number"][i],
                        Description = rawRecordData["Description"][i],
                        StartBalance = Decimal.Parse(rawRecordData["Start Balance"][i]),
                        Mutation = Decimal.Parse(rawRecordData["Mutation"][i]),
                        EndBalance = Decimal.Parse(rawRecordData["End Balance"][i]),
                    };

                    records.Add(newRecord);
                }
                catch (FormatException fmtEx)
                {
                    // TODO: Proper report
                    Console.WriteLine(fmtEx);
                }
            }

            return true;
        }
    }
}
