# ThreadPilot Intergration Layer

## Overview

This project builds the first version of an intergration between a new core system (**ThreadPilot**) and legacy systems. 

It consists of two Azure Function Apps written in .NET 8:

- **VehicleFunctionApp**: Takes a vehicle registration number and returns info about the vehicle.
- **InsuranceFunctionApp**: Takes a person’s ID and returns their insurances. If the person has car insurance, it fetches the vehicle info by calling the vehicle function.

The apps talk to each other via HTTP and are kept seperate for modularity.

---

## Architecture & Design

- Each Azure Function runs seperatly.
- Vehicle info is stored in memory (mocked).
- InsuranceFunction uses an injected `IVehicleClient` to call VehicleFunction, which makes it testable and flexible for the future.

---

## How to Run It
cd VehicleFunctionApp
func start --port 7071

cd ../InsuranceFunctionApp
func start --port 7072

### Requirements

- .NET 8 SDK
- Azure Function Core Tools v4+

### Steps


• At the end of your README, please include a short reflection (3–5 sentences)
on:
o Any similar project or experience you’ve had in the past.
I have worked with SJ functions for GDPR cleanse, build custom API cache, as well as use functions as help methods for Logic Apps and other heavy tasks

o What was challenging or interesting in this assignment.
Use .net 8, instead of .net 6 as previous experience. 

o What you would improve or extend if you had more time.
As a developer I would like to use API Management for exposing the function internally, providing more security for consumption, based on the needs of the project
I would add more Unit tests for the HTTP handling cases

In the future I would add Auth token handling such as JWT or work with API keys, and different APP registration roles based on the operation of each API call
HTTPS and Validation for inputs. 