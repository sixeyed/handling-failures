# handling-failures
Demo code for the webinar 'Handling Failures with Messaging'

The Webinar was hosted by [@ParticularSW](http://particular.net/), the makers of NServiceBus, ServiceMatrix, ServicePulse etc.

It was presented on 18th August 2015 by me, [@EltonStoneman](http://blog.sixeyed.com), with help from @DanielMarbach and @MauroServienti.

# The Code

The demo solution is a simple Web App which connects to 3 different REST APIs. 

Using endpoints from [badapi.net](http://badapi.net), the APIs behave in a known way - one is reliable (returns 200), one gives a permanent failure (400), and the other is unreliable (returns 200 3% of the time, otherwise 503).

Different versions of the app handle these failures in different ways:

+ v1 - synchronous call from Web App to API; no error handling
+ v2 - synchronous call from Web App to API; basic error handling
+ v3 - asynchronous call, Web App sends message to queue & message handler calls APIs

Version 3 uses [ZeroMQ](http://zeromq.org/) as the transport, so there are no dependencies to set up.

## Usage

Build the solution, run the console app to start the handler, then browse to http://localhost/Sixeyed.HandlingFailures.Web/Confirmation.

## Next Steps

The Webinar focuses on how to scale your system, but there's much more to messaging. 

Check out my @Pluralsight course, [Message Queue Fundamentals in .NET](http://www.pluralsight.com/courses/message-queue-fundamentals-dotnet) for more patterns, practices, and lots more queueing technologies.


