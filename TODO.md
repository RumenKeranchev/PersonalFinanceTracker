# Migration:

# General:
2.	When updating data, log only the changed fields with old and new values.
3.	Change pagination from index/page to cursor based pagination.
4.	Event Ids for exceptions?
5.	Don't use actual GUIDs in the API, use shortened ids instead.
6.	Implement rate limiting on the API.
7.	Implement caching for frequently accessed data.
8.	Implement soft deletes for all entities.
9.  Store tokens in cookies?
10. Check if FinanceErrors are needed at all.
11. Document:
	1. API Endpoints
	1. Versioning
	1. Results, their operators and errors
12.	Containerize the application using Docker.