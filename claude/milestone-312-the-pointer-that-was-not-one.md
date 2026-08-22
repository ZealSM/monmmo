# 312 — the pointer that was not one

**3563 tests green.** Base: 311, 3563. No number in the tutor reading moved; what moved is how
much of it is evidence.

311 ended with a loose end it called "something": `0x0823E140`, the runner-up candidate — fifteen
move ids, terminated by a nought, one aligned pointer, and *not* the tutor table. Chasing it took
one command and found two faults, both in conditions **I chose and called strong**.

---

## It is not a table of fifteen

Asked wider than the window the hunt used:

```
  0x23DF00  130   50  106  207
  0x23DF08  130   50  107  207
  0x23DF10  170   55  106  207
  0x23DF18  170   55  107  207
  ...
  0x23E140  170   54   57  207
  0x23E148  170   54  237  207
  0x23E150  170   54   68  207
  0x23E158  160   52   28    0
```

**Groups of FOUR.** A flat array of four-halfword movesets, with a nought where a record has fewer
than four moves. There is no terminator in it anywhere.

So the "fifteen entries ended by a nought" the hunt found is three whole records plus three
quarters of a fourth, and the "terminator" is that fourth record's **empty last slot**. The window
was fifteen because I asked for fifteen; the structure knows nothing about it.

That is a filter deciding the shape of its own answer, and the only reason it did not decide the
*result* is that something else was doing the work.

## And the thing doing the work was weaker than I said

311 printed *"pointed at by exactly one aligned word in sixteen megabytes"* as the condition that
took 81 candidates to 2. It sounds decisive. It is not: **four bytes anywhere in a graphics or
compression region can equal an address by accident**, and these do —

```
  0x245BB4  0x00000001
  0x245BB8  0x00000003
  0x245BBC  0x0823E140   <- the "pointer"
  0x245BC0  0x670B4E00
  0x245BC4  0xC8BBD0BF
```

**The right test was already in this repository.** 246 built it for a different question: a THUMB
`ldr rX, [pc, #imm]` whose arithmetic lands on exactly this word — five fixed bits that reach *this*
word, which only 2.4% of aligned words have.

```
  tutor table pointer at 0x120BE4: loaded by an instruction at 0x120BD6
  runner-up  "pointer" at 0x245BBC: loaded by nothing
```

The hunt now reads:

```
      by shape alone:               81 in the image,  11 in the reversal
      named by ONE aligned word:     2 in the image   <- what 311 quoted, and it is not enough
      ...that an INSTRUCTION loads:  1 in the image   <- 246's test, and the one that means anything
```

Both counts stay in the output permanently, because the weak one is the argument for the strong
one (25).

---

## What this does to 311

**Nothing in the answer, everything in the evidence.** The tutor table is still `0x08459B60`, still
fifteen halfwords, still confirmed by 14 of 15 tutors' own dialogue. What changes:

* The hunt gives **one** candidate, not two. The text confirmation is no longer needed to
  *discriminate* — it stays as an independent confirmation, which is what it was always worth.
* 311's runner-up was never a rival. It failed on **two** counts, each of which I had called a
  condition: it is not fifteen of anything, and nothing loads its address.
* And the discrimination 311 was proud of — *14 of 15 against the runner-up's 0* — was scoring a
  candidate that a stronger, already-built test would have thrown out before it got there. The
  score is still right. It was answering a question that should not have been asked.

**Two milestones running, a condition I chose has turned out weaker than the sentence I wrote
about it** (310's fixture compared against the leftover; this). The pattern is the same both
times: a condition that sounds like a filter, quoted as though its output were its strength.

---

## The guard

| break | predicted | killed |
|---|---|---|
| the instruction test is dropped, so any aligned word counts | 1 | 1 |

The fixture puts `ldr r0, [pc, #0]` four bytes before an aligned word and asserts `(1, 1)`, against
a bare word's `(1, 0)` and an unaligned one's `(0, 0)` — so a version that counts words and calls
them loaded fails, and so does one that stops counting words at all.

---

## What this leaves

* **The four-move array at `0x23DF00`-odd is unnamed.** It is a flat run of four-halfword movesets
  and nothing here says whose. It has the shape of a party record's moves; this milestone only
  needed it to be *not a table of fifteen*, and stopped there.
* **Every other table this project has located by "a pointer to it" should be re-asked with the
  instruction test.** `--the-species` at 302 hunted 462 four-aligned bases and reported them
  against a reversed-image floor of nought; whether any of them is *loaded* was never asked, and
  it is the same question this milestone just answered for the tutors.
* **The nought-terminator condition is still in `Hunt` and is still modelled.** It happens to be
  right for the tutor table, whose sixteenth halfword is nought. Nothing has shown it is a
  terminator rather than a coincidence of that particular table's neighbour.
