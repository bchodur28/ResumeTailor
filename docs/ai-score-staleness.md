# AI Score Staleness

## Overview

`AiScoreStaleness` indicates how significantly a resume has changed since its current AI score was calculated.

The AI score represents the relevance of the generated resume to a specific job posting. Changes to resume content or content ordering may cause the existing score to no longer accurately represent the current resume.

## Staleness Levels

| Level | Description |
|---|---|
| `None` | No AI-relevant changes have been made since the score was calculated. |
| `Low` | A minor change was made that is unlikely to significantly affect the score. |
| `Medium` | A meaningful change was made to the presentation or prioritization of AI-evaluated content. |
| `High` | Resume content used by the AI analysis was added, removed, or materially changed. |

Staleness levels are ordered by severity:

`None < Low < Medium < High`

When multiple changes occur, the resume retains the highest detected staleness level.

A lower-severity change cannot reduce an existing higher staleness level.

---

## Staleness Rules

### General

| Change | Staleness | Reason |
|---|---|---|
| No AI-relevant change | `None` | Resume remains consistent with the content evaluated by the AI. |
| Resume name changed | `None` | Metadata only and does not affect resume relevance. |
| Job posting changed | `High` | The AI score is based on relevance to a specific job posting. |

### Company Experience

| Change | Staleness | Reason |
|---|---|---|
| Company reordered | `Medium` | Changes the prominence of relevant experience. |
| Company added | `High` | Introduces experience not considered by the existing AI analysis. |
| Company removed | `High` | Removes experience considered by the existing AI analysis. |

### Bullets

| Change | Staleness | Reason |
|---|---|---|
| Alternative bullet swapped with primary | `Low` | Both versions were generated as alternatives for the same accomplishment. |
| Bullet reordered | `Medium` | Changes the prominence of AI-ranked content. |
| Bullet manually edited | `High` | The displayed content differs from what the AI evaluated. |
| Bullet added | `High` | Introduces content not considered by the existing AI analysis. |
| Bullet removed | `High` | Removes content considered by the existing AI analysis. |
| Source bullet changed | `High` | The underlying accomplishment represented by the bullet has changed. |

### Education

| Change | Staleness | Reason |
|---|---|---|
| Education reordered | `Low` | Changes presentation with limited impact on overall relevance. |
| Education added | `Medium` | Introduces qualifications not considered by the existing analysis. |
| Education removed | `Medium` | Removes qualifications considered by the existing analysis. |
| Education content changed | `Medium` | Changes qualifications represented by the resume. |

### Projects

> Project staleness rules apply once projects participate in AI selection or scoring.

| Change | Staleness | Reason |
|---|---|---|
| Project reordered | `Medium` | Changes the prominence of relevant project experience. |
| Project added | `High` | Introduces AI-relevant content not considered by the existing analysis. |
| Project removed | `High` | Removes AI-relevant content considered by the existing analysis. |
| Project content changed | `High` | The project content differs from what the AI evaluated. |

---

## Multiple Changes

When multiple changes occur during the same update, the highest staleness level wins.

Examples:

| Changes | Result |
|---|---|
| Alternative bullet swap | `Low` |
| Alternative bullet swap + bullet reorder | `Medium` |
| Education change + bullet reorder | `Medium` |
| Bullet reorder + manual bullet edit | `High` |
| Company reorder + bullet deletion | `High` |

For example:

```csharp
resume.MarkAiScoreStale(AiScoreStaleness.Low);
resume.MarkAiScoreStale(AiScoreStaleness.Medium);
resume.MarkAiScoreStale(AiScoreStaleness.Low);
