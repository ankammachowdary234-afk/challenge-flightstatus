export type UnifiedStatus = "OnTime" | "Delayed" | "Cancelled" | "Diverted" | "Unknown";

export interface FlightStatusResult {
  flightNumber: string;
  date: string;
  status: UnifiedStatus;
  scheduledDeparture: string | null;
  actualDeparture: string | null;
  scheduledArrival: string | null;
  actualArrival: string | null;
  terminal: string | null;
  gate: string | null;
  delayReason: string | null;
  message: string | null;
  lastUpdatedUtc: string;
  providerName: string;
}
