using LFV.Parsers;
using LFV.Records;
using LFV.Validators;

// TODO: Use parser service to handle parsing different types of files
// Create parser
XMLParser xmlParser = new XMLParser();
CSVParser csvParser = new CSVParser();

// Parse file into collection of entry type
List<IRecord> xmlRecords = xmlParser.Parse("./assets/records.xml");
List<IRecord> csvRecords = csvParser.Parse("./assets/records.csv");


// Create Validator
Validator validator = new Validator();
// Validate collection
bool xmlValid = validator.Validate(xmlRecords);
bool csvValid = validator.Validate(csvRecords);


// Print report
// TODO: Create proper report
Console.WriteLine($"Number of XML results: "+ xmlRecords.Count);
Console.WriteLine($"XML results are{(xmlValid ? String.Empty : "n't")} valid!");

Console.WriteLine($"Number of CSV results: " + csvRecords.Count);
Console.WriteLine($"CSV results are{(csvValid ? String.Empty : "n't")} valid!");
