# prompts.md — AI Tooling Log

Tool used: **GitHub Copilot (VS Code IDE-integrated)**

---

## 1. Analysis & Design Phase

**Prompt:**
> "Read the flight status challenge brief and help me design the data models and interface contracts before writing any code. Focus on the merge rules and normalisation logic."

**Decision:** Chose a `ProviderResult` internal model (pre-normalisation) separate from `FlightStatusResult` (API response). This isolates provider-specific vocabulary from the unified contract and makes the normaliser a pure function — easy to test.

---

## 2. Status Normalisation

**Prompt:**
> "Implement a static StatusNormaliser that maps both AeroTrack (ON_TIME, DELAYED, CANCELLED, DIVERTED, UNKNOWN) and QuickFlight (ontime, delay, cancel, divert) vocabularies to the UnifiedStatus enum. Fall back to time-based calculation for unknown strings."

**Decision:** Used a single switch expression with `ToUpperInvariant()` to handle both vocabularies in one place, then fell back to time-diff logic. Keeps it simple and testable.

---

## 3. Merge Logic

**Prompt:**
> "Implement the merge: run both providers in parallel with Task.WhenAll, pick the one with the later lastUpdatedUtc, handle null results."

**Decision:** `Task.WhenAll` for true parallel fan-out. Ordering by `lastUpdatedUtc` descending and taking `First()` is the simplest correct implementation of the merge rule.

---

## 4. Stub Design

**Prompt:**
> "Design deterministic stubs that cover OnTime, Delayed, Cancelled, Diverted, Unknown, AeroTrack-only, QuickFlight-only, and both-providers-merge scenarios."

**Decision:** Used in-memory dictionaries keyed by flight number. QF100/QF200 are shared across both stubs with different timestamps to exercise the merge logic.

---

## 5. Unit Tests

**Prompt:**
> "Write xUnit tests using Moq for: merge prefers newer timestamp, single-provider fallback, neither-provider returns Unknown, normalisation of all status strings, time-based boundary at exactly 15 minutes."

**Decision:** Kept tests focused on behaviour (merge rules, normalisation) rather than implementation details. 16 tests, all meaningful.

---

## 6. React Frontend

**Prompt:**
> "Build a React+TypeScript search form for flight number and date, a StatusCard with Tailwind colour coding (green=OnTime, amber=Delayed, red=Cancelled/Diverted, grey=Unknown), AeroTrack-only fields shown conditionally, and a clear error state."

**Decision:** Colour coding via a `STATUS_CONFIG` map keeps the card component data-driven and easy to extend. Quick-select buttons for sample flights improve demo experience.
