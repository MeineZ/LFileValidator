using System;
using System.Collections.Generic;

using LFV.Parsers;
using LFV.Records;
using LFV.Validators;


// TODO: Create proper reporter to replace console logs.
// TODO: Do validation during parse

// Create Validator
Validator validator = new Validator();


// Parse XML file
Console.WriteLine("Loading XML...");

List<IRecord>? xmlRecords = ParserService.Parse("./assets/records.xml");
bool xmlValid = validator.Validate(xmlRecords);

Console.WriteLine($"XML results are{(xmlValid ? string.Empty : "n't")} valid!");


Console.WriteLine(string.Empty);


// Parse CSV file
Console.WriteLine("Loading CSV...");

List<IRecord>? csvRecords = ParserService.Parse("./assets/records.csv");
bool csvValid = validator.Validate(csvRecords);

Console.WriteLine($"CSV results are{(csvValid ? string.Empty : "n't")} valid!");


