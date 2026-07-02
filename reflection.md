# reflection.md — What I Would Improve With More Time

## Architecture
- **Real provider HTTP clients** — replace stubs with actual HTTP calls wrapped in `IFlightStatusProvider`, using `HttpClientFactory` and Polly for resilience (retries, circuit breaker, timeouts).
- **Response caching** — cache provider responses for ~30 seconds to avoid hammering upstream APIs on repeated lookups for the same flight.
- **Structured logging** — add `ILogger<T>` throughout with correlation IDs to trace a single request across both provider calls.

## Testing
- **Integration tests** — use `WebApplicationFactory<Program>` (ASP.NET TestServer) to test the full request pipeline, not just unit-level components.
- **Contract tests** — add provider-level tests that verify the stub data covers every merge/normalisation scenario explicitly.
- **Frontend tests** — add Vitest + React Testing Library tests for the StatusCard and search form.

## Resilience
- **Provider timeout handling** — current implementation will hang if a provider is slow. Would add `CancellationToken` timeouts per provider and treat a timeout as a null result.
- **Partial failure UX** — show which provider failed/timed out alongside the result.

## UX & Accessibility
- **Loading skeleton** — replace the "Searching…" text with a skeleton card for better perceived performance.
- **Keyboard accessibility** — ensure the quick-select flight buttons are navigable by keyboard.
- **Date validation** — warn users when the date has no stub data instead of returning a confusing Unknown result.

## Delivery
- **Docker Compose** — a single `docker-compose up` that starts both API and UI would eliminate the manual two-terminal setup.
- **CI pipeline** — GitHub Actions running `dotnet test` and `tsc --noEmit` on every push.
