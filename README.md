# Couchcase Demo app

This app is meant to demonstrate Couchbase's clustering, replication, and failover abilities.

## The expected setup

This app was made to demonstrate a 3-node cluster, with a bucket that has replicas enabled, and creates 2 replicas.

The bucket name is defined in `HomeController.cs`
The URIs to the Couchbase Node(s) are defined in `Startup.cs`
Normally, these settings would go into appsettings.json or something like that.

## What is demonstrated?

This application operates on an idea of 10 "magic" documents, which have well-known keys doc0, doc1, ..., and doc9.

The idea is to create these documents on a cluster of three nodes, and then see what happens when one of the nodes goes down:

* No failover - replicas are read-only, and writes cannot be performed until the node comes back online
* Manual failover - replicas are read-only, and writes cannot be performed until the node is failed over and the cluster is rebalanced (done by hand using the Couchbase)
* Auto failover - replicas are read-only, and writes cannot be performed until the node is detected as down and the cluster automatically fails it over (a matter of seconds)

## How is it demonstrated?

When you run the app, you will see a dashboard:

* Total documents in the bucket - this will just show a count of ALL documents
* Magic Ten - this will show the status/values of the "magic" 10 documents
  * If there is an active document, it will show the value in green
  * If there is not an active document, it will show the status/error message in red
  * If there is a replica of the document, it will show the value in green
  * If there is not a replica of the document, it will show the status in yellow

There are some operations you can use to demonstrate/respond to changes to the cluster.

* Reset Magic 10: This will recreate the "magic 10" documents back to their initial state.
* Add 10 arbitrary documents: To show how new incoming writes work before/during/after a change to the cluster
* Delete all arbitrary documents: Reset any documents that you've created (to reset the demo perhaps)
* Update all magic 10: Perform a 'replace' on each magic document. This will add/update an "updated" field on each document with a timestamp. If there are any errors, they will be listed in an "errors" panel on the dashboard (e.g. when trying to update a document that has not yet been failed-over)

## Running on Docker

If you don't have 3 tiny computers in a box, you can still run and use this app in Docker. I have included a Dockerfile. You'll need 2+ Couchbase Server nodes also running in docker.

For the first node:

`docker run -d --name db -p 8091:8091 couchbase`

Just to make things easier, port 8091 is what you can use to connect to this node and setup a cluster via a web browser.

For subsequent nodes:

`docker run -d --name db2 couchbase`
`docker run -d --name db3 couchbase`
`... etc ...`

You don't need to map ports on any other nodes, since they'll only be communicating with eachother and the web app within the docker host.

Once you've got that setup, get the IP addresses (the ones internal to the host) and enter then into the appropriate setup in Startup.cs. (For more information, check out my [blog post on Docker with .NET Core](http://blog.couchbase.com/2016/november/docker-and-asp.net-core-with-couchbase-server)).

To build the image of the ASP.NET Core website on Docker:

`docker build -t webcouchcase bin\Debug\netcoreapp1.1\publish -t webcouchcase`

Notice this is webcouch*c*ase with a "c"! Then to start the website:

`docker run -d --name web -p 8880:80 webcouchcase`

Which you would access via http://localhost:8880 (you can specify a different port number in `docker run` if you want).

## I want to know more about Couchbase!

Check out [developer.couchbase.com](http://developer.couchbase.com) and [blog.couchbase.com](http://blog.couchbase.com)!

*Disclaimer: This application does not intend to show the ideal way to use Couchbase or even the Couchbase SDK.*

