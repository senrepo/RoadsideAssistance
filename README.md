# Roadside Assistance Application - Homework


## EPIC(ES1) Roadside Assistance Requirements
    Develop an application for the below functionality
        • Update location of a service provider
        • Return the nearest service trucks ordered by ascending distance
        • Reserve a service provider for a customer
        • Release a service provider from a customer

### FEATURE(FS1) MAIN FLOW - Create a REST Api for customer main flow.
    * Step 1: Check if any available assistants nearby where assistance required
    * Step 2: Customer creates a roadside assistance service request
    * Step 3: reserve an assistant
    * Step 4: Once the job done, release the assistant
    * Step 5: assistant update his location

    In-Scope
        * Validate the customer and assistant exists in the datastore
        * Api should enforce all the required field validation
        * Write the unit tests for the key functionality in Business logic layer and Api layer
        * Write the integration tests for key functionality in Business logic layer and Api layer

    Out-Of-Scope
        * Create a API for new customer and assistant are out of scope. 
        * Some hardcoded customers and assistants objects can be used.
        * Persistant store can be hardcoded for demo purpose

#### User Story(US1) STEP 1- Create a REST method for find nearest assistants   
    * The (x, y) coordinates are used to identify a location
    * The distance is calculated based on for formula -  Sqrt(power(X2-X1)^2 + power(Y2-Y1)^2)
    * The assistants are sorted based on the assistants distance from customer location
    * The occupied assistants should be excluded

    Input
        * limit - no of sorted assistants are returned
        * customer location - (x, y)
    Output
        * returns the assistants not greater than the limit

    Acceptance Criteria
        Api should return the available assistants not greater than the limit
        If no available assistants are found, then return empty

#### User Story(US2) STEP 3- Create a REST method for reserve an assistant
    * Customer creates a roadside assistance service request
    * Reserve an assistant

    Business Logic
        * System find 3 available assistants near by based customer location
        * System send notification message for this service request
        * System collects the notification response and auto allocate an assistant
        * System send the auto allocation notification to assistant, then he starts servicing the request
        * System will wait for 20 seconds, if no response from any assistant, then it cancel the operation
        * In case of no assistant reserved, then user has to request again.

    Input
        * customer - customer with a service request
        * customer location - (x, y)
    Output
        * returns a nearest available assistant 

    Acceptance Criteria
        * Customer should reserve an assistant
        * If no available assistants are found, then return empty

#### User Story(US3) STEP 4- Create a REST method for release an assistant   
    * Once the job done, release the assistant

    Input
        * customer - customer with a service request
        * assistant 
    Output
        * No content (Http 204) for successful operation

        Acceptance Criteria
        * The assistant will be released from customer once the job done

#### User Story(US4) STEP 5- Create a REST method for an assistant update his location    
    * Assistant should be able to update his location

    Input
        * assistant 
        * customer location - (x, y)
    Output
        * No content (Http 204) for successful operation

    Acceptance Criteria
        * Customer should be able to update his location and make it available in the assistants pool

## Technical Design
    API Layer
        Api Controllers
            * validate the input
            * call processor to process the request
            * convert the response to http content format
        Processors
            * validate the input with repository methods
            * mapping logic (DTO -> Modal vice versa)
        Repository
            * All business service call orchestration logic

    Business Logic Layer
        * Business logic components
        * Business service use the Datastore service for save, reterive and update logic

    DataStore Layer
        * Out of scope, use the hardcoded values

    Testing
        * Write unit tests and flow tests for key functionality

## Tech Stack Details
    .NET Core 6.0 
    ASP.NET MVC Controller Api
    Built-in Swagger
    .NET Core 6.0 Class Libarary
    Nunit
    Xunit
    Moq
    Serilog
    Microsoft Visual Studio Community 2022 (64-bit)
    Nuget

## Test Data for Swagger
    Run the RoadsideAssistanceApi from Visual Studio 2022
    Note: The port number might vary based on the debug profile

 ```json
     //POST: https://localhost:7027/api/roadsideAssistance/findNearestAssistants/3

    //Input
    {
      "x": 1,
      "y": 3
    }

    //Output
    [
      {
        "id": 3,
        "name": "Pawn",
        "isOccupied": false
      },
      {
        "id": 1,
        "name": "Rajesh",
        "isOccupied": false
      },
      {
        "id": 2,
        "name": "Muthu",
        "isOccupied": false
      }
    ]    
```
 ```json
    //POST: https://localhost:7027/api/roadsideAssistance/reserveAssistant

    //Input
    {
      "customer": {
        "id": 1,
        "name": "mike",
        "serviceRequest": {
          "id": 1,
          "problemDescription": "flat tire",
          "vehicle": {
            "licensePlate": "AC67024"
          }
        }
      },
      "geolocation": {
        "x": 1,
        "y": 3
      }
    }

    //Output
    {
      "id": 3,
      "name": "Pawn",
      "isOccupied": true
    }
```
 ```json
    //PUT: https://localhost:7027/api/roadsideAssistance/releaseAssistant

    //Input
    {
      "customer": {
        "id": 1,
        "name": "mike",
        "serviceRequest": {
          "id": 1,
          "problemDescription": "flat tire",
          "vehicle": {
            "licensePlate": "AC67024"
          }
        }
      },
      "assistant": {
	      "id": 3,
	      "name": "Pawn",
	      "isOccupied": true
	    }
    }

    //Output: Http 204 for success
```
 ```json
    //PUT: https://localhost:7027/api/roadsideAssistance/updateAssistantLocation

    //Input
    {
      "assistant": {
        "id": 3,
        "name": "Pawn"
      },
      "geolocation": {
        "x": 1,
        "y": 3
      }
    }
    //Output: Http 204 for success
```

## Unit Test Results
![TestResults](https://github.com/senrepo/RoadsideAssistance/blob/main/Readme/TestResults.PNG)

## Integration Test Results
![IntegrationTestResults](https://github.com/senrepo/RoadsideAssistance/blob/main/Readme/ConcurrentTestResults.PNG)

## More Logs
   Please refer the log file created in the log folder
![LogFile](https://github.com/senrepo/RoadsideAssistance/blob/main/Readme/logfile.PNG)
