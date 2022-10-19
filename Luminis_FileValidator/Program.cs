using System;
using System.Collections.Generic;

using LFV.Parsers;
using LFV.Records;
using LFV.Validators;


// Use parser service to handle parsing different types of files
List<IRecord>? xmlRecords = ParserService.Parse("./assets/records.xml");
List<IRecord>? csvRecords = ParserService.Parse("./assets/records.csv");


// Create Validator
Validator validator = new Validator();
// Validate collection
bool xmlValid = validator.Validate(xmlRecords);
bool csvValid = validator.Validate(csvRecords);


// Print report
// TODO: Create proper report
Console.WriteLine($"XML results are{(xmlValid ? String.Empty : "n't")} valid!");
Console.WriteLine($"CSV results are{(csvValid ? String.Empty : "n't")} valid!");
