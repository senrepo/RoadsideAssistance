# Roadside Assistance Demo Application


## EPIC(ES1) Roadside Assistance Requirements
    Develop an application for below functionality
        • Update location of a service provider
        • Return the nearest service trucks ordered by ascending distance
        • Reserve a service provider for a customer
        • Release a service provider from a customer

    ### FEATURE(FS1) MAIN FLOW - Create a REST Api for customer main flow.
        * Step 1: Check if any available assistants near by where an assistance needed
        * Step 2: Customer creates a roadside assitance service request
        * Step 3: resserve an assitant
        * Step 4: Once the job done, release the assitant
        * Step 5: assistant update his location

        In-Scope
           * Validate the customer and assitant exists in the datastore
           * Api should enforece all the required field validation
           * Write the unit tests for the key functionality in Business logic layer and Api layer
           * Write the integration tests for key functionlity in Business logic layer and Api layer

        Out-Of-Scope
            Creating a API for new customer and assitant. Some hardcoded customers and assitants objects can be used.

         #### User Story(US1) STEP 1- Create a REST method for find nearest assistants   
            * The (x, y) coordinates are used to identify a location
            * The distance is calculated based on for formula -  Sqrt(power(X2-X1)^2 + power(Y2-Y1)^2)
            * The assitatns are sorted based on the assitants distance from customer location
            * The occupied assitatants should be excluded

            Input
                * limit - no of sorted assitants are retured
                * customer location - (x, y)
            Output
                * returns the assitatns not greater than the limit

            Acceptance Criteria
                Api should return the available assitants not greater than the limit
                If no available assitants are found, then return empty

         #### User Story(US2) STEP 3- Create a REST method for reserve an assistant
            * Customer creates a roadside assitance service request
            * Resserve an assitant

            Input
                * customer - customer with a service request
                * customer location - (x, y)
            Output
                * returns a nearest available assitant 

            Acceptance Criteria
                * Customer should reserve an assitant
                * If no available assitants are found, then return empty

            Business Logic
                * System find 3 available assistants near by based customer location
                * System send notification message for this service request
                * System collects the notification response and auto allocate an assistant
                * System send the auto allocation notification to assitant, then he starts servicing the request
                * System will wait for 20 seconds, if no response from any assistant, then it cancel the operation
                * In case of no assitant reserved, then user has to request again.

         #### User Story(US3) STEP 4- Create a REST method for release an assistant   
            * Once the job done, release the assitant

            Input
                * customer - customer with a service request
                * assitant 
            Output
                * No content (Http 204) for successful operation

             Acceptance Criteria
                * The assitant will be released from customer once the job done


         #### User Story(US4) STEP 5- Create a REST method for an assistant update his location    
            * Assistant should be able ot update his location

            Input
                * assitant 
                * customer location - (x, y)
            Output
                * No content (Http 204) for successful operation

            Acceptance Criteria
                * Customer should be able to update his location and make it available in the assistants pool

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


## Test Data for Swagger
    Run the RoadsideAssistanceApi from Visual Studio 2022
    Note: The port number might vary based on the debug profile

    POST: https://localhost:7027/api/roadsideAssistance/findNearestAssistants/3
    Input:
    {
      "x": 1,
      "y": 3
    }

    POST: https://localhost:7027/api/roadsideAssistance/reserveAssistant
    Input:
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

    PUT: https://localhost:7027/api/roadsideAssistance/releaseAssistant
    Input:
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
	      "id": 2,
	      "name": "Muthu",
	      "isOccupied": true
	    }
    }

    PUT: https://localhost:7027/api/roadsideAssistance/updateAssistantLocation
    Input:
    {
      "assistant": {
        "id": 2,
        "name": "Muthu"
      },
      "geolocation": {
        "x": 1,
        "y": 3
      }
    }

## Unit Test Results

## Integration Test Results

## More Logs
   Please refer the log file created in the log folder
