# 0002 - Use Wolverine for Messaging

## Status

Planned

## Context

The MVP will need command handlers and internal events such as task completion creating notifications.

## Decision

Wolverine is the planned mediator and messaging abstraction for commands, handlers, and internal events. RabbitMQ transport is deferred until notifications are extracted to a separate service.

## Consequences

The first implementation can use in-process messaging while preserving a natural path to external messaging later.
