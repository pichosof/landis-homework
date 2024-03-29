﻿// See https://aka.ms/new-console-template for more information

using LadisEndpointBLL.Err;
using LadisEndpointBLL.Err.Core;
using LadisEndpointBLL.Services.Console;
using LadisEndpointBLL.Services.Interface;
using LadisEndpointBLL.Util;
using LadisEndpointBLL.Util.Validators.Console;
using LadisEndpointModel;
using System.Diagnostics;


IEndpointService endpointService = new EndpointServiceConsole();
bool cont = true;
Console.WriteLine("Hello, this is a Teste from Landis! Coded by Jonathan Mendonça");
Console.WriteLine("Thanks for the oportunity!");

while (cont)
{
    Console.WriteLine("Please, type one of the options below to continue");
    Console.WriteLine(Space());
    Console.WriteLine("1) Insert a new endpoint");
    Console.WriteLine("2) Edit an existing endpoint");
    Console.WriteLine("3) Delete an existing endpoint");
    Console.WriteLine("4) List all endpoints");
    Console.WriteLine("5) Find a endpoint by 'Endpoint Serial Number'");
    Console.WriteLine("6) Exit");
    cont = LinkDistribution(endpointService);
}

Process.GetCurrentProcess().Kill();

static bool LinkDistribution(IEndpointService endpointService)
{    
    try
    {
        EndPoint endPoint = new();
        string? opt = Console.ReadLine();
        switch (opt)
        {
            case "1":
                Console.WriteLine(Space());   
                Console.WriteLine("Insert a Serial Number");
                endPoint.SerialNumber = Console.ReadLine();
                bool validateModelId = true;
                while (validateModelId)
                {
                    Console.WriteLine("Chose one option for ModelId");
                    ConsoleValidators.CallOptAtributes<ModelId>();

                    var optId = Console.ReadLine();
                    var validation = ConsoleValidators.ValidateNumberOptWithEnum<ModelId>(optId);
                    validateModelId = validation.Item1;
                    endPoint.ModelId = !validateModelId ? validation.Item2 : 0;                                
                }

                Console.WriteLine("Insert a FirmwareVersion");
                endPoint.FirmwareVersion = Console.ReadLine();

                bool validateNumber = true;
                while (validateNumber)
                {
                    Console.WriteLine("Insert a Number");
                    var number = Console.ReadLine();
                    var validation = ConsoleValidators.ValidateNumberOpt(number);
                    validateNumber = validation.Item1;
                    endPoint.Number = !validateNumber ? validation.Item2 : 0;                 
                }

                bool validateState = true;
                while (validateState)
                {
                    Console.WriteLine("Chose one option for State");
                    ConsoleValidators.CallOptAtributes<State>();
                    var optId = Console.ReadLine();
                    var validation = ConsoleValidators.ValidateNumberOptWithEnum<State>(optId);
                    validateState = validation.Item1;
                    endPoint.State = !validateState ? validation.Item2 : 0;
                }
                var resultInsert = endpointService.InsertEndPoint(endPoint);
                Console.WriteLine(resultInsert.ToString());
                return true;
            case "2":
                Console.WriteLine(Space());
                Console.WriteLine("Insert a Serial Number to Edit a EndPoint");
                endPoint.SerialNumber = Console.ReadLine();
                endPoint = endpointService.FindEndpoint(endPoint);
                validateState = true;
                while (validateState)
                {
                    Console.WriteLine("Chose one option for State");
                    ConsoleValidators.CallOptAtributes<State>();
                    var optId = Console.ReadLine();
                    var validation = ConsoleValidators.ValidateNumberOptWithEnum<State>(optId);
                    validateState = validation.Item1;
                    endPoint.State = !validateState ? validation.Item2 : 0;
                }
                var resultEdit = endpointService.EditEndPoint(endPoint);
                Console.WriteLine($"EndPoint with Serial Number {resultEdit.SerialNumber} updated State to {resultEdit.State}");            
                return true;
            case "3":
                Console.WriteLine(Space());
                Console.WriteLine("Insert a Serial Number to remove a existing EndPoint");
                endPoint.SerialNumber = Console.ReadLine();
                endpointService.DeleteEndPoint(endPoint);
                Console.WriteLine($"Endpoint with SerialNumber - {endPoint.SerialNumber} successfully removed ");
                return true;
            case "4":
                var resultAll = endpointService.ListEndPointsAll();
                Console.WriteLine($"There are {resultAll.Count} endpoints");
                resultAll.ForEach(x => Console.WriteLine(x));           
                Console.WriteLine(Space());
                return true;
            case "5":
                Console.WriteLine(Space());
                Console.WriteLine("Insert a Serial Number to Find a EndPoint");
                endPoint.SerialNumber = Console.ReadLine();
                var result = endpointService.FindEndpoint(endPoint);
                Console.WriteLine($"EndPoint Atributes - [{result}]");
                return true;
            case "6":
                return false;
            default:
                System.Console.WriteLine("No option, try again!");
                return true;
        }
    }
    catch(EndPointException ex)
    {
        Console.WriteLine(new ErrModel(ex));      
        Console.WriteLine(Space());
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine(new ErrModel(ex));
        return false;
    }    
}
static string Space()
{
    return "*-------------------------------*";
}

