# 0001 - Start as a Modular Monolith

## Status

Accepted

## Context

The MVP needs to demonstrate project and task management with internal notifications while staying easy to run locally.

## Decision

DevBoard starts as a modular monolith with separate class libraries for each module and one API host.

## Consequences

The codebase can keep clear domain boundaries without adding distributed-system complexity before the product needs it.
