using LFV.Attributes;

namespace LFV.Records
{
    // TODO: Use attributes to specify what must be validated and what header names the properties are bound to.

    /// <summary>
    /// The data structure of a default record.
    /// </summary>
    internal struct DefaultRecord : IRecord
    {
        [ParsableField(xmlName: "Reference", csvName: "reference")]
        public int Reference;

        [ParsableField(xmlName: "Account Number", csvName: "accountNumber" )]
        public string AccountNumber;

        [ParsableField(xmlName: "Description", csvName: "description")]
        public string Description;

        [ParsableField(xmlName: "Start Balance", csvName: "startBalance")]
        public decimal StartBalance;

        [ParsableField(xmlName: "Mutation", csvName: "mutation")]
        public decimal Mutation;

        [ParsableField(xmlName: "End Balance", csvName: "endBalance")]
        public decimal EndBalance;

    }
}
