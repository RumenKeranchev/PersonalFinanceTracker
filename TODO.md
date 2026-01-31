# Migration:
Add Status (Pending, Completed, Cancelled) to transactions.

# General:
2.	When updating data, log only the changed fields with old and new values.
3.	Change pagination from index/page to cursor based pagination.
4.	Event Ids for exceptions?
5.	Don't use actual GUIDs in the API, use shortened ids instead.
6.	Implement rate limiting on the API.
7.	Implement caching for frequently accessed data.
11. Document:
	1. API Endpoints
	1. Versioning
	1. Results, their operators and errors
	1. User injection into services
12.	Containerize the application using Docker.
13. Testing:
	1. Add integration tests for all API endpoints.
	2. Add unit tests for all services and repositories.
	3. Implement end-to-end testing for critical user flows.
	4. Type generation (Target in Server.csproj)
14. Add a desktop client with MAUI and/or Blazor.
15. Use logging with open telemetry.
16. Implement health checks for the application.
17. Add unauthenticated layout with basic showcase of the application.