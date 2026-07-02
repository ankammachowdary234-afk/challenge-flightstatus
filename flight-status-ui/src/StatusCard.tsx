import type { FlightStatusResult, UnifiedStatus } from "./types";

const STATUS_CONFIG: Record<UnifiedStatus, { label: string; emoji: string; bg: string; border: string; badge: string; badgeText: string }> = {
  OnTime:    { label: "On Time",   emoji: "✅", bg: "#f0fdf4", border: "#86efac", badge: "#dcfce7", badgeText: "#166534" },
  Delayed:   { label: "Delayed",   emoji: "🕐", bg: "#fffbeb", border: "#fcd34d", badge: "#fef3c7", badgeText: "#92400e" },
  Cancelled: { label: "Cancelled", emoji: "❌", bg: "#fff1f2", border: "#fca5a5", badge: "#fee2e2", badgeText: "#991b1b" },
  Diverted:  { label: "Diverted",  emoji: "��", bg: "#fff1f2", border: "#fca5a5", badge: "#fee2e2", badgeText: "#991b1b" },
  Unknown:   { label: "Unknown",   emoji: "❓", bg: "#f9fafb", border: "#d1d5db", badge: "#f3f4f6", badgeText: "#4b5563" },
};

function fmt(dt: string | null) {
  if (!dt) return "—";
  return new Date(dt).toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" }) + " UTC";
}

function Row({ label, value }: { label: string; value: string }) {
  return (
    <div style={{ padding: "10px 0", borderBottom: "1px solid #f0f0f0" }}>
      <div style={{ fontSize: "11px", fontWeight: 700, color: "#9ca3af", textTransform: "uppercase", letterSpacing: "0.5px", marginBottom: "2px" }}>{label}</div>
      <div style={{ fontSize: "15px", fontWeight: 600, color: "#111827" }}>{value}</div>
    </div>
  );
}

interface Props { result: FlightStatusResult }

export function StatusCard({ result }: Props) {
  const cfg = STATUS_CONFIG[result.status];
  return (
    <div style={{
      background: cfg.bg,
      border: "2px solid " + cfg.border,
      borderRadius: "20px",
      boxShadow: "0 8px 30px rgba(0,0,0,0.10)",
      overflow: "hidden"
    }}>
      {/* Header bar */}
      <div style={{
        background: cfg.border,
        padding: "16px 24px",
        display: "flex", alignItems: "center", justifyContent: "space-between"
      }}>
        <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
          <span style={{ fontSize: "28px" }}>{cfg.emoji}</span>
          <div>
            <div style={{ fontSize: "22px", fontWeight: 900, color: "#111827", letterSpacing: "1px" }}>
              {result.flightNumber}
            </div>
            <div style={{ fontSize: "13px", color: "#374151" }}>{result.date}</div>
          </div>
        </div>
        <div style={{
          background: cfg.badge,
          color: cfg.badgeText,
          padding: "6px 18px",
          borderRadius: "30px",
          fontSize: "14px",
          fontWeight: 800,
          border: "2px solid " + cfg.badgeText + "33"
        }}>
          {cfg.label}
        </div>
      </div>

      {/* Body */}
      <div style={{ padding: "20px 24px" }}>

        {result.message && (
          <div style={{
            background: "#f3f4f6", borderRadius: "10px",
            padding: "12px 16px", marginBottom: "16px",
            fontSize: "14px", color: "#6b7280", fontStyle: "italic"
          }}>
            {result.message}
          </div>
        )}

        {/* Times grid */}
        <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "0 24px" }}>
          <Row label="Scheduled Departure" value={fmt(result.scheduledDeparture)} />
          <Row label="Actual Departure"    value={fmt(result.actualDeparture)} />
          <Row label="Scheduled Arrival"   value={fmt(result.scheduledArrival)} />
          <Row label="Actual Arrival"      value={fmt(result.actualArrival)} />
        </div>

        {/* AeroTrack extras */}
        {(result.terminal || result.gate || result.delayReason) && (
          <div style={{
            marginTop: "16px", padding: "14px 16px",
            background: "white", borderRadius: "12px",
            border: "1px solid #e5e7eb"
          }}>
            <div style={{ fontSize: "11px", fontWeight: 700, color: "#9ca3af", textTransform: "uppercase", letterSpacing: "0.5px", marginBottom: "10px" }}>
              Gate Information
            </div>
            <div style={{ display: "flex", gap: "24px", flexWrap: "wrap" }}>
              {result.terminal && (
                <div>
                  <div style={{ fontSize: "11px", color: "#9ca3af", fontWeight: 600 }}>TERMINAL</div>
                  <div style={{ fontSize: "20px", fontWeight: 900, color: "#1d4ed8" }}>{result.terminal}</div>
                </div>
              )}
              {result.gate && (
                <div>
                  <div style={{ fontSize: "11px", color: "#9ca3af", fontWeight: 600 }}>GATE</div>
                  <div style={{ fontSize: "20px", fontWeight: 900, color: "#1d4ed8" }}>{result.gate}</div>
                </div>
              )}
              {result.delayReason && (
                <div style={{ flexBasis: "100%" }}>
                  <div style={{ fontSize: "11px", color: "#9ca3af", fontWeight: 600 }}>DELAY REASON</div>
                  <div style={{ fontSize: "14px", fontWeight: 600, color: "#b45309" }}>{result.delayReason}</div>
                </div>
              )}
            </div>
          </div>
        )}

        {/* Footer */}
        <div style={{
          marginTop: "16px", paddingTop: "12px",
          borderTop: "1px solid #e5e7eb",
          display: "flex", justifyContent: "space-between",
          fontSize: "12px", color: "#9ca3af"
        }}>
          <span>Provider: <strong style={{ color: "#4b5563" }}>{result.providerName}</strong></span>
          <span>Updated: <strong style={{ color: "#4b5563" }}>{fmt(result.lastUpdatedUtc)}</strong></span>
        </div>
      </div>
    </div>
  );
}