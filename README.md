MSShift

The solution contains several projects:
- `MessagingIPI` just a generated code. Did not continue on it. At least for now.
- `MessagingContracts` some basic common models.
- `MessagingProcessor` kinder main logic for now handling queues.
- `MessagingTest` some load simulation.
- `MessagingWebAPI` the Web API to accept requests. Only one request for now to submit a message.

I am using *RabbitMQ* for this. I ran it in a container with defaults using *Docker Desktop* with
`docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management`.

Three queues are in use:
- `messages` accepts requests to send a message.
- `messages_retry` a message is passed to this queue for retry.
- `messages_error` for *failed* message that did not qualify for retry.

There is no logic here for actually sending messages. There is no error handling either besides a little simulation of message processing to direct them to diffrent queues. I think, at least here, the `MessagingProcessor` is specific enough so it does not necessaraly calls for isolated implementation though it all will be required for unit testing etc. I showed some DI in the API service for `MessagingService` but the implementation is in the same project for simplicity at the moment. Wanted to do unit tests but do not have time for this. Did not get into UI for monitoring/managing. Do not have enough knowledge of RabbitMQ at the moment. For example, is there a persistance functionality. Also, RabbitMQ might not be a final approach anyways. Another big outstanding question is authentication.

I used *minimal API* everywhere to reduce amout of code for this excersize as well as to get some experience with it since it is quite new and I did not have many opportunities to use it yet.

In order to run:
1. start RabbitMQ
1. start MessagingProcessor
1. start MessagingWebAPI
1. use Swagger UI and/or launch MessagingTest
