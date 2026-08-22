# 311 — fifteen people and a table

**3563 tests green.** Base: 310, 3557.

310 adopted the nought and the only flags that stopped being set were `0x02C0`–`0x02CE`: a
contiguous run, **not one of which hides any object**. It said so and left them unread. A
contiguous band of flags that gates nothing is a table, and this project had never named one.

They are one per **move tutor**.

---

## Found by shape, not by the band

The band is an *output*, so nothing here is filtered to produce fifteen. The rule is: a script the
map scan opens that puts a number in `0x8005` and hands it **straight to a routine** — a `call`
whose block's whole content is one `special`. Then **grouped by that routine**:

```
      routine  scripts  indices
      0x18D         15  0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14
```

One group. Fifteen scripts, one routine, and the indices are `0..14` with no holes.

```
      index  flag    map     what        script
          0  0x02C4  3.22    person 5    0x081C494E
          1  0x02C9  3.17    person 1    0x081C4B56
          2  0x02C5  3.22    person 6    0x081C49B6
          …
         14  0x02C8  3.7     person 15   0x081C4AEE

    The flags run 0x02C0..0x02CE — CONTIGUOUS. The indices run 0..14 — CONTIGUOUS.
```

**Two dense numberings over the same fifteen people, and they are not the same one**: `0x02C4` is
index 0 and `0x02C0` is index 4. One flag per person, one person per map — and the move each
teaches is in **none of the script**. The only thing that varies is `setvar 0x8005, N`, so what is
taught lives in compiled code, indexed by that.

---

## The table it indexes

Hunted by shape alone — a run of fifteen halfwords, every one a move id this cartridge has, ended
by a nought. **No move name is used to find it**, which is what lets the names confirm it.

```
      by shape alone:          81 in the image,     11 in the reversal
      and named by ONE word:    2 in the image
```

The pointer condition does the work and is printed apart from the one that does not (79). Two
candidates survive — and the tutors' own dialogue separates them:

```
      where        tutors whose own text names the move their index selects
      0x0823E140   0 of 15
      0x08459B60   14 of 15
```

**`0x08459B60`**, fifteen halfwords, terminated by nought, pointed at by exactly **one** aligned
word in sixteen megabytes — at `0x120BE4`, in code.

```
      index  move  name              map     its own text says the name
          0     5  MEGA PUNCH        3.22    yes
          1    14  SWORDS DANCE      3.17    yes
          2    25  MEGA KICK         3.22    yes
          3    34  BODY SLAM         35.3    yes
          4    38  DOUBLE-EDGE       1.40    yes
          5    68  COUNTER           10.2    yes
          6    69  SEISMIC TOSS      6.0     yes
          7   102  MIMIC             14.1    yes
          8   118  METRONOME         12.3    yes
          9   135  SOFTBOILED        3.6     yes
         10   138  DREAM EATER       3.1     NO — its own text never says the name
         11    86  THUNDER WAVE      1.48    yes
         12   153  EXPLOSION         1.97    yes
         13   157  ROCK SLIDE        1.82    yes
         14   164  SUBSTITUTE        3.7     yes
```

The table was found without reading a word of dialogue and the dialogue was written without
reference to any table, so this is two structures agreeing rather than one restated — 248's rule,
and the strongest form of it this project has had.

## The fifteenth is a PREDICTION

`3.1`'s tutor never names her move. Her text is:

> *"I had this weird dream about a DROWZEE eating my dream." … "And... I learned how to eat
> dreams..." … "Let me teach it to a POKeMON so I can forget about it!"*

The table says index 10 is **138**, and the move table — located for a different question
entirely — calls 138 **DREAM EATER**. The reading predicted a name the text withholds and an
independent table supplied it.

And the floor settles early: **one place in the image holds the first THREE entries**, and it is
this table. Everything after the third halfword is confirmation, not evidence.

---

## Two things it closed on the way

* **The fifteenth tutor is why no run reaches index 7.** `14.1 person 4` is COPYCAT, and her
  tutoring is behind a **POKé DOLL** — the item 197 filed as a reach problem and 198 showed is a
  money problem. The purse is modelled and the payout table has never been located, so the run
  cannot buy one, so MIMIC is the one flag in the band nothing sets. Three open items meeting at
  one person.

* **`0x0171` and `0x018D` are named.** They were the top two routines in 308's
  branch-went-the-other-way table, 28 of the 38 places. `0x018D` is the one handed the index —
  *teach the move*; `0x0171` is the one asked first, whose answer the run cannot have.

---

## The guards

| break | predicted | killed |
|---|---|---|
| any block counts as the routine's | 2 | **1** |
| the table's terminator is dropped | 1 | 1 |
| the index slot is not checked | 1 | 1 |
| **the control**: the allowed neighbours listed in another order | **0** | **0** |

The over-shoot is mine: a block asking *two* routines is refused by a different line than the one
I broke, so the second fixture could not have fired (32).

**And a fixture lied again, in a new shape.** `Assert.Empty` on the table hunt failed against a
fixture whose own bytes made a second table: a zero-filled image ends every run of ids with a
nought, so `14, 25, 9` at the next halfword passes. That is the nop-slide trap (fixtures lie #1)
pointed at a table hunt rather than a script read. Asserted on the address now, not the count.

---

## What this leaves

* **What `0x0171` answers is still compiled code** — the seventh wall of that kind. It is asked
  before the teaching and the run cannot have it.
* **The tutors' two numberings have no relation anybody has read.** The flag order is not the
  index order and neither is map order. Whether the flag band is in some third order — the order
  the tutors were written, say — is unasked.
* **`0x0823E140` is the runner-up and nobody looked at it.** It is fifteen move ids ended by a
  nought with one pointer to it, and it is not the tutor table. It is something, and this
  milestone only needed it to be *not this*.
* **Nothing reads the table.** It is located and printed; `MoveTutors` is not a thing the server
  or the client knows about, and teaching a move is not something the run can do.
