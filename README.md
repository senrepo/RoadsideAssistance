# RoadsideAssistance


## Requirements

### EPIC(EP1) MAIN FLOW - Create a REST Api for customer main flow.
    * Step 1: Check if any available assistants near by where an assistance needed
    * Step 2: Customer creates a roadside assitance service request
    * Step 3: resserve an assitant
    * Step 4: Once the job done, release the assitant
    * Step 5: assistant update his location

    Out Of Scope
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

