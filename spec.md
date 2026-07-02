# Flight Status Tracker — Specification
> Committed before any implementation files.

## UnifiedStatus Enum
| Value     | Rule |
|-----------|------|
| OnTime    | Departure/arrival within 15 min of schedule |
| Delayed   | Departure/arrival beyond 15 min |
| Cancelled | Flight will not operate |
| Diverted  | Flight landed at different airport |
| Unknown   | No usable status from either provider |

## Merge Rules
1. Both providers respond → prefer later lastUpdatedUtc
2. Only one responds → use that result
3. Neither responds → Unknown with message

## API: GET /flights/status?flightNumber={code}&date={yyyy-MM-dd}
- 200: FlightStatusResult
- 400: missing flightNumber or date
