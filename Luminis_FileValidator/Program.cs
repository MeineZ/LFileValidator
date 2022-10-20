using System;
using System.Collections.Generic;

using LFV.Parsers;
using LFV.Records;
using LFV.Validators;

// Create Validator
Validator validator = new Validator();

// Use parser service to handle parsing different types of files
Console.WriteLine("Loading XML...");
List<IRecord>? xmlRecords = ParserService.Parse("./assets/records.xml");
bool xmlValid = validator.Validate(xmlRecords);
Console.WriteLine($"XML results are{(xmlValid ? String.Empty : "n't")} valid!");

Console.WriteLine("");

Console.WriteLine("Loading CSV...");
List<IRecord>? csvRecords = ParserService.Parse("./assets/records.csv");
bool csvValid = validator.Validate(csvRecords);
Console.WriteLine($"CSV results are{(csvValid ? String.Empty : "n't")} valid!");


// TODO: Create proper report
