# 310 — two copies of one reading

**3557 tests green.** Base: 309, 3555.

309 left one line explicitly not re-run and 308 left one question open. Both are closed here, and
paying the first debt turned up something bigger than either.

---

## The debt, paid in one line

*"279 of the places, across 59 routines, have an answer nothing branches on"* — 309 could not check
it because it needs `SpecialCalls`' profiles joined to the run.

At its own row — `--play --say-yes --boat --surf --in-order` — it reads **279 of the places, across
59 routines.** Exact. So it belongs in 309's *held* population, and only the superlative moved.

**And it proves the line above it wrong from the inside.** The four buckets at that row are
`90 + 63 + 441 + 279 = 873`, and the line says **869**. A split contradicting its own total, in the
second consecutive line of the block, after 309 found the same fault in the why-shut buckets.

## Which made it worth counting the rest

* **The 18 signs the run reached the map for and never got to a wall of** are *"TEN on 12.0
  CINNABAR ISLAND in five adjacent PAIRS, 3 on 10.9, 2 on 14.2, and one each on 1.60 and 35.1"* —
  which is **17**. It is **ELEVEN** on 12.0: five pairs plus one more at `(3,1)`, a different
  address. The five-pairs observation was right; the eleventh was lost when the total was written.

* **And the `--namespaces` paragraph in the instruments section has FIVE stale numbers**, every one
  of which is *already correct in the block a few hundred lines below*:

  | the instruments section says | the instrument says |
  |---|---|
  | 238 flags, **236** variables | 238 and **238** |
  | floor of **1.71** | **1.73** |
  | whole image **2117 / 12659 / 1182** | **2117 / 14308 / 1333** |
  | **19** of the **90** variables never looked at | **7** of **115** |
  | **10** past the boundary, **9** nowhere | **5** and **2** |

  This is 224's rule — *a shared wrong list is worse than five private ones* — pointed at the
  prompt itself. The block has been right since 264. **A number written twice in one file is a
  number that will be corrected once.**

---

## And 308's open question, decided by the bytes

Every leftover 308 could not remove needed `--say-yes` and held the value **1** that lever writes
into `0x800D`. The place is always the same shape, and it is read rather than described:

```
    0x1C47B0  checkflag 0x02C0
    0x1C47B3  if EQUAL goto <already done>
    0x1C47B9  loadpointer ; callstd 0x05        <- the yes-or-no box
    0x1C47C1  compare 0x800D, 0                 <- legitimately reads the box
    0x1C47CC  call 0x081C4F37
    0x1C47D1  compare 0x800D, 0                 <- meant to read the CALL
```

and the called block, entire:

```
    0x1C4F37  0xC7 03 ; special 0x0171
    0x1C4F30  special 0x018D ; waitstate ; lock ; faceplayer2 ; return
```

**Its whole content is one `special`** — and `0x0171` and `0x018D` are 28 of the 38. So the compare
is meant to read that routine's answer. The run cannot have it and must fall back on a convention.

Its convention, stated since 214, is **nought**. The code left whatever was in the slot instead, so
at 38 places the run took the non-zero arm *because a different question had been answered yes
earlier in the same script*.

### Adopted

* **0 maps at every setting.**
* **0 gating flags at any setting** — `--the-floor`'s gates-set, never-set and all six bucket
  columns are byte-identical to 309's. Only *ever on*, *places* and *routines* moved.
* The flags it stops setting are `0x02C0`–`0x02CE`, a contiguous run, **not one of which hides
  anything**.

`--leave-the-slot` is the pre-310 behaviour and stays, because a control the reader cannot re-run
is not a control (241).

---

## Two things about my own work

**The correction removed the instrument's own subject.** With nought written there is never an
unanswered slot to record, so `--the-answer-slot`'s table went to nought at every setting — the
reading it exists to produce, invisible. It measures the phenomenon under `--leave-the-slot` now,
with the run as it stands as the row that MUST be nought.

**And a fixture I wrote the same hour lied.** A break writing **1** instead of nought came back
GREEN, because the fixture's compare was against **5** — and 1 and 0 are both Less than 5, so every
row read the same. 13's costume. Compared against **nought** the three cases separate, and the same
break kills two.

| break | predicted | killed |
|---|---|---|
| the default flips back to leaving the slot | 1 | 1 |
| it writes ONE instead of nought | 2 | **0** → fixture fixed → **2** |

---

## What this leaves

* **The block has more splits than I checked.** Three found in two milestones, all by adding the
  columns up. Nothing sweeps for them; a command that parses this block and checks its own
  arithmetic is a real instrument and does not exist.
* **The instruments section restates the block in a dozen places.** Only `--namespaces` was
  checked. The rest is the same shape and was not swept.
* **`0x02C0`–`0x02CE` is a contiguous run of flags that hides nothing**, set by scripts that ask
  `0x0171` and `0x018D`. What the band IS has not been read — it looks like per-person
  bookkeeping, and this project has never named one.
* **`--the-answer-slot` now makes 21 runs** and `--the-floor` seven plus a whole-image sweep. Both
  are earning it; neither is fast.
