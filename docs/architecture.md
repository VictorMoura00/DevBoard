# DevBoard Architecture

DevBoard starts as a modular monolith: one ASP.NET Core API process, one PostgreSQL database, and clear module boundaries for projects, tasks, and notifications.

The initial structure keeps infrastructure light so the MVP can run locally with few moving parts. RabbitMQ, YARP, separate services, and separate databases are planned evolutions, not startup dependencies.
