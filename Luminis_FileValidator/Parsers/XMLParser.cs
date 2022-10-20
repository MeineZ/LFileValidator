using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using LFV.Records;

namespace LFV.Parsers
{
    /// <summary>
    /// Allows for an XML data file to be parsed into a collection of <see cref="IRecord"/>s.
    /// 
    /// </summary>
    internal class XMLParser : Parser
    {
        /// <summary>
        /// Parses the document into a list of <see cref="IRecord"/>s by loading the XML document 
        /// and looping through its records.
        /// </summary>
        /// <param name="path">A validated path to the document. Guaranteed to be valid.</param>
        /// <returns>
        /// A <see cref="List{T}"/> of parsed <see cref="IRecord"/>s. 
        /// It's empty when no data was found or a parsing error occured.
        /// </returns>
        protected override List<IRecord> ParseDocument(string path)
        {
            List<IRecord> records = new List<IRecord>();

            // Load and find root element of document at given path
            XmlElement? rootElement = LoadRootElement(path);
            if(rootElement == null) return records;

            // Find collection of records
            XmlNode? recordsCollectionNode = rootElement.SelectSingleNode("/records");
            if(recordsCollectionNode == null) return records;

            // Parse record collection
            ParseXmlRecords(recordsCollectionNode, records);

            return records;
        }

        /// <summary>
        /// Loads the XML document and finds the root element if possible.
        /// </summary>
        /// <param name="path">A validated path to the document. Guaranteed to be valid.</param>
        /// <returns>
        /// The <see cref="XmlElement"/> (root) of the document at the given path.<br/>
        /// <c>null</c> if loading the file went wrong or if the root element couldn't be found.
        /// </returns>
        private XmlElement? LoadRootElement(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(path);
            }
            catch (XmlException xmlEx)
            {
                // TODO: Proper report
                Console.WriteLine(xmlEx);
                return null;
            }
            catch (FileNotFoundException fnfEx)
            {
                // TODO: Proper report
                Console.WriteLine(fnfEx);
                return null;
            }

            return xmlDocument.DocumentElement;
        }

        /// <summary>
        /// Parses the collection of record nodes into the given list.
        /// Skips the record if it was not formatted correctly.
        /// </summary>
        /// <param name="recordsCollection">The node which is supposedly the collection of record nodes.</param>
        /// <param name="records">The list of <see cref="IRecord"/>s to add parsed records to.</param>
        private void ParseXmlRecords(XmlNode recordsCollection, List<IRecord> records)
        {
            if(recordsCollection == null) return;
            if (records == null) return;

            foreach (XmlNode recordNode in recordsCollection.ChildNodes)
            {
                try
                {
                    DefaultRecord newRecord = new DefaultRecord()
                    {
                        Reference = Int32.Parse(recordNode.Attributes?["reference"]?.InnerText ?? "-1"),
                        AccountNumber = recordNode.SelectSingleNode("accountNumber")?.InnerText ?? "-",
                        Description = recordNode.SelectSingleNode("description")?.InnerText ?? "-",
                        StartBalance = Decimal.Parse(recordNode.SelectSingleNode("startBalance")?.InnerText ?? "0.0"),
                        Mutation = Decimal.Parse(recordNode.SelectSingleNode("mutation")?.InnerText ?? "0.0"),
                        EndBalance = Decimal.Parse(recordNode.SelectSingleNode("endBalance")?.InnerText ?? "0.0")
                    };

                    records.Add(newRecord);
                }
                catch (FormatException fmtEx)
                {
                    // TODO: Proper report
                    Console.WriteLine(fmtEx);
                }
            }
        }

    }
}
