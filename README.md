
# Proxy API Proof of Concept
One of our clients wants to create a Log Proxy API,which receives log messages and transfers them to third-party API. This project is a proof of concept of a backend system which accept log messages with fields "title" and “text" from the clients and transfers them to third-party API. It also enable the clients to query for existing log messages from third-party API. It contains an HTTP API for the clients to send log messages and recieve log messages from third-party API.

# Running Application

To run the application. Make sure you first have docker installed 

After that, you simply run

```bash
docker-compose up
```

After that you navigate to http://localhost:5000/swagger/index.html in your browser for the swagger documentation

Authenticate user with the url http://localhost:5000/api/account/authenticate and get the token for subsequent request.
There is in-memory users that is already registered to be used to get token. This is for demo purposes for production app we use database or implement with Auth0.

Sample Request body:
{
   "username": "test-proxy",
   "password": "password"
}

Copy the token and add it to Authorization header Bearer token in Postman or any other rest client.

HTTP GET request:  http://localhost:5000/api/messages returns all log messages.

HTTP GET request: http://localhost:5000/api/messages?maxRecords=10&view=Grid%20view returns log messages with filter based on query string on the url.

HTTP POST request:  http://localhost:5000/api/messages.

Sample Request body:
    {
      "messages": [
        {
          "title": "Test message summary1",
          "text": "Exceptiion yyy at ...1"
        },
        {
          "title": "Test message summary2",
          "text": "Exceptiion yyy at ...2"
        }
      ]
    }




