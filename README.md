# Receipt Service
A C#/.NET implementation of a coding challenge to implement a receipt processor.

### Assumptions
The following assumptions were made from the requirements:
- All costs must be a positive value
- All Ids should be stored as a UUID data type
- All data is stored in-memory
- All requests to the API are assumed to have admin-level permission with access to all data
- User data is not tracked in any form
- The API does not handle any authentication or authorization
- This solution does not compose multiple microservices, but rather a single receipt service that would potentially be a part of a larger system 
