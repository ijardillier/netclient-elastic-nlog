# Context

The purpose of this application is to show how to integrate:

- logs (via NLog for Elastic)
- health checks (via Microsoft AspNetCore HealthChecks)
  
to an Elasticsearch cluster with .Net.

The Elastic packages dependencies are :

- Elastic.CommonSchema.NLog
- NLog.Targets.ElasticSearch