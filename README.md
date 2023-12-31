# Receipt Service
A C#/.NET implementation of a coding challenge to implement a receipt processor.

### Assumptions
The following assumptions were made from the requirements:
- All costs must be a positive value
- Points are calculated at process-time
- All Ids should be stored as a UUID data type
- All data is stored in-memory
- All requests to the API are assumed to have admin-level permission with access to all data
- User data is not tracked in any form
- The API does not handle any authentication or authorization
- This solution does not compose multiple microservices, but rather a single receipt service that would potentially be a part of a larger system

### Running the service
- Install [Docker](https://www.docker.com/)
- From the repository root, run the following commands:
  - `docker build -f Receipt.API/Dockerfile --tag jj-receipt-service .`
  - `docker run -p 1997:80 --detach jj-receipt-service`
- Use an http service to test the endpoints
  - [Insomnia](https://insomnia.rest/) files are provided in the repository for convenience