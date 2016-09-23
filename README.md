# Couchcase Demo app

This app is meant to demonstrate Couchbase's clustering, replication, and failover abilities.

## The expected setup

This app was made to demonstrate a 3-node cluster, with a bucket that has replicas enabled, and creates 2 replicas.

In Web.config, there are 4 settings:
* CouchbaseBucketName - This is the name of the bucket you want to use. The bucket should have at least a primary index.
* CouchbaseNode1,2,3 - These are the URIs to the three nodes in the cluster (e.g. "couchbase://localhost", "couchbase://192.168.1.10", etc). Technically, you only need one, but if you plan to unplug nodes at random, it's best if you have all three.

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

## I want to know more about Couchbase!

Check out [developer.couchbase.com](http://developer.couchbase.com) and [blog.couchbase.com](http://blog.couchbase.com)!

*Disclaimer: This application does not intend to show the ideal way to use Couchbase or even the Couchbase SDK.*

