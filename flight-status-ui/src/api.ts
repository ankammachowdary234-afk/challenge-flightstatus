import type { FlightStatusResult } from "./types";

const API_BASE = "http://localhost:5050";

export async function fetchFlightStatus(
  flightNumber: string,
  date: string
): Promise<FlightStatusResult> {
  const url = `${API_BASE}/flights/status?flightNumber=${encodeURIComponent(flightNumber)}&date=${encodeURIComponent(date)}`;
  const res = await fetch(url);
  if (!res.ok) {
    const body = await res.json().catch(() => ({}));
    throw new Error(body.error ?? `Request failed with status ${res.status}`);
  }
  return res.json();
}