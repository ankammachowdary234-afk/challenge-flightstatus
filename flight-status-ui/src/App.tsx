import { useState } from "react";
import { fetchFlightStatus } from "./api";
import { StatusCard } from "./StatusCard";
import type { FlightStatusResult } from "./types";

const SAMPLE_FLIGHTS = ["AA100", "AA200", "AA300", "AA400", "AA500", "QF100", "QF200", "QF300", "XX999"];

export default function App() {
  const [flightNumber, setFlightNumber] = useState("");
  const [date, setDate] = useState("2024-06-15");
  const [result, setResult] = useState<FlightStatusResult | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  async function handleSearch(e: React.FormEvent) {
    e.preventDefault();
    setResult(null);
    setError(null);
    setLoading(true);
    try {
      const data = await fetchFlightStatus(flightNumber.trim(), date);
      setResult(data);
    } catch (err: unknown) {
      setError(err instanceof Error ? err.message : "Unexpected error");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div style={{
      minHeight: "100vh",
      background: "linear-gradient(135deg, #e0f2fe 0%, #bfdbfe 50%, #ddd6fe 100%)",
      display: "flex",
      flexDirection: "column",
      alignItems: "center",
      padding: "48px 16px",
      fontFamily: "-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif"
    }}>
      <div style={{ width: "100%", maxWidth: "520px" }}>
        {/* Header */}
        <div style={{ textAlign: "center", marginBottom: "32px" }}>
          <div style={{ fontSize: "48px", marginBottom: "8px" }}>✈️</div>
          <h1 style={{
            fontSize: "28px", fontWeight: 800, color: "#1e3a5f",
            margin: "0 0 8px", letterSpacing: "-0.5px"
          }}>SkyRoute Flight Status</h1>
          <p style={{ color: "#4a7fa5", fontSize: "15px", margin: 0 }}>
            Enter a flight number and date to check real-time status
          </p>
        </div>

        {/* Search Card */}
        <div style={{
          background: "white", borderRadius: "20px",
          boxShadow: "0 10px 40px rgba(0,0,0,0.12)",
          padding: "28px", marginBottom: "20px"
        }}>
          <form onSubmit={handleSearch}>
            {/* Flight Number */}
            <div style={{ marginBottom: "18px" }}>
              <label style={{ display: "block", fontSize: "13px", fontWeight: 600, color: "#374151", marginBottom: "6px" }}>
                FLIGHT NUMBER
              </label>
              <input
                type="text"
                value={flightNumber}
                onChange={(e) => setFlightNumber(e.target.value)}
                placeholder="e.g. AA100"
                required
                style={{
                  width: "100%", boxSizing: "border-box",
                  border: "2px solid #e5e7eb", borderRadius: "10px",
                  padding: "12px 14px", fontSize: "16px", fontWeight: 600,
                  color: "#111827", outline: "none", transition: "border 0.2s",
                  letterSpacing: "1px"
                }}
                onFocus={(e) => e.target.style.borderColor = "#3b82f6"}
                onBlur={(e) => e.target.style.borderColor = "#e5e7eb"}
              />
              {/* Quick select */}
              <div style={{ display: "flex", flexWrap: "wrap", gap: "6px", marginTop: "10px" }}>
                {SAMPLE_FLIGHTS.map((f) => (
                  <button
                    key={f}
                    type="button"
                    onClick={() => setFlightNumber(f)}
                    style={{
                      padding: "4px 12px", fontSize: "12px", fontWeight: 600,
                      background: flightNumber === f ? "#3b82f6" : "#eff6ff",
                      color: flightNumber === f ? "white" : "#1d4ed8",
                      border: "1px solid " + (flightNumber === f ? "#3b82f6" : "#bfdbfe"),
                      borderRadius: "20px", cursor: "pointer", transition: "all 0.15s"
                    }}
                  >{f}</button>
                ))}
              </div>
            </div>

            {/* Date */}
            <div style={{ marginBottom: "20px" }}>
              <label style={{ display: "block", fontSize: "13px", fontWeight: 600, color: "#374151", marginBottom: "6px" }}>
                DATE
              </label>
              <input
                type="date"
                value={date}
                onChange={(e) => setDate(e.target.value)}
                required
                style={{
                  width: "100%", boxSizing: "border-box",
                  border: "2px solid #e5e7eb", borderRadius: "10px",
                  padding: "12px 14px", fontSize: "15px", color: "#111827",
                  outline: "none", cursor: "pointer"
                }}
                onFocus={(e) => e.target.style.borderColor = "#3b82f6"}
                onBlur={(e) => e.target.style.borderColor = "#e5e7eb"}
              />
            </div>

            {/* Search Button */}
            <button
              type="submit"
              disabled={loading}
              style={{
                width: "100%", padding: "14px",
                background: loading ? "#93c5fd" : "linear-gradient(135deg, #3b82f6, #1d4ed8)",
                color: "white", border: "none", borderRadius: "12px",
                fontSize: "16px", fontWeight: 700, cursor: loading ? "not-allowed" : "pointer",
                boxShadow: "0 4px 15px rgba(59,130,246,0.4)",
                transition: "all 0.2s", letterSpacing: "0.5px"
              }}
            >
              {loading ? "🔍 Searching..." : "🔍 Search Flight"}
            </button>
          </form>
        </div>

        {/* Error */}
        {error && (
          <div style={{
            background: "#fef2f2", border: "2px solid #fca5a5",
            borderRadius: "14px", padding: "16px 20px", marginBottom: "20px",
            display: "flex", alignItems: "flex-start", gap: "10px"
          }}>
            <span style={{ fontSize: "20px" }}>⚠️</span>
            <div>
              <p style={{ margin: 0, fontWeight: 700, color: "#991b1b", fontSize: "14px" }}>Error</p>
              <p style={{ margin: "2px 0 0", color: "#b91c1c", fontSize: "14px" }}>{error}</p>
            </div>
          </div>
        )}

        {/* Result */}
        {result && <StatusCard result={result} />}
      </div>
    </div>
  );
}