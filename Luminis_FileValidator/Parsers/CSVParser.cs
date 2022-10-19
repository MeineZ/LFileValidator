using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using LFV.Records;

namespace LFV.Parsers
{
    internal class CSVParser : Parser
    {
        char[] newline = Environment.NewLine.ToCharArray();
        char comma = ',';

        public CSVParser()
            : base()
        { }

        protected override List<IRecord> ParseDocument(string path)
        {
            List<IRecord> records = new List<IRecord>();

            string data = string.Empty;
            try
            {
                data = File.ReadAllText(path);
            }
            // All expection type handling is the same in this case.
            catch(Exception ex)
            {
                // TODO: Handle
                Console.WriteLine(ex);
                return records;
            }

            // Assumes lines are divided by THIS environment newline character code(s)
            string[] lines = data.Split(newline);

            bool succesfulHeaderParse = ParseDocumentHeader(lines, out string[] headerCells, out Dictionary<string, List<string>> rawRecordData);
            if (!succesfulHeaderParse)
            {
                // TODO: Proper report
                Console.WriteLine($"No header was found in '{path}'.");
                return records;
            }

            bool gatherRawDataSuccess = GatherRawRecordData(lines, headerCells, rawRecordData);
            if (!gatherRawDataSuccess)
            {
                // TODO: Proper report
                Console.WriteLine($"Gathering data failed in '{path}'.");
                return records;
            }

            bool parseRawDataSuccess = ParseRawData(rawRecordData, headerCells, records);
            if (!parseRawDataSuccess)
            {
                // TODO: Proper report
                Console.WriteLine($"Parsing raw data failed in '{path}'.");
                return records;
            }

            return records;
        }

        private bool ParseDocumentHeader(string[] lines, out string[] headerCells, out Dictionary<string, List<string>> rawRecordData)
        {
            headerCells = new string[lines.Length];
            rawRecordData = new Dictionary<string, List<string>>();

            if (lines.Length > 0) return false;
            if (headerCells.Length <= 0) return false;

            foreach (string headerCell in headerCells)
            {
                if (!rawRecordData.ContainsKey(headerCell))
                {
                    rawRecordData.Add(headerCell, new List<string>());
                }
            }
            return true;
        }

        private bool GatherRawRecordData(string[] lines, string[] headerCells, Dictionary<string, List<string>> rawRecordData)
        {
            if (!(lines?.Any() ?? false)) return false;
            if (!(headerCells?.Any() ?? false)) return false;
            if (rawRecordData == null) return false;

            foreach (string line in lines.Skip(1))
            {
                string[] rawRecordCells = line.Split(comma);
                for (int i = 0; i < rawRecordCells.Length; ++i)
                {
                    if (i >= headerCells.Length) break;
                    // TODO: Vulnerable to duplicate header names
                    rawRecordData[headerCells[i]].Add(rawRecordCells[i]);
                }
            }

            return true;
        }

        private bool ParseRawData(Dictionary<string, List<string>> rawRecordData, string[] headerCells, List<IRecord> records)
        {
            if (!(headerCells?.Any() ?? false)) return false;
            if (!(rawRecordData?.Any() ?? false)) return false;
            if (records == null) return false;

            // TODO: Find better way for looping through entries
            for (int i = 0; i < rawRecordData[headerCells[0]].Count; ++i)
            {
                try
                {
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
                    // TODO: Handle
                    Console.WriteLine(fmtEx);
                }
            }

            return true;
        }
    }
}
