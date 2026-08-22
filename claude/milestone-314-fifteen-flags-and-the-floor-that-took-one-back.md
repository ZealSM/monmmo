# 314 — fifteen flags, and the floor that took one back

**3571 tests green.** Base: 313, 3564.

306 found flag `0x0089` hiding somebody in MT. EMBER's doorway with eight maps behind them and
nothing in sixteen megabytes setting it, and offered three ways to answer: leave the door shut,
MODEL an opener, or **mark the fact in the world file by a derived rule**. The call went unmade
for eight milestones. This takes the third, because it is the one that hardcodes nothing.

---

## The mark goes on the person, not the door

Whether a door is fenced is a question about a **walk** — it needs a run, a lever setting and a
grid — and 211's rule keeps numbers that move with a lever out of a world record. Whether somebody
can ever be taken off the map needs none of them: a set flag hides them, so a hide flag nothing
can set is a person who is always there. The shut door follows, and it follows for anybody reading
the record rather than only for this project's walker.

**Four ways a hide flag gets set, and only one of them is a script.** The narrow rule would be "no
`setflag` names it", and 211 is the milestone about why that is wrong twice over:

| excluded | how it is known | mechanism |
|---|---|---|
| a script moves it | `EveryFlagMoved` — every byte of the image | `setflag` / `clearflag` |
| it hands something over | the object's own record | compiled: the routine that gives you the thing |
| a field move shifts it | the object's move, or `GatesThatAreObstacles` | compiled: the routine that shifts it |
| a new game has it on | the opening script's flags | not a mechanism — the state before anybody walks |

## 56 people, 15 flags, and it is not one door

```
  56 of those 605 are behind a flag NOTHING in the data sets
     against 355 whose flag a script moves, 76 hidden from the first frame,
             184 picked up and 158 shifted by a field move (overlapping, so these do not sum)

  15 flags: 0x002D 0x0038 0x003A 0x004E 0x0053 0x0056 0x0057 0x005B
            0x005E 0x0061 0x0071 0x0079 0x0089 0x008D 0x008F
```

The four exclusion counts are printed permanently beside the mark, because a count of people
nothing removes cannot be read without the count of people something does (25, 79, 313).

And the answer is a **class**, not a quirk:

```
      0x0053  31 people  SILPH CO., ten floors
      0x0079   8 people  THREE ISLAND and THREE ISLE PORT
      0x008D   3 people  ICEFALL CAVE
      0x003A   2 people  PALLET TOWN
      0x0089   2 people  MT. EMBER      <- 306's door
      …ROCKET HIDEOUT, DOTTED HOLE, ONE ISLAND, PEWTER, CELADON x2, SAFFRON
```

**306 counted one person and the flag hides two.** Those are different questions and the
difference matters: 306 asked which single person *fences the door* on `1.103` and the answer is
`1.97 person 3`. This asks who the flag can hide at all, and `1.97 person 2` is behind it too,
fencing nothing. The door reading stands; it was never a count of the flag's people.

Every one of these fifteen places is a scene the game's own event code runs, which is what the
mark's honest form says: **nothing in the data opens this.** Not *the game cannot* — compiled code
is a wall this project respects.

`WhyTheGatesAreShut`'s boundary bucket asks the same question of **one run's unmet gates** and
reads 35. This asks it of the **file** and reads 15 flags over 56 people, and by construction it
cannot move with a lever — which is the property that let it into a world record at all.

---

## The half the set sweep could not see, and the floor that took it back

A flag nothing sets is two very different things wearing one face: one the game's compiled code
owns — scripts still **read** it — or one nothing anywhere refers to. `EveryFlagAsked` is the
other half of `EveryFlagMoved`, same shape test, and it separated them:

```
  1 of those 15 are asked about by something shaped like a script
      0x0089 at 0x08DBA154
```

**One of fifteen, and it landed on exactly the flag 306 asked about.** A clean discrimination
pointing at the right person. It is also wrong:

```
  ...against 3 of the same 15 in the image REVERSED, which holds no scripts at all
```

**The finding reads below its own floor.** And the site says why —

```
  08DBA144  84 88 00 37 00 84 88 04 00 89 88 17 03 00 99 01
  08DBA154  2B 89 00 03 50 E7 30 03 E0 00 F7 03 BC 00 06 33
            ^^^^^^^^ ^^  checkflag 0x0089; end
  08DBA164  00 22 B2 BB AA 00 0A 0B 00 54 33 50 03 0B 00 18
```

`88 88`, `33 33`, `B3 B3` — compressed data. `checkflag` followed by `end` is two commands and
`ReadsAsAScript` is satisfied by two commands, so a sixteen-megabyte file produces these by the
handful. **Nothing reads any of the fifteen either.** They are dead in both directions.

Three milestones running a condition I chose has turned out weaker than the sentence I wrote
about it — 310's fixture compared against the leftover, 312's pointer was four bytes of graphics,
313's density was free where those bases lived. This is the fourth, and it is the first one the
control caught **inside the same milestone**, before the sentence reached a document. That is the
only difference worth anything: the floor was built at the same time as the number.

---

## The guards

| break | predicted | killed |
|---|---|---|
| the pickup exclusion is dropped from the rule | 2 | 2 |
| the asked sweep keeps hits that do not read as script | 1 | 1 |
| the mark is written to the world file as false | 1 | 1 |
| the floor is measured on the forward image | 1 | 1 |
| **the control**: the four exclusions in another order | **0** | **0** |

Five fixtures. The first names each of the five conditions **on its own**, after asserting the
base case — without that, a rule returning false for everything satisfies every negative. The
floor gets its own fixture because it is the number that overturned the finding, and a floor
nothing guards is a decoration sitting where the error bar is meant to be (205).

---

## What this leaves

* **Whether the mark means "always there" or "never there" turns on a reading it does not
  make.** It says nothing sets the flag; that this makes the person *present* needs the flag to
  start **clear**, which comes from `NewGameLocator` and its 49 flags. If the cartridge sets any
  of these fifteen from compiled code at the start of a game, the mark is exactly backwards for
  those people — they would be ones who are never there at all. Nothing here tests that, and the
  only evidence either way is indirect: the walker treats all 56 as present and solid, and 306's
  door was measured shut.
* **`0x0053` holds thirty-one people across ten floors of SILPH CO.** and nothing in this project
  knows what turns it. One flag doing that much work is the largest single unread lever the
  reading has found, and it is not a door — it is a building.
* **`ReadsAsAScript` is two commands.** It is the shape test under both flag sweeps and under
  much else, and this milestone is the first measurement of what it costs: three false flags out
  of fifteen in a file with no scripts in it. Nothing else that leans on it has been asked.
* **The world file is version 30.** Any file exported before this one will be refused rather than
  read short.
